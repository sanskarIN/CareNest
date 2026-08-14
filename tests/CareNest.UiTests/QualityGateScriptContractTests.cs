namespace CareNest.UiTests;

public sealed class QualityGateScriptContractTests
{
    [Theory]
    [InlineData("quality-gate.sh")]
    [InlineData("quality-gate.ps1")]
    public void QualityGate_RestoresTestsAndRunsBlockingAudit(string scriptName)
    {
        var script = RepositoryLocator.Read("build", "scripts", scriptName);

        Assert.DoesNotContain("--no-restore", script, StringComparison.Ordinal);
        Assert.Contains("NuGetAudit=true", script, StringComparison.Ordinal);
        Assert.Contains("NuGetAuditMode=all", script, StringComparison.Ordinal);
        Assert.Contains("CareNest.UnitTests.csproj", script, StringComparison.Ordinal);
        Assert.Contains("CareNest.IntegrationTests.csproj", script, StringComparison.Ordinal);
        Assert.Contains("CareNest.UiTests.csproj", script, StringComparison.Ordinal);
    }

    [Fact]
    public void BashQualityGate_FailsOnNativeCommandErrors()
    {
        var script = RepositoryLocator.Read("build", "scripts", "quality-gate.sh");

        Assert.Contains("set -euo pipefail", script, StringComparison.Ordinal);
    }

    [Fact]
    public void PowerShellQualityGate_ChecksEveryDotnetExitCode()
    {
        var script = RepositoryLocator.Read("build", "scripts", "quality-gate.ps1");

        Assert.Contains("function Invoke-Dotnet", script, StringComparison.Ordinal);
        Assert.Contains("$LASTEXITCODE -ne 0", script, StringComparison.Ordinal);
        Assert.DoesNotContain("dotnet test ", script, StringComparison.Ordinal);
        Assert.DoesNotContain("dotnet build ", script, StringComparison.Ordinal);
    }
}
