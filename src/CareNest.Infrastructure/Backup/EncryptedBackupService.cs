using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Infrastructure.Configuration;
using CareNest.Infrastructure.Persistence;
using CareNest.Shared;
using SQLite;

namespace CareNest.Infrastructure.Backup;

public sealed class EncryptedBackupService(
    SqliteDatabase database,
    ICareNestRepository repository,
    CareNestStorageOptions options,
    ISecretStore secretStore,
    TimeProvider timeProvider) : IBackupService
{
    private static readonly byte[] Magic = "CNBK"u8.ToArray();
    private static readonly byte[] PayloadMagic = "CBPL"u8.ToArray();
    private static readonly byte[] Aad = "CareNest.Backup.v1"u8.ToArray();
    private const int SaltSize = 16;
    private const int Iterations = 250_000;

    public async Task CreateEncryptedBackupAsync(
        Stream destination,
        string password,
        string appVersion,
        CancellationToken cancellationToken = default)
    {
        ValidatePassword(password);
        options.EnsureDirectories();

        var work = Path.Combine(options.WorkingDirectory, $"backup-{Guid.NewGuid():N}");
        Directory.CreateDirectory(work);
        var snapshot = Path.Combine(work, "carenest.db");
        var archive = Path.Combine(work, "payload.zip");

        try
        {
            await database.CreateSnapshotAsync(snapshot, cancellationToken);
            var schema = await repository.GetSchemaVersionAsync(cancellationToken);
            var storedDocuments = Directory.Exists(options.DocumentDirectory)
                ? Directory.EnumerateFiles(options.DocumentDirectory, "*.cndoc", SearchOption.TopDirectoryOnly).ToArray()
                : [];

            var documentKey = await secretStore.GetBytesAsync(SecretKeys.DocumentMasterKey, cancellationToken);
            try
            {
                if (storedDocuments.Length > 0 && documentKey is not { Length: 32 })
                {
                    throw new InvalidOperationException("The document encryption key is unavailable, so a complete backup cannot be created.");
                }

                var manifest = new BackupManifest(
                    AppConstants.BackupFormatVersion,
                    schema,
                    timeProvider.GetUtcNow().UtcDateTime,
                    appVersion,
                    storedDocuments.Length);

                using (var zip = ZipFile.Open(archive, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(snapshot, "database/carenest.db", CompressionLevel.Optimal);

                    var manifestEntry = zip.CreateEntry("manifest.json", CompressionLevel.Optimal);
                    await using (var manifestStream = manifestEntry.Open())
                    {
                        await JsonSerializer.SerializeAsync(manifestStream, manifest, cancellationToken: cancellationToken);
                    }

                    if (documentKey is { Length: 32 })
                    {
                        var keyEntry = zip.CreateEntry("secrets/document-master-key.bin", CompressionLevel.NoCompression);
                        await using var keyStream = keyEntry.Open();
                        await keyStream.WriteAsync(documentKey, cancellationToken);
                    }

                    foreach (var documentPath in storedDocuments)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        zip.CreateEntryFromFile(
                            documentPath,
                            $"documents/{Path.GetFileName(documentPath)}",
                            CompressionLevel.NoCompression);
                    }
                }

                var salt = RandomNumberGenerator.GetBytes(SaltSize);
                var key = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    Iterations,
                    HashAlgorithmName.SHA256,
                    32);

                try
                {
                    await destination.WriteAsync(Magic, cancellationToken);
                    await destination.WriteAsync(new byte[] { 1 }, cancellationToken);
                    await destination.WriteAsync(salt, cancellationToken);

                    await using var archiveStream = File.OpenRead(archive);
                    await Security.ChunkedAead.EncryptAsync(
                        archiveStream,
                        destination,
                        key,
                        PayloadMagic,
                        Aad,
                        cancellationToken);
                }
                finally
                {
                    CryptographicOperations.ZeroMemory(key);
                    CryptographicOperations.ZeroMemory(salt);
                }

                await repository.CreateBackupMetadataAsync(new BackupMetadata
                {
                    FormatVersion = AppConstants.BackupFormatVersion,
                    SchemaVersion = schema,
                    CreatedAtUtc = manifest.CreatedUtc,
                    AppVersion = appVersion,
                    DestinationHint = "User-selected destination"
                }, cancellationToken);
            }
            finally
            {
                if (documentKey is not null)
                {
                    CryptographicOperations.ZeroMemory(documentKey);
                }
            }
        }
        finally
        {
            TryDeleteDirectory(work);
        }
    }

    public async Task<BackupInspection> InspectAsync(
        Stream source,
        string password,
        CancellationToken cancellationToken = default)
    {
        ValidatePassword(password);
        var work = Path.Combine(options.WorkingDirectory, $"inspect-{Guid.NewGuid():N}");
        Directory.CreateDirectory(work);

        try
        {
            var archive = Path.Combine(work, "payload.zip");
            await DecryptArchiveAsync(source, password, archive, cancellationToken);
            var manifest = await ReadAndValidateArchiveAsync(
                archive,
                work,
                extract: false,
                cancellationToken);

            return new BackupInspection(
                manifest.FormatVersion,
                manifest.SchemaVersion,
                manifest.CreatedUtc,
                manifest.AppVersion,
                manifest.DocumentCount);
        }
        finally
        {
            TryDeleteDirectory(work);
        }
    }

    public async Task RestoreEncryptedBackupAsync(
        Stream source,
        string password,
        CancellationToken cancellationToken = default)
    {
        ValidatePassword(password);
        options.EnsureDirectories();

        var work = Path.Combine(options.WorkingDirectory, $"restore-{Guid.NewGuid():N}");
        Directory.CreateDirectory(work);

        try
        {
            var archive = Path.Combine(work, "payload.zip");
            var extracted = Path.Combine(work, "extracted");
            Directory.CreateDirectory(extracted);

            await DecryptArchiveAsync(source, password, archive, cancellationToken);
            var manifest = await ReadAndValidateArchiveAsync(
                archive,
                extracted,
                extract: true,
                cancellationToken);

            var restoredDb = Path.Combine(extracted, "database", "carenest.db");
            await ValidateDatabaseAsync(restoredDb, cancellationToken);

            var restoredDocs = Path.Combine(extracted, "documents");
            var stagedDocs = Path.Combine(work, "staged-documents");
            Directory.CreateDirectory(stagedDocs);

            if (Directory.Exists(restoredDocs))
            {
                foreach (var file in Directory.EnumerateFiles(restoredDocs, "*.cndoc", SearchOption.TopDirectoryOnly))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    File.Copy(file, Path.Combine(stagedDocs, Path.GetFileName(file)), overwrite: true);
                }
            }

            byte[]? restoredDocumentKey = null;
            var restoredKeyPath = Path.Combine(extracted, "secrets", "document-master-key.bin");
            if (File.Exists(restoredKeyPath))
            {
                restoredDocumentKey = await File.ReadAllBytesAsync(restoredKeyPath, cancellationToken);
            }
            if (manifest.DocumentCount > 0 && restoredDocumentKey is not { Length: 32 })
            {
                throw new InvalidDataException("Backup document encryption key is missing or invalid.");
            }

            var oldDocumentKey = await secretStore.GetBytesAsync(SecretKeys.DocumentMasterKey, cancellationToken);
            var oldDocs = options.DocumentDirectory + ".pre-restore";
            TryDeleteDirectory(oldDocs);

            if (Directory.Exists(options.DocumentDirectory))
            {
                Directory.Move(options.DocumentDirectory, oldDocs);
            }

            try
            {
                Directory.Move(stagedDocs, options.DocumentDirectory);

                if (restoredDocumentKey is { Length: 32 })
                {
                    await secretStore.SetBytesAsync(SecretKeys.DocumentMasterKey, restoredDocumentKey, cancellationToken);
                }
                else
                {
                    await secretStore.RemoveAsync(SecretKeys.DocumentMasterKey, cancellationToken);
                }

                await database.ReplaceDatabaseAsync(restoredDb, cancellationToken);
                TryDeleteDirectory(oldDocs);
            }
            catch
            {
                TryDeleteDirectory(options.DocumentDirectory);
                if (Directory.Exists(oldDocs))
                {
                    Directory.Move(oldDocs, options.DocumentDirectory);
                }

                if (oldDocumentKey is { Length: 32 })
                {
                    await secretStore.SetBytesAsync(SecretKeys.DocumentMasterKey, oldDocumentKey, CancellationToken.None);
                }
                else
                {
                    await secretStore.RemoveAsync(SecretKeys.DocumentMasterKey, CancellationToken.None);
                }

                throw;
            }
            finally
            {
                if (restoredDocumentKey is not null)
                {
                    CryptographicOperations.ZeroMemory(restoredDocumentKey);
                }
                if (oldDocumentKey is not null)
                {
                    CryptographicOperations.ZeroMemory(oldDocumentKey);
                }
            }

            await repository.AddAuditEntryAsync(new AuditEntry
            {
                EntityType = "Backup",
                EntityId = Guid.NewGuid().ToString("N"),
                Action = Domain.Enums.AuditAction.Restored,
                EventUtc = timeProvider.GetUtcNow().UtcDateTime,
                SafeSummary = $"Backup format {manifest.FormatVersion} restored"
            }, cancellationToken);
        }
        finally
        {
            TryDeleteDirectory(work);
        }
    }

    private static async Task DecryptArchiveAsync(
        Stream source,
        string password,
        string archivePath,
        CancellationToken cancellationToken)
    {
        var header = new byte[Magic.Length];
        await ReadExactlyAsync(source, header, cancellationToken);
        if (!CryptographicOperations.FixedTimeEquals(header, Magic))
        {
            throw new InvalidDataException("This is not a supported CareNest backup.");
        }

        var version = source.ReadByte();
        if (version != 1)
        {
            throw new InvalidDataException("Backup encryption version is unsupported.");
        }

        var salt = new byte[SaltSize];
        await ReadExactlyAsync(source, salt, cancellationToken);
        var key = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            32);

        try
        {
            await using var output = File.Create(archivePath);
            await Security.ChunkedAead.DecryptAsync(
                source,
                output,
                key,
                PayloadMagic,
                Aad,
                cancellationToken);
        }
        catch (CryptographicException)
        {
            throw new InvalidDataException("Backup password is incorrect or the backup has been modified.");
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
            CryptographicOperations.ZeroMemory(salt);
        }
    }

    private static async Task<BackupManifest> ReadAndValidateArchiveAsync(
        string archivePath,
        string target,
        bool extract,
        CancellationToken cancellationToken)
    {
        using var zip = ZipFile.OpenRead(archivePath);
        var manifestEntry = zip.GetEntry("manifest.json")
            ?? throw new InvalidDataException("Backup manifest is missing.");

        BackupManifest? manifest;
        await using (var stream = manifestEntry.Open())
        {
            manifest = await JsonSerializer.DeserializeAsync<BackupManifest>(
                stream,
                cancellationToken: cancellationToken);
        }

        if (manifest is null)
        {
            throw new InvalidDataException("Backup manifest is invalid.");
        }

        BackupArchiveValidator.ValidateTopology(zip, manifest);

        if (extract)
        {
            foreach (var entry in zip.Entries)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrEmpty(entry.Name))
                {
                    continue;
                }

                var destination = Path.GetFullPath(Path.Combine(target, entry.FullName));
                var root = Path.GetFullPath(target) + Path.DirectorySeparatorChar;

                if (!destination.StartsWith(root, StringComparison.Ordinal))
                {
                    throw new InvalidDataException("Backup contains an unsafe path.");
                }

                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                entry.ExtractToFile(destination, overwrite: true);
            }
        }

        return manifest;
    }

    private static async Task ValidateDatabaseAsync(
        string databasePath,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var connection = new SQLiteAsyncConnection(
            databasePath,
            SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);

        try
        {
            var integrity = await connection.ExecuteScalarAsync<string>("PRAGMA integrity_check;");
            if (!string.Equals(integrity, "ok", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException("Backup database failed integrity validation.");
            }

            var version = await connection.ExecuteScalarAsync<int>(
                "SELECT COALESCE(MAX(Version), 0) FROM SchemaInfo;");

            if (version <= 0)
            {
                throw new InvalidDataException("Backup database schema version is missing.");
            }
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            throw new ArgumentException(
                "Backup password must contain at least 8 characters.",
                nameof(password));
        }
    }

    private static async Task ReadExactlyAsync(
        Stream stream,
        Memory<byte> buffer,
        CancellationToken cancellationToken)
    {
        var total = 0;
        while (total < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer[total..], cancellationToken);
            if (read == 0)
            {
                throw new EndOfStreamException();
            }
            total += read;
        }
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
        catch
        {
            // Best-effort cleanup. Never log user file names or document contents here.
        }
    }
}
