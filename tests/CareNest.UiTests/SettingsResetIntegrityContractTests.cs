namespace CareNest.UiTests;

public sealed class SettingsResetIntegrityContractTests
{
    [Fact]
    public void ResetAllData_ClearsStructuredRecordsBeforeEncryptedPayloads()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "SettingsViewModel.cs");

        var methodStart = source.IndexOf(
            "private async Task ResetAllDataAsync()",
            StringComparison.Ordinal);
        Assert.True(methodStart >= 0, "ResetAllDataAsync must remain present.");

        var methodSource = source[methodStart..];
        var listFiles = methodSource.IndexOf(
            "var storedFiles = await documentStore.ListStoredFilesAsync();",
            StringComparison.Ordinal);
        var clearDatabase = methodSource.IndexOf(
            "await repository.ClearAllAsync();",
            StringComparison.Ordinal);
        var disableLock = methodSource.IndexOf(
            "await appLock.DisableAsync();",
            StringComparison.Ordinal);
        var deleteFile = methodSource.IndexOf(
            "await documentStore.DeleteAsync(file);",
            StringComparison.Ordinal);
        var removeDocumentKey = methodSource.IndexOf(
            "await secretStore.RemoveAsync(SecretKeys.DocumentMasterKey);",
            StringComparison.Ordinal);
        var navigate = methodSource.IndexOf(
            "await navigation.NavigateAsync(\"//onboarding\");",
            StringComparison.Ordinal);

        Assert.True(listFiles >= 0, "Reset must capture encrypted filenames before structured data is cleared.");
        Assert.True(clearDatabase > listFiles, "Structured records must be cleared after filenames are captured.");
        Assert.True(disableLock > clearDatabase, "App-lock state must be cleared after the database reset succeeds.");
        Assert.True(deleteFile > disableLock, "Encrypted payload deletion must happen only after structured records and app-lock state are cleared.");
        Assert.True(removeDocumentKey > deleteFile, "The document master key must remain available until encrypted payload cleanup succeeds.");
        Assert.True(navigate > removeDocumentKey, "Reset must navigate to onboarding only after encrypted payload and document-key cleanup completes.");
    }

    [Fact]
    public void ResetAllData_CancelsNotificationsBeforeDestructiveStorageChanges()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "SettingsViewModel.cs");

        var methodStart = source.IndexOf(
            "private async Task ResetAllDataAsync()",
            StringComparison.Ordinal);
        Assert.True(methodStart >= 0, "ResetAllDataAsync must remain present.");

        var methodSource = source[methodStart..];
        var cancelNotifications = methodSource.IndexOf(
            "await notifications.CancelAllAsync();",
            StringComparison.Ordinal);
        var clearDatabase = methodSource.IndexOf(
            "await repository.ClearAllAsync();",
            StringComparison.Ordinal);

        Assert.True(cancelNotifications >= 0, "Reset must cancel CareNest notifications.");
        Assert.True(clearDatabase > cancelNotifications, "Notification registrations should be cancelled before destructive local-data reset begins.");
    }

    [Fact]
    public void ResetAllData_RemovesDocumentVaultSecretThroughSecretStore()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "SettingsViewModel.cs");

        Assert.Contains("private readonly ISecretStore secretStore;", source, StringComparison.Ordinal);
        Assert.Contains("ISecretStore secretStore,", source, StringComparison.Ordinal);
        Assert.Contains("this.secretStore = secretStore;", source, StringComparison.Ordinal);
        Assert.Contains(
            "await secretStore.RemoveAsync(SecretKeys.DocumentMasterKey);",
            source,
            StringComparison.Ordinal);
    }
}
