using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Domain.Rules;

namespace CareNest.Application.Services;

public sealed class ProfileService(ICareNestRepository repository, IDocumentStore documentStore, TimeProvider timeProvider) : IProfileService
{
    public Task<IReadOnlyList<PersonProfile>> ListAsync(CancellationToken cancellationToken = default) =>
        repository.GetProfilesAsync(false, cancellationToken);

    public Task<PersonProfile?> GetAsync(string id, CancellationToken cancellationToken = default) =>
        repository.GetProfileAsync(id, cancellationToken);

    public async Task SaveAsync(PersonProfile profile, CancellationToken cancellationToken = default)
    {
        ProfileRules.Validate(profile);
        var exists = await repository.GetProfileAsync(profile.Id, cancellationToken) is not null;
        profile.Touch(timeProvider.GetUtcNow().UtcDateTime);
        await repository.SaveProfileAsync(profile, cancellationToken);
        await repository.AddAuditEntryAsync(new AuditEntry
        {
            EntityType = nameof(PersonProfile),
            EntityId = profile.Id,
            Action = exists ? AuditAction.Updated : AuditAction.Created,
            EventUtc = timeProvider.GetUtcNow().UtcDateTime,
            ChangedFieldsCsv = exists ? "profile" : null,
            SafeSummary = exists ? "Profile updated" : "Profile created"
        }, cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var profile = await repository.GetProfileAsync(id, cancellationToken);
        var documents = await repository.GetDocumentsAsync(id, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        await repository.DeleteProfileCascadeAsync(id, cancellationToken);

        var completionFailures = new List<Exception>();
        var encryptedFiles = documents
            .Select(document => document.EncryptedFileName)
            .Append(profile?.PhotoPath)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Cast<string>()
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        foreach (var encryptedFile in encryptedFiles)
        {
            try
            {
                await documentStore.DeleteAsync(encryptedFile, CancellationToken.None);
            }
            catch (Exception cleanupFailure)
            {
                completionFailures.Add(cleanupFailure);
            }
        }

        try
        {
            await repository.AddAuditEntryAsync(new AuditEntry
            {
                EntityType = nameof(PersonProfile),
                EntityId = id,
                Action = AuditAction.Deleted,
                EventUtc = timeProvider.GetUtcNow().UtcDateTime,
                SafeSummary = "Profile and associated local records deleted"
            }, CancellationToken.None);
        }
        catch (Exception auditFailure)
        {
            completionFailures.Add(auditFailure);
        }

        if (completionFailures.Count > 0)
        {
            throw new AggregateException(
                "The profile records were deleted, but one or more local cleanup steps could not be completed.",
                completionFailures);
        }
    }
}
