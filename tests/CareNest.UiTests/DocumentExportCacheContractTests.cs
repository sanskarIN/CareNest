namespace CareNest.UiTests;

public sealed class DocumentExportCacheContractTests
{
    [Fact]
    public void DocumentExports_UseManagedExportsDirectory()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "DocumentsViewModel.cs");
        var exportMethod = source[source.IndexOf(
            "private Task ExportAsync(",
            StringComparison.Ordinal)..source.IndexOf(
            "private void ApplyFilter()",
            StringComparison.Ordinal)];

        Assert.Contains("Path.Combine(FileSystem.Current.CacheDirectory, \"Exports\")", exportMethod, StringComparison.Ordinal);
        Assert.Contains("Directory.CreateDirectory(directory)", exportMethod, StringComparison.Ordinal);
        Assert.Contains("ExportToTemporaryFileAsync(\n                row.Id,\n                directory", exportMethod, StringComparison.Ordinal);
        Assert.DoesNotContain("row.Id,\n                FileSystem.Current.CacheDirectory", exportMethod, StringComparison.Ordinal);
    }

    [Fact]
    public void SettingsClearCache_IncludesExportsDirectory()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "SettingsViewModel.cs");

        Assert.Contains("\"Exports\"", source, StringComparison.Ordinal);
    }
}
