namespace CareNest.UiTests;

public sealed class StoreFundingPayloadContractTests
{
    [Fact]
    public void SharedAssembly_DoesNotCarryFundingUrlConstant()
    {
        var sharedConstants = RepositoryLocator.Read("src", "CareNest.Shared", "AppConstants.cs");

        Assert.DoesNotContain("FundingUrl", sharedConstants, StringComparison.Ordinal);
        Assert.DoesNotContain("buymeacoffee.com", sharedConstants, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AboutViewModel_CompilesFundingUrlOnlyWhenFundingSurfaceIsEnabled()
    {
        var viewModel = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "AboutViewModel.cs");
        const string conditionalFundingBlock =
            "#if CARENEST_FUNDING_LINK\n    private const string FundingUrl = \"https://buymeacoffee.com/sanskarIN\";\n#endif";

        Assert.Contains(conditionalFundingBlock, viewModel, StringComparison.Ordinal);
        Assert.Contains("SupportProjectCommand = new AsyncCommand(() => OpenAsync(FundingUrl));", viewModel, StringComparison.Ordinal);
        Assert.Contains("SupportProjectCommand = new AsyncCommand(() => Task.CompletedTask, static () => false);", viewModel, StringComparison.Ordinal);
        Assert.DoesNotContain("AppConstants.FundingUrl", viewModel, StringComparison.Ordinal);
    }

    [Fact]
    public void PayloadScanner_SearchesCommonManagedStringEncodingsAndZipEntries()
    {
        var scanner = RepositoryLocator.Read("build", "scripts", "verify-store-safe-payload.py");

        Assert.Contains("buymeacoffee.com/sanskarIN", scanner, StringComparison.Ordinal);
        Assert.Contains("value.encode(\"utf-8\")", scanner, StringComparison.Ordinal);
        Assert.Contains("value.encode(\"utf-16-le\")", scanner, StringComparison.Ordinal);
        Assert.Contains("value.encode(\"utf-16-be\")", scanner, StringComparison.Ordinal);
        Assert.Contains("zipfile.is_zipfile(path)", scanner, StringComparison.Ordinal);
        Assert.Contains("zipfile.ZipFile(path, \"r\")", scanner, StringComparison.Ordinal);
        Assert.Contains("stream_contains(stream, needles)", scanner, StringComparison.Ordinal);
    }

    [Fact]
    public void PayloadScanner_FailsClosedForMatchesAndInspectionErrors()
    {
        var scanner = RepositoryLocator.Read("build", "scripts", "verify-store-safe-payload.py");

        Assert.Contains("if matches:", scanner, StringComparison.Ordinal);
        Assert.Contains("return 1", scanner, StringComparison.Ordinal);
        Assert.Contains("except RuntimeError as exc:", scanner, StringComparison.Ordinal);
        Assert.Contains("return 2", scanner, StringComparison.Ordinal);
        Assert.Contains("Payload path does not exist or is not a file/directory", scanner, StringComparison.Ordinal);
        Assert.DoesNotContain("except Exception", scanner, StringComparison.Ordinal);
    }
}
