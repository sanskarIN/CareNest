namespace CareNest.UiTests;

public sealed class ReleasePreflightContractTests
{
    [Theory]
    [InlineData("release-preflight.sh")]
    [InlineData("release-preflight.ps1")]
    public void PreflightScripts_RequireUnsuppressedNuGetAudit(string scriptName)
    {
        var script = RepositoryLocator.Read("build", "scripts", scriptName);

        Assert.Contains("NuGetAudit=true", script, StringComparison.Ordinal);
        Assert.Contains("NuGetAuditMode=all", script, StringComparison.Ordinal);
        Assert.DoesNotContain("known SQLitePCLRaw advisory remains", script, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BashPreflight_DoesNotIgnoreDependencyAuditFailure()
    {
        var script = RepositoryLocator.Read("build", "scripts", "release-preflight.sh");

        Assert.DoesNotContain("--vulnerable --include-transitive || true", script, StringComparison.Ordinal);
        Assert.Contains("Blocking dependency audit", script, StringComparison.Ordinal);
    }

    [Fact]
    public void PowerShellPreflight_ThrowsOnDependencyAuditFailure()
    {
        var script = RepositoryLocator.Read("build", "scripts", "release-preflight.ps1");

        Assert.Contains("throw \"Dependency audit failed: $project\"", script, StringComparison.Ordinal);
        Assert.Contains("throw \"MAUI dependency audit failed: $($env:CARENEST_TARGET)\"", script, StringComparison.Ordinal);
    }
}
