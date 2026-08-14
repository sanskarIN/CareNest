namespace CareNest.UiTests;

public sealed class ReleaseGateContractTests
{
    [Fact]
    public void ReleaseGate_BlocksIndentedUncheckedChecklistItems()
    {
        var workflow = RepositoryLocator.Read(".github", "workflows", "release-gate.yml");

        Assert.Contains(
            "^[[:space:]]*-[[:space:]]+\\[[[:space:]]\\]",
            workflow,
            StringComparison.Ordinal);
    }

    [Fact]
    public void ReleaseGate_DetectsOpenRiskStatusWithoutCaseOrIndentationBypass()
    {
        var workflow = RepositoryLocator.Read(".github", "workflows", "release-gate.yml");

        Assert.Contains("grep -Eiq", workflow, StringComparison.Ordinal);
        Assert.Contains("Status:", workflow, StringComparison.Ordinal);
        Assert.Contains("Open", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void ReleaseGate_RequiresSecurityAndEvidenceDocuments()
    {
        var workflow = RepositoryLocator.Read(".github", "workflows", "release-gate.yml");

        Assert.Contains("docs/releases/RELEASE_EVIDENCE.md", workflow, StringComparison.Ordinal);
        Assert.Contains("docs/releases/SECURITY_RELEASE_REVIEW.md", workflow, StringComparison.Ordinal);
        Assert.Contains("timeout-minutes:", workflow, StringComparison.Ordinal);
    }
}
