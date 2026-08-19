using System.IO.Compression;
using CareNest.Shared;

namespace CareNest.Infrastructure.Backup;

internal sealed record BackupArchiveLimits(
    long MaxManifestBytes,
    long MaxDatabaseBytes,
    long MaxDocumentBytes,
    long MaxTotalUncompressedBytes,
    int MaxDocumentCount,
    long MaxDecryptedArchiveBytes = 2304L * 1024 * 1024)
{
    public static BackupArchiveLimits Default { get; } = new(
        MaxManifestBytes: 1L * 1024 * 1024,
        MaxDatabaseBytes: 1L * 1024 * 1024 * 1024,
        MaxDocumentBytes: 512L * 1024 * 1024,
        MaxTotalUncompressedBytes: 2L * 1024 * 1024 * 1024,
        MaxDocumentCount: 5_000,
        MaxDecryptedArchiveBytes: 2304L * 1024 * 1024);

    public void EnsureValid()
    {
        if (MaxManifestBytes <= 0 ||
            MaxDatabaseBytes <= 0 ||
            MaxDocumentBytes <= 0 ||
            MaxTotalUncompressedBytes <= 0 ||
            MaxDocumentCount < 0 ||
            MaxDecryptedArchiveBytes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(BackupArchiveLimits), "Backup archive limits must be positive.");
        }
    }
}

internal static class BackupArchiveValidator
{
    private const string ManifestEntryName = "manifest.json";
    private const string DatabaseEntryName = "database/carenest.db";
    private const string DocumentKeyEntryName = "secrets/document-master-key.bin";
    private const string DocumentsPrefix = "documents/";

    public static void ValidateContainerLength(
        long archiveLength,
        BackupArchiveLimits? limits = null)
    {
        limits ??= BackupArchiveLimits.Default;
        limits.EnsureValid();

        if (archiveLength < 0 || archiveLength > limits.MaxDecryptedArchiveBytes)
        {
            throw new InvalidDataException("Backup decrypted archive is too large.");
        }
    }

    public static void ValidateBeforeManifest(
        ZipArchive zip,
        BackupArchiveLimits? limits = null)
    {
        ArgumentNullException.ThrowIfNull(zip);
        limits ??= BackupArchiveLimits.Default;
        limits.EnsureValid();

        var archiveEntries = zip.Entries.ToArray();
        var maximumArchiveEntries = checked(limits.MaxDocumentCount + 3);
        if (archiveEntries.Length > maximumArchiveEntries)
        {
            throw new InvalidDataException("Backup contains too many archive entries.");
        }

        if (archiveEntries.Any(entry => string.IsNullOrEmpty(entry.Name)))
        {
            throw new InvalidDataException("Backup contains unsupported directory archive entries.");
        }

        var fileEntries = archiveEntries;
        ValidateDuplicateEntries(fileEntries);

        var manifestEntry = fileEntries.SingleOrDefault(entry =>
            string.Equals(entry.FullName, ManifestEntryName, StringComparison.Ordinal))
            ?? throw new InvalidDataException("Backup manifest is missing.");

        ValidateEntryLength(manifestEntry, limits.MaxManifestBytes, "Backup manifest is too large.");

        long totalUncompressedBytes = 0;
        foreach (var entry in fileEntries)
        {
            try
            {
                totalUncompressedBytes = checked(totalUncompressedBytes + entry.Length);
            }
            catch (OverflowException)
            {
                throw new InvalidDataException("Backup uncompressed size is invalid.");
            }

            if (totalUncompressedBytes > limits.MaxTotalUncompressedBytes)
            {
                throw new InvalidDataException("Backup uncompressed payload is too large.");
            }

            if (string.Equals(entry.FullName, DatabaseEntryName, StringComparison.Ordinal))
            {
                ValidateEntryLength(entry, limits.MaxDatabaseBytes, "Backup database is too large.");
            }
            else if (IsTopLevelDocumentEntry(entry.FullName))
            {
                ValidateEntryLength(entry, limits.MaxDocumentBytes, "Backup document is too large.");
            }
        }
    }

    public static void ValidateTopology(
        ZipArchive zip,
        BackupManifest manifest,
        BackupArchiveLimits? limits = null)
    {
        ArgumentNullException.ThrowIfNull(zip);
        ArgumentNullException.ThrowIfNull(manifest);
        limits ??= BackupArchiveLimits.Default;
        limits.EnsureValid();

        ValidateBeforeManifest(zip, limits);

        if (manifest.FormatVersion != AppConstants.BackupFormatVersion)
        {
            throw new InvalidDataException("Backup format is unsupported.");
        }

        if (manifest.SchemaVersion <= 0)
        {
            throw new InvalidDataException("Backup schema version is invalid.");
        }

        if (manifest.DocumentCount < 0)
        {
            throw new InvalidDataException("Backup document count is invalid.");
        }

        if (manifest.DocumentCount > limits.MaxDocumentCount)
        {
            throw new InvalidDataException("Backup document count exceeds the supported limit.");
        }

        var fileEntries = zip.Entries.ToArray();

        if (!fileEntries.Any(entry => string.Equals(entry.FullName, DatabaseEntryName, StringComparison.Ordinal)))
        {
            throw new InvalidDataException("Backup database is missing.");
        }

        var documentEntries = new List<ZipArchiveEntry>();
        foreach (var entry in fileEntries)
        {
            if (string.Equals(entry.FullName, ManifestEntryName, StringComparison.Ordinal) ||
                string.Equals(entry.FullName, DatabaseEntryName, StringComparison.Ordinal) ||
                string.Equals(entry.FullName, DocumentKeyEntryName, StringComparison.Ordinal))
            {
                continue;
            }

            if (!IsTopLevelDocumentEntry(entry.FullName))
            {
                throw new InvalidDataException("Backup contains an unexpected or unsafe archive entry.");
            }

            documentEntries.Add(entry);
        }

        if (documentEntries.Count != manifest.DocumentCount)
        {
            throw new InvalidDataException("Backup document manifest does not match the archive.");
        }

        var keyEntry = fileEntries.SingleOrDefault(entry =>
            string.Equals(entry.FullName, DocumentKeyEntryName, StringComparison.Ordinal));
        if (manifest.DocumentCount > 0 && (keyEntry is null || keyEntry.Length != 32))
        {
            throw new InvalidDataException("Backup document encryption key is missing or invalid.");
        }

        if (keyEntry is not null && keyEntry.Length != 32)
        {
            throw new InvalidDataException("Backup document encryption key is invalid.");
        }
    }

    private static void ValidateDuplicateEntries(ZipArchiveEntry[] fileEntries)
    {
        var duplicate = fileEntries
            .GroupBy(entry => entry.FullName, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
        {
            throw new InvalidDataException("Backup contains duplicate archive entries.");
        }
    }

    private static void ValidateEntryLength(ZipArchiveEntry entry, long maximumBytes, string message)
    {
        if (entry.Length < 0 || entry.Length > maximumBytes)
        {
            throw new InvalidDataException(message);
        }
    }

    private static bool IsTopLevelDocumentEntry(string fullName)
    {
        if (!fullName.StartsWith(DocumentsPrefix, StringComparison.Ordinal))
        {
            return false;
        }

        var fileName = fullName[DocumentsPrefix.Length..];
        return fileName.Length > 0 &&
               !fileName.Contains('/') &&
               !fileName.Contains('\\') &&
               fileName.EndsWith(".cndoc", StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Path.GetFileName(fileName), fileName, StringComparison.Ordinal);
    }
}
