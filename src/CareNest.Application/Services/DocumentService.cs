using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Shared;

namespace CareNest.Application.Services;

public sealed class DocumentService(
    ICareNestRepository repository,
    IDocumentStore documentStore,
    TimeProvider timeProvider) : IDocumentService
{
    public Task<IReadOnlyList<CareDocument>> ListAsync(string? profileId = null, CancellationToken cancellationToken = default) =>
        repository.GetDocumentsAsync(profileId, cancellationToken);

    public async Task<CareDocument> ImportAsync(
        string profileId,
        string title,
        DocumentCategory category,
        string? notes,
        PickedFile file,
        CancellationToken cancellationToken = default)
    {
        Guard.NotBlank(profileId, nameof(profileId), 64);
        title = Guard.NotBlank(title, nameof(title), 180);
        ArgumentNullException.ThrowIfNull(file);

        await using var source = await file.OpenReadAsync(cancellationToken);
        var stored = await documentStore.ImportAsync(source, file.FileName, file.ContentType, cancellationToken);

        var document = new CareDocument
        {
            ProfileId = profileId,
            Title = title,
            Category = category,
            EncryptedFileName = stored.EncryptedFileName,
            OriginalFileName = file.FileName,
            ContentType = file.ContentType,
            OriginalSizeBytes = stored.OriginalSizeBytes,
            Sha256 = stored.Sha256,
            EncryptionVersion = stored.EncryptionVersion,
            Notes = notes,
            CreatedUtc = timeProvider.GetUtcNow().UtcDateTime,
            UpdatedUtc = timeProvider.GetUtcNow().UtcDateTime
        };

        var recordSaved = false;
        try
        {
            await repository.SaveDocumentAsync(document, cancellationToken);
            recordSaved = true;
            await repository.AddAuditEntryAsync(new AuditEntry
            {
                EntityType = nameof(CareDocument),
                EntityId = document.Id,
                Action = AuditAction.Created,
                EventUtc = timeProvider.GetUtcNow().UtcDateTime,
                SafeSummary = "Encrypted document imported"
            }, cancellationToken);
            return document;
        }
        catch (Exception importFailure)
        {
            var cleanupFailures = new List<Exception>();

            if (recordSaved)
            {
                try
                {
                    await repository.DeleteDocumentAsync(document.Id, CancellationToken.None);
                }
                catch (Exception cleanupFailure)
                {
                    cleanupFailures.Add(cleanupFailure);
                }
            }

            try
            {
                await documentStore.DeleteAsync(stored.EncryptedFileName, CancellationToken.None);
            }
            catch (Exception cleanupFailure)
            {
                cleanupFailures.Add(cleanupFailure);
            }

            if (cleanupFailures.Count > 0)
            {
                throw new AggregateException(
                    "Document import failed and rollback could not fully clean the local record or encrypted payload.",
                    new[] { importFailure }.Concat(cleanupFailures));
            }

            throw;
        }
    }

    public async Task<string> ExportToTemporaryFileAsync(string documentId, string temporaryDirectory, CancellationToken cancellationToken = default)
    {
        var document = await repository.GetDocumentAsync(documentId, cancellationToken)
            ?? throw new FileNotFoundException("Document record was not found.");

        Directory.CreateDirectory(temporaryDirectory);
        var safeName = Path.GetFileName(document.OriginalFileName);
        var outputPath = Path.Combine(temporaryDirectory, $"{Guid.NewGuid():N}_{safeName}");

        try
        {
            await using (var destination = File.Create(outputPath))
            {
                await documentStore.ExportDecryptedAsync(
                    document.EncryptedFileName,
                    destination,
                    cancellationToken);
            }

            await repository.AddAuditEntryAsync(new AuditEntry
            {
                EntityType = nameof(CareDocument),
                EntityId = document.Id,
                Action = AuditAction.Exported,
                EventUtc = timeProvider.GetUtcNow().UtcDateTime,
                SafeSummary = "Document exported by explicit user action"
            }, cancellationToken);

            return outputPath;
        }
        catch (Exception exportFailure)
        {
            try
            {
                if (File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }
            }
            catch (Exception cleanupFailure)
            {
                throw new AggregateException(
                    "Document export failed and its temporary plaintext file could not be fully cleaned up.",
                    exportFailure,
                    cleanupFailure);
            }

            throw;
        }
    }

    public async Task DeleteAsync(string documentId, CancellationToken cancellationToken = default)
    {
        var document = await repository.GetDocumentAsync(documentId, cancellationToken);
        if (document is null)
        {
            return;
        }

        await repository.DeleteDocumentAsync(documentId, cancellationToken);
        await documentStore.DeleteAsync(document.EncryptedFileName, cancellationToken);
    }
}
