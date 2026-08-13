using System.IO.Compression;
using CareNest.Shared;

namespace CareNest.Infrastructure.Backup;

internal static class BackupArchiveValidator
{
    private const string ManifestEntryName = "manifest.json";
    private const string DatabaseEntryName = "database/carenest.db";
    private const string DocumentKeyEntryName = "secrets/document-master-key.bin";
    private const string DocumentsPrefix = "documents/";

    public static void ValidateTopology(ZipArchive zip, BackupManifest manifest)
    {
        ArgumentNullException.ThrowIfNull(zip);
        ArgumentNullException.ThrowIfNull(manifest);

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

        var fileEntries = zip.Entries
            .Where(entry => !string.IsNullOrEmpty(entry.Name))
            .ToArray();

        var duplicate = fileEntries
            .GroupBy(entry => entry.FullName, StringComparer.Ordinal)
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
        {
            throw new InvalidDataException("Backup contains duplicate archive entries.");
        }

        if (!fileEntries.Any(entry => string.Equals(entry.FullName, ManifestEntryName, StringComparison.Ordinal)))
        {
            throw new InvalidDataException("Backup manifest is missing.");
        }

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

    private static bool IsTopLevelDocumentEntry(string fullName)
    {
        if (!fullName.StartsWith(DocumentsPrefix, StringComparison.Ordinal))
        {
            return false;
        }

        var fileName = fullName[DocumentsPrefix.Length..];
        return fileName.Length > 0 &&
               !fileName.Contains('/', StringComparison.Ordinal) &&
               !fileName.Contains('\\', StringComparison.Ordinal) &&
               fileName.EndsWith(".cndoc", StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Path.GetFileName(fileName), fileName, StringComparison.Ordinal);
    }
}
