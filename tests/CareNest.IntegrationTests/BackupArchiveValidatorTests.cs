using System.IO.Compression;
using CareNest.Infrastructure.Backup;
using CareNest.Shared;

namespace CareNest.IntegrationTests;

public sealed class BackupArchiveValidatorTests
{
    [Fact]
    public void ValidTopology_WithoutDocuments_IsAccepted()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1));
        var manifest = Manifest(documentCount: 0);

        BackupArchiveValidator.ValidateTopology(archive, manifest);
    }

    [Fact]
    public void ValidTopology_WithTopLevelDocumentAndKey_IsAccepted()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("secrets/document-master-key.bin", 32),
            ("documents/abc.cndoc", 8));
        var manifest = Manifest(documentCount: 1);

        BackupArchiveValidator.ValidateTopology(archive, manifest);
    }

    [Fact]
    public void NestedDocumentEntry_IsRejected()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("secrets/document-master-key.bin", 32),
            ("documents/nested/abc.cndoc", 8));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(archive, Manifest(documentCount: 1)));
    }

    [Fact]
    public void NonCareNestDocumentExtension_IsRejected()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("secrets/document-master-key.bin", 32),
            ("documents/abc.txt", 8));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(archive, Manifest(documentCount: 1)));
    }

    [Fact]
    public void UnexpectedFileEntry_IsRejected()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("unexpected.bin", 8));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(archive, Manifest(documentCount: 0)));
    }

    [Fact]
    public void DuplicateFileEntry_IsRejected()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("database/carenest.db", 1));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(archive, Manifest(documentCount: 0)));
    }

    [Fact]
    public void ManifestDocumentCountMismatch_IsRejected()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("secrets/document-master-key.bin", 32),
            ("documents/abc.cndoc", 8));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(archive, Manifest(documentCount: 2)));
    }

    [Fact]
    public void MissingDocumentKey_WhenDocumentsExist_IsRejected()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("documents/abc.cndoc", 8));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(archive, Manifest(documentCount: 1)));
    }

    [Fact]
    public void InvalidDocumentKeyLength_IsRejectedEvenWithoutDocuments()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("secrets/document-master-key.bin", 31));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(archive, Manifest(documentCount: 0)));
    }

    [Fact]
    public void InvalidSchemaVersion_IsRejected()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1));
        var manifest = new BackupManifest(
            AppConstants.BackupFormatVersion,
            0,
            new DateTime(2026, 8, 13, 9, 30, 0, DateTimeKind.Utc),
            "test",
            0);

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(archive, manifest));
    }

    [Fact]
    public void NegativeDocumentCount_IsRejected()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1));
        var manifest = new BackupManifest(
            AppConstants.BackupFormatVersion,
            5,
            new DateTime(2026, 8, 13, 9, 30, 0, DateTimeKind.Utc),
            "test",
            -1);

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(archive, manifest));
    }

    [Fact]
    public void OversizedManifest_IsRejectedBeforeParsing()
    {
        using var archive = CreateArchive(
            ("manifest.json", 9),
            ("database/carenest.db", 1));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateBeforeManifest(archive, Limits(maxManifestBytes: 8)));
    }

    [Fact]
    public void OversizedDatabase_IsRejectedBeforeExtraction()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 9));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateBeforeManifest(archive, Limits(maxDatabaseBytes: 8)));
    }

    [Fact]
    public void OversizedDocument_IsRejectedBeforeExtraction()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("secrets/document-master-key.bin", 32),
            ("documents/abc.cndoc", 9));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateBeforeManifest(archive, Limits(maxDocumentBytes: 8)));
    }

    [Fact]
    public void ExcessiveUncompressedPayload_IsRejectedBeforeExtraction()
    {
        using var archive = CreateArchive(
            ("manifest.json", 4),
            ("database/carenest.db", 4));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateBeforeManifest(archive, Limits(maxTotalUncompressedBytes: 7)));
    }

    [Fact]
    public void ExcessiveArchiveEntryCount_IsRejectedBeforeManifestParsing()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("secrets/document-master-key.bin", 32),
            ("documents/a.cndoc", 1),
            ("documents/b.cndoc", 1));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateBeforeManifest(archive, Limits(maxDocumentCount: 1)));
    }

    [Fact]
    public void DirectoryArchiveEntry_IsRejectedBeforeManifestParsing()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1),
            ("documents/", 0));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateBeforeManifest(archive, Limits()));
    }

    [Fact]
    public void ManifestDocumentCountBeyondLimit_IsRejected()
    {
        using var archive = CreateArchive(
            ("manifest.json", 1),
            ("database/carenest.db", 1));

        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateTopology(
                archive,
                Manifest(documentCount: 2),
                Limits(maxDocumentCount: 1)));
    }

    [Fact]
    public void OversizedDecryptedArchiveContainer_IsRejected()
    {
        Assert.Throws<InvalidDataException>(() =>
            BackupArchiveValidator.ValidateContainerLength(
                archiveLength: 9,
                Limits(maxDecryptedArchiveBytes: 8)));
    }

    [Fact]
    public void DecryptedArchiveContainerAtLimit_IsAccepted()
    {
        BackupArchiveValidator.ValidateContainerLength(
            archiveLength: 8,
            Limits(maxDecryptedArchiveBytes: 8));
    }

    private static BackupManifest Manifest(int documentCount) => new(
        AppConstants.BackupFormatVersion,
        5,
        new DateTime(2026, 8, 13, 9, 30, 0, DateTimeKind.Utc),
        "test",
        documentCount);

    private static BackupArchiveLimits Limits(
        long maxManifestBytes = 64,
        long maxDatabaseBytes = 64,
        long maxDocumentBytes = 64,
        long maxTotalUncompressedBytes = 256,
        int maxDocumentCount = 10,
        long maxDecryptedArchiveBytes = 512) => new(
            maxManifestBytes,
            maxDatabaseBytes,
            maxDocumentBytes,
            maxTotalUncompressedBytes,
            maxDocumentCount,
            maxDecryptedArchiveBytes);

    private static ZipArchive CreateArchive(params (string Name, int Length)[] entries)
    {
        var memory = new MemoryStream();
        using (var writer = new ZipArchive(memory, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var (name, length) in entries)
            {
                var entry = writer.CreateEntry(name, CompressionLevel.NoCompression);
                using var stream = entry.Open();
                stream.Write(new byte[length]);
            }
        }

        memory.Position = 0;
        return new ZipArchive(memory, ZipArchiveMode.Read, leaveOpen: false);
    }
}
