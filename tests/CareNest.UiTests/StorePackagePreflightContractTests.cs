namespace CareNest.UiTests;

public sealed class StorePackagePreflightContractTests
{
    [Theory]
    [InlineData("store-package-preflight.sh")]
    [InlineData("store-package-preflight.ps1")]
    public void StorePackagePreflight_ForcesFundingLinkOffAndDelegatesToReleasePreflight(string scriptName)
    {
        var script = RepositoryLocator.Read("build", "scripts", scriptName);

        Assert.Contains("CARENEST_SHOW_FUNDING_LINK", script, StringComparison.Ordinal);
        Assert.Contains("false", script, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("release-preflight", script, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("store-package-preflight.sh")]
    [InlineData("store-package-preflight.ps1")]
    public void StorePackagePreflight_RequiresAnExplicitSupportedTarget(string scriptName)
    {
        var script = RepositoryLocator.Read("build", "scripts", scriptName);

        Assert.Contains("CARENEST_TARGET", script, StringComparison.Ordinal);
        Assert.Contains("required for store-package preflight", script, StringComparison.Ordinal);
        Assert.Contains("Unsupported CARENEST_TARGET", script, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("net10.0-android")]
    [InlineData("net10.0-ios")]
    [InlineData("net10.0-maccatalyst")]
    [InlineData("net10.0-windows10.0.19041.0")]
    public void StorePackagePreflight_AllowsEverySupportedTarget(string targetFramework)
    {
        var bash = RepositoryLocator.Read("build", "scripts", "store-package-preflight.sh");
        var powershell = RepositoryLocator.Read("build", "scripts", "store-package-preflight.ps1");

        Assert.Contains(targetFramework, bash, StringComparison.Ordinal);
        Assert.Contains(targetFramework, powershell, StringComparison.Ordinal);
    }

    [Fact]
    public void StorePackagePreflight_DoesNotAcceptAUserOverrideThatReenablesFunding()
    {
        var bash = RepositoryLocator.Read("build", "scripts", "store-package-preflight.sh");
        var powershell = RepositoryLocator.Read("build", "scripts", "store-package-preflight.ps1");

        Assert.Contains("export CARENEST_SHOW_FUNDING_LINK=false", bash, StringComparison.Ordinal);
        Assert.Contains("$env:CARENEST_SHOW_FUNDING_LINK = 'false'", powershell, StringComparison.Ordinal);
        Assert.DoesNotContain("CARENEST_SHOW_FUNDING_LINK:-true", bash, StringComparison.Ordinal);
    }
}
