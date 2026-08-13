using System.Security.Cryptography;
using CareNest.Application.Contracts;
using CareNest.Infrastructure.Configuration;
using CareNest.Shared;

namespace CareNest.Infrastructure.Documents;

public sealed class EncryptedDocumentStore(
    CareNestStorageOptions options,
    ISecretStore secretStore) : IDocumentStore
{
    private static readonly byte[] Magic = "CNDC"u8.ToArray();
    private static readonly byte[] Aad = "CareNest.Document.v1"u8.ToArray();

    public async Task<StoredDocument> ImportAsync(
        Stream source,
        string originalFileName,
        string? contentType,
        CancellationToken cancellationToken = default)
    {
        options.EnsureDirectories();
        var key = await GetOrCreateKeyAsync(cancellationToken);
        try
        {
            var encryptedFileName = $"{Guid.NewGuid():N}.cndoc";
            var outputPath = Path.Combine(options.DocumentDirectory, encryptedFileName);
            var tempPlain = Path.Combine(options.WorkingDirectory, $"{Guid.NewGuid():N}.import");
            Directory.CreateDirectory(options.WorkingDirectory);

            long size;
            string hash;
            try
            {
                await using (var temp = File.Create(tempPlain))
                using (var hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256))
                {
                    var buffer = new byte[64 * 1024];
                    size = 0;
                    while (true)
                    {
                        var read = await source.ReadAsync(buffer, cancellationToken);
                        if (read == 0)
                        {
                            break;
                        }

                        size += read;
                        if (size > 512L * 1024 * 1024)
                        {
                            throw new InvalidDataException("Document exceeds the 512 MB safety limit.");
                        }

                        hasher.AppendData(buffer, 0, read);
                        await temp.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
                    }
                    hash = Convert.ToHexString(hasher.GetHashAndReset()).ToLowerInvariant();
                }

                await using var plain = File.OpenRead(tempPlain);
                await using var encrypted = File.Create(outputPath);
                await Security.ChunkedAead.EncryptAsync(plain, encrypted, key, Magic, Aad, cancellationToken);
                return new StoredDocument(encryptedFileName, size, hash, 1);
            }
            catch
            {
                if (File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }
                throw;
            }
            finally
            {
                if (File.Exists(tempPlain))
                {
                    File.Delete(tempPlain);
                }
            }
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
        }
    }

    public async Task ExportDecryptedAsync(string encryptedFileName, Stream destination, CancellationToken cancellationToken = default)
    {
        var safeName = Path.GetFileName(encryptedFileName);
        var path = Path.Combine(options.DocumentDirectory, safeName);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Encrypted document file is missing.");
        }

        var key = await GetOrCreateKeyAsync(cancellationToken);
        try
        {
            await using var source = File.OpenRead(path);
            await Security.ChunkedAead.DecryptAsync(source, destination, key, Magic, Aad, cancellationToken);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
        }
    }

    public Task DeleteAsync(string encryptedFileName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var safeName = Path.GetFileName(encryptedFileName);
        var path = Path.Combine(options.DocumentDirectory, safeName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        return Task.CompletedTask;
    }

    public Task<long> GetStorageUsageBytesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!Directory.Exists(options.DocumentDirectory))
        {
            return Task.FromResult(0L);
        }

        var total = Directory.EnumerateFiles(options.DocumentDirectory, "*.cndoc", SearchOption.TopDirectoryOnly)
            .Sum(file => new FileInfo(file).Length);
        return Task.FromResult(total);
    }

    public Task<IReadOnlyList<string>> ListStoredFilesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!Directory.Exists(options.DocumentDirectory))
        {
            return Task.FromResult<IReadOnlyList<string>>([]);
        }

        IReadOnlyList<string> result = Directory
            .EnumerateFiles(options.DocumentDirectory, "*.cndoc", SearchOption.TopDirectoryOnly)
            .Select(Path.GetFileName)
            .Where(x => x is not null)
            .Cast<string>()
            .ToArray();

        return Task.FromResult(result);
    }

    private async Task<byte[]> GetOrCreateKeyAsync(CancellationToken cancellationToken)
    {
        var existing = await secretStore.GetBytesAsync(SecretKeys.DocumentMasterKey, cancellationToken);
        if (existing is { Length: 32 })
        {
            return existing;
        }

        if (existing is not null)
        {
            CryptographicOperations.ZeroMemory(existing);
        }

        var key = RandomNumberGenerator.GetBytes(32);
        try
        {
            await secretStore.SetBytesAsync(SecretKeys.DocumentMasterKey, key, cancellationToken);
            return key;
        }
        catch
        {
            CryptographicOperations.ZeroMemory(key);
            throw;
        }
    }
}
