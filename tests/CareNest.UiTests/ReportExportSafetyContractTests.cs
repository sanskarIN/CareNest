namespace CareNest.UiTests;

public sealed class ReportExportSafetyContractTests
{
    [Theory]
    [InlineData("CsvWriter.cs")]
    [InlineData("SimplePdfWriter.cs")]
    public void PlaintextReportWriters_UsePartialFileThenAtomicMove(string fileName)
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Reports",
            fileName);

        Assert.Contains(".partial", source, StringComparison.Ordinal);
        Assert.Contains("File.Move(partialPath, path, overwrite: true)", source, StringComparison.Ordinal);
        Assert.Contains("finally", source, StringComparison.Ordinal);
        Assert.Contains("TryDeleteFile(partialPath)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void JsonProfileExport_UsesPartialFileThenAtomicMove()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Reports",
            "ReportService.cs");

        Assert.Contains("WriteJsonAtomicallyAsync", source, StringComparison.Ordinal);
        Assert.Contains(".partial", source, StringComparison.Ordinal);
        Assert.Contains("File.Move(partialPath, outputPath, overwrite: true)", source, StringComparison.Ordinal);
        Assert.Contains("TryDeleteFile(partialPath)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ReportsViewModel_ReselectsProfileFromFreshRows()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "ReportsViewModel.cs");

        Assert.Contains("var selectedId = SelectedProfile?.Id", source, StringComparison.Ordinal);
        Assert.Contains("Profiles.FirstOrDefault(x => x.Id == selectedId)", source, StringComparison.Ordinal);
        Assert.Contains("Profiles.FirstOrDefault(x => x.IsPrimary)", source, StringComparison.Ordinal);
        Assert.DoesNotContain("SelectedProfile ??=", source, StringComparison.Ordinal);
    }
}
