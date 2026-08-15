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
    public void FundingPolicy_PhysicallySeparatesEnabledAndStoreSafeCompileUnits()
    {
        var viewModel = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "AboutViewModel.cs");
        var enabled = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "FundingLinkPolicy.Enabled.cs");
        var disabled = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "FundingLinkPolicy.Disabled.cs");

        Assert.DoesNotContain("buymeacoffee.com", viewModel, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("CARENEST_FUNDING_LINK", viewModel, StringComparison.Ordinal);
        Assert.Contains("SupportProjectCommand = FundingLinkPolicy.CreateCommand(OpenAsync);", viewModel, StringComparison.Ordinal);
        Assert.Contains("public bool IsProjectSupportVisible => FundingLinkPolicy.IsVisible;", viewModel, StringComparison.Ordinal);

        Assert.Contains("private const string FundingUrl = \"https://buymeacoffee.com/sanskarIN\";", enabled, StringComparison.Ordinal);
        Assert.Contains("public static bool IsVisible => true;", enabled, StringComparison.Ordinal);
        Assert.Contains("new AsyncCommand(() => openAsync(FundingUrl))", enabled, StringComparison.Ordinal);

        Assert.DoesNotContain("buymeacoffee.com", disabled, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("public static bool IsVisible => false;", disabled, StringComparison.Ordinal);
        Assert.Contains("new AsyncCommand(() => Task.CompletedTask, static () => false)", disabled, StringComparison.Ordinal);
    }

    [Fact]
    public void Project_PhysicallySelectsOneFundingPolicyAndFailsClosed()
    {
        var project = RepositoryLocator.Read("src", "CareNest.App", "CareNest.App.csproj");

        Assert.Contains(
            "<CareNestShowFundingLink Condition=\"'$(CareNestShowFundingLink)' == '' and '$(CARENEST_STORE_FUNDING_LINK)' != ''\">$(CARENEST_STORE_FUNDING_LINK)</CareNestShowFundingLink>",
            project,
            StringComparison.Ordinal);
        Assert.Contains(
            "<CareNestShowFundingLink Condition=\"'$(CareNestShowFundingLink)' == ''\">true</CareNestShowFundingLink>",
            project,
            StringComparison.Ordinal);
        Assert.Contains("ValidateCareNestFundingLinkConfiguration", project, StringComparison.Ordinal);
        Assert.Contains("BeforeTargets=\"CoreCompile\"", project, StringComparison.Ordinal);
        Assert.Contains("'$(CareNestShowFundingLink)' != 'true' and '$(CareNestShowFundingLink)' != 'false'", project, StringComparison.Ordinal);
        Assert.Contains("CareNestShowFundingLink must be exactly 'true' or 'false'.", project, StringComparison.Ordinal);
        Assert.Contains("<Compile Remove=\"ViewModels\\FundingLinkPolicy.Enabled.cs\" />", project, StringComparison.Ordinal);
        Assert.Contains("<Compile Remove=\"ViewModels\\FundingLinkPolicy.Disabled.cs\" />", project, StringComparison.Ordinal);
        Assert.Contains("<Compile Include=\"ViewModels\\FundingLinkPolicy.Enabled.cs\" Condition=\"'$(CareNestShowFundingLink)' == 'true'\" />", project, StringComparison.Ordinal);
        Assert.Contains("<Compile Include=\"ViewModels\\FundingLinkPolicy.Disabled.cs\" Condition=\"'$(CareNestShowFundingLink)' == 'false'\" />", project, StringComparison.Ordinal);
        Assert.DoesNotContain("CARENEST_FUNDING_LINK", project, StringComparison.Ordinal);
        Assert.DoesNotContain("DefineConstants", project, StringComparison.Ordinal);
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
