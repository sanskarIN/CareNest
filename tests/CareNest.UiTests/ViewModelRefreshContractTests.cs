namespace CareNest.UiTests;

public sealed class ViewModelRefreshContractTests
{
    [Fact]
    public void MedicationLogMutations_RefreshThroughNonReentrantCore()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "MedicationLogViewModel.cs");

        Assert.Contains("public Task LoadAsync() =>", source, StringComparison.Ordinal);
        Assert.Contains("LoadCoreAsync", source, StringComparison.Ordinal);
        Assert.Contains("await LoadCoreAsync(ct)", source, StringComparison.Ordinal);

        var editRegion = Slice(
            source,
            "public Task EditEntryAsync(",
            "private Task ChangeReminderAsync(");
        var reminderRegion = Slice(
            source,
            "private Task ChangeReminderAsync(",
            "private Task SnoozeAsync(");
        var snoozeRegion = source[source.IndexOf(
            "private Task SnoozeAsync(",
            StringComparison.Ordinal)..];

        Assert.DoesNotContain("await LoadAsync()", editRegion, StringComparison.Ordinal);
        Assert.DoesNotContain("await LoadAsync()", reminderRegion, StringComparison.Ordinal);
        Assert.DoesNotContain("await LoadAsync()", snoozeRegion, StringComparison.Ordinal);
    }

    [Fact]
    public void DocumentMutations_RefreshThroughNonReentrantCore()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "DocumentsViewModel.cs");

        Assert.Contains("private async Task LoadCoreAsync(CancellationToken ct)", source, StringComparison.Ordinal);

        var deleteRegion = Slice(
            source,
            "public Task DeleteAsync(",
            "public Task SetTagsAsync(");
        var tagsRegion = Slice(
            source,
            "public Task SetTagsAsync(",
            "private async Task LoadCoreAsync(");
        var importRegion = Slice(
            source,
            "private async Task ImportPickedFileAsync(",
            "private Task ExportAsync(");

        Assert.Contains("await LoadCoreAsync(ct)", deleteRegion, StringComparison.Ordinal);
        Assert.Contains("await LoadCoreAsync(ct)", tagsRegion, StringComparison.Ordinal);
        Assert.Contains("await LoadCoreAsync(cancellationToken)", importRegion, StringComparison.Ordinal);
        Assert.DoesNotContain("await LoadAsync()", deleteRegion, StringComparison.Ordinal);
        Assert.DoesNotContain("await LoadAsync()", tagsRegion, StringComparison.Ordinal);
        Assert.DoesNotContain("await LoadAsync()", importRegion, StringComparison.Ordinal);
    }

    private static string Slice(string source, string startToken, string endToken)
    {
        var start = source.IndexOf(startToken, StringComparison.Ordinal);
        var end = source.IndexOf(endToken, start, StringComparison.Ordinal);
        Assert.True(start >= 0);
        Assert.True(end > start);
        return source[start..end];
    }
}
