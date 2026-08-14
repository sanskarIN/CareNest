namespace CareNest.UiTests;

public sealed class BackupCompletionSemanticsContractTests
{
    [Fact]
    public void BackupCreation_RecordsMetadataOnlyAfterEncryptedPayloadCompletes()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Backup",
            "EncryptedBackupService.cs");
        var createStart = source.IndexOf(
            "public async Task CreateEncryptedBackupAsync(",
            StringComparison.Ordinal);
        var inspectStart = source.IndexOf(
            "public async Task<BackupInspection> InspectAsync(",
            createStart,
            StringComparison.Ordinal);
        Assert.True(createStart >= 0);
        Assert.True(inspectStart > createStart);
        var create = source[createStart..inspectStart];

        var encryptIndex = create.IndexOf("ChunkedAead.EncryptAsync", StringComparison.Ordinal);
        var metadataIndex = create.IndexOf("TryRecordBackupMetadataAsync", StringComparison.Ordinal);
        Assert.True(encryptIndex >= 0);
        Assert.True(metadataIndex > encryptIndex);
    }

    [Fact]
    public void CompletedBackupBookkeeping_UsesNonCancellingBestEffortWrites()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Backup",
            "EncryptedBackupService.cs");

        Assert.Contains("TryRecordBackupMetadataAsync", source, StringComparison.Ordinal);
        Assert.Contains("CreateBackupMetadataAsync(metadata, CancellationToken.None)", source, StringComparison.Ordinal);
        Assert.Contains("TryRecordRestoreAuditAsync", source, StringComparison.Ordinal);
        Assert.Contains("CancellationToken.None", source, StringComparison.Ordinal);
        Assert.Contains("catch (Exception ex)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void BackupBookkeepingLogging_DoesNotPassFullExceptionObject()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Backup",
            "EncryptedBackupService.cs");
        var loggingStart = source.IndexOf(
            "private void LogBookkeepingFailure(",
            StringComparison.Ordinal);
        Assert.True(loggingStart >= 0);
        var logging = source[loggingStart..];

        Assert.Contains("logger.IsEnabled(LogLevel.Warning)", logging, StringComparison.Ordinal);
        Assert.Contains("exception.GetType().FullName", logging, StringComparison.Ordinal);
        Assert.DoesNotContain("logger.LogWarning(exception", logging, StringComparison.Ordinal);
        Assert.DoesNotContain("logger.LogWarning(ex", logging, StringComparison.Ordinal);
        Assert.DoesNotContain("exception.Message", logging, StringComparison.Ordinal);
        Assert.DoesNotContain("exception.StackTrace", logging, StringComparison.Ordinal);
    }

    [Fact]
    public void RestoreAudit_IsRecordedAfterDatabaseReplacementCompletes()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Backup",
            "EncryptedBackupService.cs");
        var restoreStart = source.IndexOf(
            "public async Task RestoreEncryptedBackupAsync(",
            StringComparison.Ordinal);
        var decryptStart = source.IndexOf(
            "private static async Task DecryptArchiveAsync(",
            restoreStart,
            StringComparison.Ordinal);
        Assert.True(restoreStart >= 0);
        Assert.True(decryptStart > restoreStart);
        var restore = source[restoreStart..decryptStart];

        var replaceIndex = restore.IndexOf("database.ReplaceDatabaseAsync", StringComparison.Ordinal);
        var auditIndex = restore.IndexOf("TryRecordRestoreAuditAsync", StringComparison.Ordinal);
        Assert.True(replaceIndex >= 0);
        Assert.True(auditIndex > replaceIndex);
    }
}
