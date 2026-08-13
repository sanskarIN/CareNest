namespace CareNest.UiTests;

public sealed class SettingsLifecycleContractTests
{
    [Fact]
    public void Settings_UsesRegisteredSecretStoreForDocumentKeyLifecycle()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "SettingsViewModel.cs");
        var startup = RepositoryLocator.Read("src", "CareNest.App", "MauiProgram.cs");

        Assert.Contains("private readonly ISecretStore _secretStore;", source, StringComparison.Ordinal);
        Assert.Contains("ISecretStore secretStore,", source, StringComparison.Ordinal);
        Assert.Contains("_secretStore = secretStore;", source, StringComparison.Ordinal);
        Assert.Contains("SecretKeys.DocumentMasterKey", source, StringComparison.Ordinal);
        Assert.Contains("AddSingleton<ISecretStore, SecureSecretStore>()", startup, StringComparison.Ordinal);
    }

    [Fact]
    public void Settings_LocalClearLifecycle_KeepsStorageAndSecretOperationsOrdered()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "SettingsViewModel.cs");
        var start = source.IndexOf("public Task " + "ResetAllDataAsync() =>", StringComparison.Ordinal);
        Assert.True(start >= 0);

        var end = source.IndexOf("private Task SavePreferencesAsync() =>", start, StringComparison.Ordinal);
        Assert.True(end > start);
        var method = source[start..end];

        var notifications = method.IndexOf("_notifications.CancelAllAsync", StringComparison.Ordinal);
        var files = method.IndexOf("_documents.ListStoredFilesAsync", StringComparison.Ordinal);
        var records = method.IndexOf("_repository.ClearAllAsync", StringComparison.Ordinal);
        var payloads = method.IndexOf("_documents." + "DeleteAsync", StringComparison.Ordinal);
        var documentKey = method.IndexOf("_secretStore.RemoveAsync", StringComparison.Ordinal);
        var appLock = method.IndexOf("_lock.DisableAsync", StringComparison.Ordinal);
        var navigation = method.IndexOf("_navigator.ResetToOnboardingAsync", StringComparison.Ordinal);

        Assert.True(notifications >= 0);
        Assert.True(files > notifications);
        Assert.True(records > files);
        Assert.True(payloads > records);
        Assert.True(documentKey > payloads);
        Assert.True(appLock > documentKey);
        Assert.True(navigation > appLock);
    }
}
