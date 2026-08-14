namespace CareNest.UiTests;

public sealed class GitSetupScriptContractTests
{
    [Theory]
    [InlineData("setup-git.sh")]
    [InlineData("setup-git.ps1")]
    public void SetupScripts_ConfigureRequestedRepositoryLocalIdentity(string scriptName)
    {
        var script = RepositoryLocator.Read("build", "scripts", scriptName);

        Assert.Contains("Sanskar", script, StringComparison.Ordinal);
        Assert.Contains("sanskarin@outlook.in", script, StringComparison.Ordinal);
        Assert.Contains("--local", script, StringComparison.Ordinal);
        Assert.Contains("user.name", script, StringComparison.Ordinal);
        Assert.Contains("user.email", script, StringComparison.Ordinal);
    }

    [Fact]
    public void BashSetup_IsFailClosedAndAnchoredToRepositoryRoot()
    {
        var script = RepositoryLocator.Read("build", "scripts", "setup-git.sh");

        Assert.Contains("set -euo pipefail", script, StringComparison.Ordinal);
        Assert.Contains("ROOT_DIR=", script, StringComparison.Ordinal);
        Assert.Contains("git rev-parse --is-inside-work-tree", script, StringComparison.Ordinal);
    }

    [Fact]
    public void PowerShellSetup_ChecksNativeGitExitCodes()
    {
        var script = RepositoryLocator.Read("build", "scripts", "setup-git.ps1");

        Assert.Contains("Set-StrictMode -Version Latest", script, StringComparison.Ordinal);
        Assert.Contains("function Invoke-Git", script, StringComparison.Ordinal);
        Assert.Contains("$LASTEXITCODE -ne 0", script, StringComparison.Ordinal);
        Assert.Contains("Repository-local Git user.email verification failed.", script, StringComparison.Ordinal);
    }
}
