namespace CareNest.UiTests;

public sealed class StorePackageWorkflowContractTests
{
    private const string WorkflowPath = "store-package-verification.yml";

    [Fact]
    public void StorePackageWorkflow_RunsForPullRequestsTagsAndManualVerification()
    {
        var workflow = ReadWorkflow();

        Assert.Contains("workflow_dispatch:", workflow, StringComparison.Ordinal);
        Assert.Contains("pull_request:", workflow, StringComparison.Ordinal);
        Assert.Contains("tags: [\"v*\"]", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void StorePackageWorkflow_DoesNotDependOnObsoleteFundingToggle()
    {
        var workflow = ReadWorkflow();

        Assert.DoesNotContain("CARENEST_STORE_FUNDING_LINK", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("CareNestShowFundingLink", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("external funding surface disabled", workflow, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("store candidate configuration", workflow, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void StorePackageWorkflow_VerifiesBashStorePreflightExecutableMode()
    {
        var workflow = ReadWorkflow();

        Assert.Contains("Verify Bash store preflight is executable", workflow, StringComparison.Ordinal);
        Assert.Contains("test -x build/scripts/store-package-preflight.sh", workflow, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("net10.0-android")]
    [InlineData("net10.0-windows10.0.19041.0")]
    [InlineData("net10.0-ios")]
    [InlineData("net10.0-maccatalyst")]
    public void StorePackageWorkflow_CoversEverySupportedTarget(string targetFramework)
    {
        var workflow = ReadWorkflow();

        Assert.Contains($"-f {targetFramework}", workflow, StringComparison.Ordinal);
        Assert.Contains($"-p:CareNestTargetFramework={targetFramework}", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void StorePackageWorkflow_UsesIosSimulatorRatherThanProductionSigning()
    {
        var workflow = ReadWorkflow();

        Assert.Contains("-p:RuntimeIdentifier=iossimulator-arm64", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("CodesignKey", workflow, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ProvisioningProfile", workflow, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("keystore", workflow, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void StorePackageWorkflow_DoesNotPublishUnsignedBuildOutputs()
    {
        var workflow = ReadWorkflow();

        Assert.DoesNotContain("actions/upload-artifact", workflow, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("dotnet publish", workflow, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("release create", workflow, StringComparison.OrdinalIgnoreCase);
    }

    private static string ReadWorkflow() =>
        RepositoryLocator.Read(".github", "workflows", WorkflowPath);
}
