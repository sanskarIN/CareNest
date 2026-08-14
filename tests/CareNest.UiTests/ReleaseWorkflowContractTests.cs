namespace CareNest.UiTests;

public sealed class ReleaseWorkflowContractTests
{
    [Theory]
    [InlineData("ci.yml")]
    [InlineData("codeql.yml")]
    [InlineData("dependency-review.yml")]
    public void ExactReleaseVerificationWorkflows_SupportTagAndManualExecution(string workflowName)
    {
        var workflow = RepositoryLocator.Read(".github", "workflows", workflowName);

        Assert.Contains("workflow_dispatch:", workflow, StringComparison.Ordinal);
        Assert.Contains("tags: [\"v*\"]", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void DependencyAudit_PullRequestDiffStep_IsGuardedForTagAndManualRuns()
    {
        var workflow = RepositoryLocator.Read(".github", "workflows", "dependency-review.yml");

        Assert.Contains("if: github.event_name == 'pull_request'", workflow, StringComparison.Ordinal);
        Assert.Contains("github.event.pull_request.base.sha", workflow, StringComparison.Ordinal);
        Assert.Contains("github.event.pull_request.head.sha", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void ReleaseEvidence_CapturesSourceProvenanceAndCompleteTestOutcomes()
    {
        var workflow = RepositoryLocator.Read(".github", "workflows", "release-evidence.yml");

        Assert.Contains("git ls-files > artifacts/source/tracked-files.txt", workflow, StringComparison.Ordinal);
        Assert.Contains("tracked-files-SHA256SUMS", workflow, StringComparison.Ordinal);
        Assert.Contains("github-run-id.txt", workflow, StringComparison.Ordinal);
        Assert.Contains("id: unit", workflow, StringComparison.Ordinal);
        Assert.Contains("id: integration", workflow, StringComparison.Ordinal);
        Assert.Contains("id: ui", workflow, StringComparison.Ordinal);
        Assert.Contains("id: dependencies", workflow, StringComparison.Ordinal);
        Assert.Contains("id: workspace", workflow, StringComparison.Ordinal);
        Assert.Contains("Require complete successful release evidence", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void ReleaseEvidence_UploadsFailureEvidenceAndKeepsItForNinetyDays()
    {
        var workflow = RepositoryLocator.Read(".github", "workflows", "release-evidence.yml");
        var uploadIndex = workflow.IndexOf("name: Upload immutable run evidence", StringComparison.Ordinal);
        var finalGateIndex = workflow.IndexOf("name: Require complete successful release evidence", StringComparison.Ordinal);

        Assert.True(uploadIndex >= 0);
        Assert.True(finalGateIndex > uploadIndex);
        Assert.Contains("if: always()", workflow[uploadIndex..finalGateIndex], StringComparison.Ordinal);
        Assert.Contains("retention-days: 90", workflow, StringComparison.Ordinal);
    }
}
