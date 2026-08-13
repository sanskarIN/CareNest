using System.Text;
using CareNest.Application.Contracts;
using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.UnitTests.TestDoubles;

namespace CareNest.UnitTests;

public sealed class DocumentServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 9, 30, 0, TimeSpan.Zero);

    [Fact]
    public async Task ImportAsync_Success_PersistsEncryptedMetadataAndAudit()
    {
        var repository = new RecordingRepository();
        var store = new DocumentStoreSpy
        {
            ImportResult = new StoredDocument("vault-item.cndoc", 5, "abc123", 1)
        };
        var service = new DocumentService(repository, store, new FixedTimeProvider(Now));
        var picked = Picked("report.pdf", "application/pdf", "hello");

        var document = await service.ImportAsync(
            "profile-1",
            "Report",
            DocumentCategory.LabReport,
            "User note",
            picked);

        Assert.Same(document, repository.SavedDocument);
        Assert.Equal("vault-item.cndoc", document.EncryptedFileName);
        Assert.Equal("report.pdf", document.OriginalFileName);
        Assert.Equal("application/pdf", document.ContentType);
        Assert.Equal(5, document.OriginalSizeBytes);
        Assert.Equal("abc123", document.Sha256);
        Assert.Equal(Now.UtcDateTime, document.CreatedUtc);
        Assert.Equal(Encoding.UTF8.GetBytes("hello"), store.LastImportedBytes);
        var audit = Assert.Single(repository.AuditEntries);
        Assert.Equal(AuditAction.Created, audit.Action);
        Assert.Equal(document.Id, audit.EntityId);
    }

    [Fact]
    public async Task ImportAsync_RecordSaveFails_RemovesEncryptedPayload()
    {
        var repository = new RecordingRepository { ThrowOnSave = true };
        var store = new DocumentStoreSpy
        {
            ImportResult = new StoredDocument("orphan.cndoc", 5, "abc123", 1)
        };
        var service = new DocumentService(repository, store, new FixedTimeProvider(Now));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ImportAsync(
            "profile-1",
            "Report",
            DocumentCategory.Custom,
            null,
            Picked("report.bin", "application/octet-stream", "hello")));

        Assert.Contains("orphan.cndoc", store.DeletedFiles);
        Assert.Empty(repository.DeletedDocumentIds);
    }

    [Fact]
    public async Task ImportAsync_AuditFails_RollsBackRecordAndEncryptedPayload()
    {
        var repository = new RecordingRepository { ThrowOnAudit = true };
        var store = new DocumentStoreSpy
        {
            ImportResult = new StoredDocument("rollback.cndoc", 5, "abc123", 1)
        };
        var service = new DocumentService(repository, store, new FixedTimeProvider(Now));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ImportAsync(
            "profile-1",
            "Report",
            DocumentCategory.Custom,
            null,
            Picked("report.bin", "application/octet-stream", "hello")));

        var saved = Assert.IsType<CareDocument>(repository.SavedDocument);
        Assert.Contains(saved.Id, repository.DeletedDocumentIds);
        Assert.Contains("rollback.cndoc", store.DeletedFiles);
    }

    [Fact]
    public async Task ExportToTemporaryFileAsync_UsesSafeFileNameAndAuditsExplicitExport()
    {
        var document = new CareDocument
        {
            Id = "document-1",
            ProfileId = "profile-1",
            Title = "Report",
            OriginalFileName = $"..{Path.DirectorySeparatorChar}outside.txt",
            EncryptedFileName = "document-1.cndoc"
        };
        var repository = new RecordingRepository { ExistingDocument = document };
        var store = new DocumentStoreSpy { ExportPayload = Encoding.UTF8.GetBytes("decrypted") };
        var service = new DocumentService(repository, store, new FixedTimeProvider(Now));
        var temporaryDirectory = Path.Combine(Path.GetTempPath(), "CareNest.UnitTests", Guid.NewGuid().ToString("N"));

        try
        {
            var outputPath = await service.ExportToTemporaryFileAsync(document.Id, temporaryDirectory);

            var normalizedDirectory = Path.GetFullPath(temporaryDirectory) + Path.DirectorySeparatorChar;
            Assert.StartsWith(normalizedDirectory, Path.GetFullPath(outputPath), StringComparison.Ordinal);
            Assert.EndsWith("_outside.txt", outputPath, StringComparison.Ordinal);
            Assert.Equal("decrypted", await File.ReadAllTextAsync(outputPath));
            var audit = Assert.Single(repository.AuditEntries);
            Assert.Equal(AuditAction.Exported, audit.Action);
            Assert.Equal(document.Id, audit.EntityId);
        }
        finally
        {
            if (Directory.Exists(temporaryDirectory))
            {
                Directory.Delete(temporaryDirectory, recursive: true);
            }
        }
    }

    [Fact]
    public async Task DeleteAsync_ExistingDocument_RemovesRecordAndEncryptedPayload()
    {
        var document = new CareDocument
        {
            Id = "document-1",
            ProfileId = "profile-1",
            Title = "Report",
            OriginalFileName = "report.pdf",
            EncryptedFileName = "document-1.cndoc"
        };
        var repository = new RecordingRepository { ExistingDocument = document };
        var store = new DocumentStoreSpy();
        var service = new DocumentService(repository, store, new FixedTimeProvider(Now));

        await service.DeleteAsync(document.Id);

        Assert.Contains(document.Id, repository.DeletedDocumentIds);
        Assert.Contains(document.EncryptedFileName, store.DeletedFiles);
    }

    [Fact]
    public async Task DeleteAsync_MissingDocument_IsIdempotent()
    {
        var repository = new RecordingRepository();
        var store = new DocumentStoreSpy();
        var service = new DocumentService(repository, store, new FixedTimeProvider(Now));

        await service.DeleteAsync("missing");

        Assert.Empty(repository.DeletedDocumentIds);
        Assert.Empty(store.DeletedFiles);
    }

    private static PickedFile Picked(string fileName, string? contentType, string value) =>
        new(fileName, contentType, _ => Task.FromResult<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(value), writable: false)));

    private sealed class RecordingRepository : RepositoryStub
    {
        public bool ThrowOnSave { get; init; }

        public bool ThrowOnAudit { get; init; }

        public CareDocument? ExistingDocument { get; init; }

        public CareDocument? SavedDocument { get; private set; }

        public List<string> DeletedDocumentIds { get; } = [];

        public List<AuditEntry> AuditEntries { get; } = [];

        public override Task<CareDocument?> GetDocumentAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ExistingDocument?.Id == id ? ExistingDocument : null);
        }

        public override Task SaveDocumentAsync(CareDocument document, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (ThrowOnSave)
            {
                throw new InvalidOperationException("save failed");
            }

            SavedDocument = document;
            return Task.CompletedTask;
        }

        public override Task DeleteDocumentAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DeletedDocumentIds.Add(id);
            return Task.CompletedTask;
        }

        public override Task AddAuditEntryAsync(AuditEntry entry, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (ThrowOnAudit)
            {
                throw new InvalidOperationException("audit failed");
            }

            AuditEntries.Add(entry);
            return Task.CompletedTask;
        }
    }
}
