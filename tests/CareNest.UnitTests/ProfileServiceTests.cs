using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.UnitTests.TestDoubles;

namespace CareNest.UnitTests;

public sealed class ProfileServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 9, 30, 0, TimeSpan.Zero);

    [Fact]
    public async Task SaveAsync_NewProfile_PersistsCreatedAuditAndUtcTouch()
    {
        var repository = new RecordingRepository();
        var service = new ProfileService(repository, new DocumentStoreSpy(), new FixedTimeProvider(Now));
        var profile = new PersonProfile { Id = "profile-1", Name = "Alex" };

        await service.SaveAsync(profile);

        Assert.Same(profile, repository.SavedProfile);
        Assert.Equal(Now.UtcDateTime, profile.UpdatedUtc);
        var audit = Assert.Single(repository.AuditEntries);
        Assert.Equal(AuditAction.Created, audit.Action);
        Assert.Equal(profile.Id, audit.EntityId);
        Assert.Equal("Profile created", audit.SafeSummary);
    }

    [Fact]
    public async Task SaveAsync_ExistingProfile_PersistsUpdatedAudit()
    {
        var profile = new PersonProfile { Id = "profile-1", Name = "Alex" };
        var repository = new RecordingRepository { ExistingProfile = profile };
        var service = new ProfileService(repository, new DocumentStoreSpy(), new FixedTimeProvider(Now));

        await service.SaveAsync(profile);

        var audit = Assert.Single(repository.AuditEntries);
        Assert.Equal(AuditAction.Updated, audit.Action);
        Assert.Equal("profile", audit.ChangedFieldsCsv);
        Assert.Equal("Profile updated", audit.SafeSummary);
    }

    [Fact]
    public async Task DeleteAsync_RemovesProfileDocumentsAndPhotoThenAuditsDeletion()
    {
        var profile = new PersonProfile
        {
            Id = "profile-1",
            Name = "Alex",
            PhotoPath = "profile-photo.cndoc"
        };
        var repository = new RecordingRepository
        {
            ExistingProfile = profile,
            Documents =
            [
                new CareDocument { Id = "doc-1", ProfileId = profile.Id, EncryptedFileName = "doc-1.cndoc" },
                new CareDocument { Id = "doc-2", ProfileId = profile.Id, EncryptedFileName = "doc-2.cndoc" }
            ]
        };
        var documentStore = new DocumentStoreSpy();
        var service = new ProfileService(repository, documentStore, new FixedTimeProvider(Now));

        await service.DeleteAsync(profile.Id);

        Assert.Equal(profile.Id, repository.DeletedProfileId);
        Assert.Equal(
            new[] { "doc-1.cndoc", "doc-2.cndoc", "profile-photo.cndoc" },
            documentStore.DeletedFiles);
        var audit = Assert.Single(repository.AuditEntries);
        Assert.Equal(AuditAction.Deleted, audit.Action);
        Assert.Equal(profile.Id, audit.EntityId);
    }

    private sealed class RecordingRepository : RepositoryStub
    {
        public PersonProfile? ExistingProfile { get; init; }

        public PersonProfile? SavedProfile { get; private set; }

        public string? DeletedProfileId { get; private set; }

        public IReadOnlyList<CareDocument> Documents { get; init; } = Array.Empty<CareDocument>();

        public List<AuditEntry> AuditEntries { get; } = [];

        public override Task<PersonProfile?> GetProfileAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ExistingProfile?.Id == id ? ExistingProfile : null);
        }

        public override Task SaveProfileAsync(PersonProfile profile, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SavedProfile = profile;
            return Task.CompletedTask;
        }

        public override Task<IReadOnlyList<CareDocument>> GetDocumentsAsync(string? profileId = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(Documents);
        }

        public override Task DeleteProfileCascadeAsync(string profileId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DeletedProfileId = profileId;
            return Task.CompletedTask;
        }

        public override Task AddAuditEntryAsync(AuditEntry entry, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            AuditEntries.Add(entry);
            return Task.CompletedTask;
        }
    }
}
