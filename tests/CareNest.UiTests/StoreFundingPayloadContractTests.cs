namespace CareNest.UiTests;

public sealed class StoreFundingPayloadContractTests
{
    [Fact]
    public void AppRuntime_DoesNotContainExternalFundingOrStorefrontDestinationOrSurface()
    {
        var appRoot = RepositoryLocator.PathOf("src", "CareNest.App");
        var textExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".cs", ".csproj", ".xaml", ".xml", ".plist", ".resx", ".svg", ".txt", ".json"
        };

        foreach (var path in Directory.EnumerateFiles(appRoot, "*", SearchOption.AllDirectories)
                     .Where(path => textExtensions.Contains(Path.GetExtension(path))))
        {
            var source = File.ReadAllText(path);
            Assert.DoesNotContain("buymeacoffee.com/sanskarIN", source, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("ramsandesh.gumroad.com", source, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("SupportProjectCommand", source, StringComparison.Ordinal);
            Assert.DoesNotContain("IsProjectSupportVisible", source, StringComparison.Ordinal);
            Assert.DoesNotContain("FundingLinkPolicy", source, StringComparison.Ordinal);
        }

        Assert.False(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "ViewModels", "FundingLinkPolicy.Enabled.cs")));
        Assert.False(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "ViewModels", "FundingLinkPolicy.Disabled.cs")));
        Assert.False(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "Images", "buy_me_a_coffee_carenest.svg")));
        Assert.False(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "Images", "carenest_support.svg")));
        Assert.False(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "Images", "gumroad_store_badge.svg")));
    }

    [Fact]
    public void SharedAssembly_DoesNotCarryExternalCommerceUrlConstants()
    {
        var sharedConstants = RepositoryLocator.Read("src", "CareNest.Shared", "AppConstants.cs");

        Assert.DoesNotContain("FundingUrl", sharedConstants, StringComparison.Ordinal);
        Assert.DoesNotContain("Gumroad", sharedConstants, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("buymeacoffee.com", sharedConstants, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("gumroad.com", sharedConstants, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MauiProject_DoesNotContainObsoleteExternalCommerceBuildConfiguration()
    {
        var project = RepositoryLocator.Read("src", "CareNest.App", "CareNest.App.csproj");

        Assert.DoesNotContain("CareNestShowFundingLink", project, StringComparison.Ordinal);
        Assert.DoesNotContain("CareNestEffectiveFundingLink", project, StringComparison.Ordinal);
        Assert.DoesNotContain("CARENEST_STORE_FUNDING_LINK", project, StringComparison.Ordinal);
        Assert.DoesNotContain("CARENEST_FUNDING_LINK", project, StringComparison.Ordinal);
        Assert.DoesNotContain("CARENEST_GUMROAD", project, StringComparison.Ordinal);
        Assert.DoesNotContain("FundingLinkPolicy", project, StringComparison.Ordinal);
        Assert.DoesNotContain("DefineConstants", project, StringComparison.Ordinal);
    }

    [Fact]
    public void PayloadScanner_SearchesRepositoryOnlyFundingAndStorefrontMarkers()
    {
        var scanner = RepositoryLocator.Read("build", "scripts", "verify-store-safe-payload.py");

        Assert.Contains("buymeacoffee.com/sanskarIN", scanner, StringComparison.Ordinal);
        Assert.Contains("ramsandesh.gumroad.com", scanner, StringComparison.Ordinal);
        Assert.Contains("value.encode(\"utf-8\")", scanner, StringComparison.Ordinal);
        Assert.Contains("value.encode(\"utf-16-le\")", scanner, StringComparison.Ordinal);
        Assert.Contains("value.encode(\"utf-16-be\")", scanner, StringComparison.Ordinal);
        Assert.Contains("zipfile.is_zipfile(path)", scanner, StringComparison.Ordinal);
        Assert.Contains("zipfile.ZipFile(path, \"r\")", scanner, StringComparison.Ordinal);
        Assert.Contains("stream_contains(stream, needles)", scanner, StringComparison.Ordinal);
        Assert.Contains("action=\"append\"", scanner, StringComparison.Ordinal);
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
