namespace CareNest.UiTests;

public sealed class ProfilePhotoLifecycleContractTests
{
    [Fact]
    public void ProfileEditor_TracksPersistedAndStagedPhotoReferencesSeparately()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "ProfileEditorViewModel.cs");

        Assert.Contains("_persistedPhotoEncryptedFileName", source, StringComparison.Ordinal);
        Assert.Contains("_photoEncryptedFileName", source, StringComparison.Ordinal);
        Assert.Contains("_persistedPhotoEncryptedFileName = profile.PhotoPath", source, StringComparison.Ordinal);
        Assert.Contains("_photoEncryptedFileName = profile.PhotoPath", source, StringComparison.Ordinal);
        Assert.Contains("string.Equals(\n                _photoEncryptedFileName,\n                _persistedPhotoEncryptedFileName", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ProfileEditor_UsesSharedAppLifetimePhotoGateInsteadOfDisposableInstanceGate()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "ProfileEditorViewModel.cs");

        Assert.Contains("private static readonly SemaphoreSlim PhotoGate = new(1, 1)", source, StringComparison.Ordinal);
        Assert.DoesNotContain("private readonly SemaphoreSlim _photoGate", source, StringComparison.Ordinal);
        Assert.Contains("await PhotoGate.WaitAsync", source, StringComparison.Ordinal);
        Assert.Contains("PhotoGate.Release()", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ProfileEditor_DeletesPersistedPhotoOnlyAfterProfileSaveSucceeds()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "ProfileEditorViewModel.cs");
        var saveMethod = source[source.IndexOf("private Task SaveAsync() =>", StringComparison.Ordinal)..];

        var saveIndex = saveMethod.IndexOf("await _profiles.SaveAsync(profile, ct)", StringComparison.Ordinal);
        var obsoleteIndex = saveMethod.IndexOf("_pendingObsoletePhotoEncryptedFileName = obsoletePhoto", StringComparison.Ordinal);
        var cleanupIndex = saveMethod.IndexOf("await TryDeletePendingObsoletePhotoAsync()", StringComparison.Ordinal);

        Assert.True(saveIndex >= 0);
        Assert.True(obsoleteIndex > saveIndex);
        Assert.True(cleanupIndex > obsoleteIndex);
    }

    [Fact]
    public void ProfileEditor_PreviewsUsePartialFileThenAtomicMove()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "ProfileEditorViewModel.cs");

        Assert.Contains(".partial", source, StringComparison.Ordinal);
        Assert.Contains("File.Move(partialPath, path, overwrite: true)", source, StringComparison.Ordinal);
        Assert.Contains("if (File.Exists(partialPath))", source, StringComparison.Ordinal);
        Assert.Contains("File.Delete(partialPath)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ProfileEditorPage_CleansPendingPhotoWhenPageDisappears()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Views", "ProfileEditorPage.xaml.cs");

        Assert.Contains("protected override async void OnDisappearing()", source, StringComparison.Ordinal);
        Assert.Contains("await _viewModel.DiscardPendingPhotoAsync()", source, StringComparison.Ordinal);
        Assert.Contains("catch", source, StringComparison.Ordinal);
    }
}
