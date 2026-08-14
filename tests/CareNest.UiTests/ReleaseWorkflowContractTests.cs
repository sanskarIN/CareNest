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
}
