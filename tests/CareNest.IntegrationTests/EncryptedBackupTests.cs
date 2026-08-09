using System.Text;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Shared;

namespace CareNest.IntegrationTests;

public sealed class EncryptedBackupTests
{
    [Fact]
    public async Task Backup_RestoresDatabaseDocumentsAndDocumentKey()
    {
        await using var store = await TestStore.CreateAsync();

        var profile = new PersonProfile { Name = "Backup profile", IsPrimary = true };
        await store.Repository.SaveProfileAsync(profile);

        await using var source = new MemoryStream(Encoding.UTF8.GetBytes("portable encrypted document"));
        var stored = await store.Documents.ImportAsync(source, "report.txt", "text/plain");
        var document = new CareDocument
        {
            ProfileId = profile.Id,
            Title = "Report",
            Category = DocumentCategory.LabReport,
            EncryptedFileName = stored.EncryptedFileName,
            OriginalFileName = "report.txt",
            ContentType = "text/plain",
            OriginalSizeBytes = stored.OriginalSizeBytes,
            Sha256 = stored.Sha256,
            EncryptionVersion = stored.EncryptionVersion
        };
        await store.Repository.SaveDocumentAsync(document);

        await using var backup = new MemoryStream();
        await store.Backups.CreateEncryptedBackupAsync(backup, "correct horse battery", "test");

        await store.Repository.ClearAllAsync();
        foreach (var file in await store.Documents.ListStoredFilesAsync())
        {
            await store.Documents.DeleteAsync(file);
        }
        await store.Secrets.RemoveAsync(SecretKeys.DocumentMasterKey);

        backup.Position = 0;
        await store.Backups.RestoreEncryptedBackupAsync(backup, "correct horse battery");

        var restoredProfile = await store.Repository.GetProfileAsync(profile.Id);
        Assert.NotNull(restoredProfile);
        var restoredDoc = await store.Repository.GetDocumentAsync(document.Id);
        Assert.NotNull(restoredDoc);

        await using var output = new MemoryStream();
        await store.Documents.ExportDecryptedAsync(restoredDoc!.EncryptedFileName, output);
        Assert.Equal("portable encrypted document", Encoding.UTF8.GetString(output.ToArray()));
    }

    [Fact]
    public async Task WrongPassword_DoesNotInspectBackup()
    {
        await using var store = await TestStore.CreateAsync();
        await using var backup = new MemoryStream();
        await store.Backups.CreateEncryptedBackupAsync(backup, "correct password", "test");
        backup.Position = 0;

        await Assert.ThrowsAsync<InvalidDataException>(() =>
            store.Backups.InspectAsync(backup, "wrong password"));
    }

    [Fact]
    public async Task ModifiedBackup_FailsAuthenticatedDecryption()
    {
        await using var store = await TestStore.CreateAsync();
        await using var backup = new MemoryStream();
        await store.Backups.CreateEncryptedBackupAsync(backup, "correct password", "test");
        var bytes = backup.ToArray();
        bytes[^1] ^= 0x20;

        await using var modified = new MemoryStream(bytes);
        await Assert.ThrowsAsync<InvalidDataException>(() =>
            store.Backups.InspectAsync(modified, "correct password"));
    }
}
