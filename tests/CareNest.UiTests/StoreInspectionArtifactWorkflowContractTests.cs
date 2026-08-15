namespace CareNest.UiTests;

public sealed class StoreInspectionArtifactWorkflowContractTests
{
    private static string Workflow =>
        RepositoryLocator.Read(".github", "workflows", "store-inspection-artifacts.yml");

    [Fact]
    public void Workflow_UsesConservativeStoreFundingPolicyAndReleaseTriggers()
    {
        var workflow = Workflow;

        Assert.Contains("workflow_dispatch:", workflow, StringComparison.Ordinal);
        Assert.Contains("pull_request:", workflow, StringComparison.Ordinal);
        Assert.Contains("branches: [main]", workflow, StringComparison.Ordinal);
        Assert.Contains("branches: [\"release/**\"]", workflow, StringComparison.Ordinal);
        Assert.Contains("tags: [\"v*\"]", workflow, StringComparison.Ordinal);
        Assert.Contains("CARENEST_STORE_FUNDING_LINK: \"false\"", workflow, StringComparison.Ordinal);
        Assert.Contains("CareNestShowFundingLink=${{ env.CARENEST_STORE_FUNDING_LINK }}", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void AndroidArtifact_IsUnsignedAabWithChecksumAndProvenance()
    {
        var workflow = Workflow;

        Assert.Contains("Android unsigned AAB inspection artifact", workflow, StringComparison.Ordinal);
        Assert.Contains("dotnet publish src/CareNest.App/CareNest.App.csproj", workflow, StringComparison.Ordinal);
        Assert.Contains("-p:AndroidKeyStore=false", workflow, StringComparison.Ordinal);
        Assert.Contains("-p:AndroidPackageFormats=aab", workflow, StringComparison.Ordinal);
        Assert.Contains("! -name '*-Signed.aab'", workflow, StringComparison.Ordinal);
        Assert.Contains("Expected exactly one unsigned Android AAB candidate", workflow, StringComparison.Ordinal);
        Assert.Contains("META-INF/[^[:space:]]+\\.(RSA|DSA|EC|SF)$", workflow, StringComparison.Ordinal);
        Assert.Contains("Android inspection AAB unexpectedly contains signing metadata.", workflow, StringComparison.Ordinal);
        Assert.Contains("sha256sum ./*.aab > SHA256SUMS.txt", workflow, StringComparison.Ordinal);
        Assert.Contains("signing=verified-unsigned", workflow, StringComparison.Ordinal);
        Assert.Contains("debug_signed_companion_staged=false", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("signing=disabled\n", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("AndroidSigningKeyStore", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("AndroidSigningKeyAlias", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("AndroidSigningKeyPass", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("AndroidSigningStorePass", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void WindowsArtifact_IsPortableSelfContainedInspectionBundle()
    {
        var workflow = Workflow;

        Assert.Contains("Windows self-contained inspection artifact", workflow, StringComparison.Ordinal);
        Assert.Contains("-p:RuntimeIdentifierOverride=win-x64", workflow, StringComparison.Ordinal);
        Assert.Contains("-p:WindowsPackageType=None", workflow, StringComparison.Ordinal);
        Assert.Contains("-p:WindowsAppSDKSelfContained=true", workflow, StringComparison.Ordinal);
        Assert.Contains("Get-FileHash -Algorithm SHA256", workflow, StringComparison.Ordinal);
        Assert.Contains("windows_package_type=None", workflow, StringComparison.Ordinal);
        Assert.Contains("store_submission_ready=false", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void AppleArtifacts_AreSimulatorOrUnsignedAndCarryChecksums()
    {
        var workflow = Workflow;

        Assert.Contains("iossimulator-arm64", workflow, StringComparison.Ordinal);
        Assert.Contains("maccatalyst-arm64", workflow, StringComparison.Ordinal);
        Assert.Contains("-p:CreatePackage=false", workflow, StringComparison.Ordinal);
        Assert.Contains("-p:EnableCodeSigning=false", workflow, StringComparison.Ordinal);
        Assert.Contains("shasum -a 256 ./*.tar.gz > SHA256SUMS.txt", workflow, StringComparison.Ordinal);
        Assert.Contains("code_signing=disabled_or_simulator_only", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("CodesignKey", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("CodesignProvision", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("PackageSigningKey", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void EveryInspectionArtifact_IsExplicitlyNonProductionAndUploadIsFailClosed()
    {
        var workflow = Workflow;

        Assert.True(workflow.Split("artifact_purpose=internal-inspection-only", StringSplitOptions.None).Length - 1 >= 3);
        Assert.True(workflow.Split("store_submission_ready=false", StringSplitOptions.None).Length - 1 >= 3);
        Assert.True(workflow.Split("if-no-files-found: error", StringSplitOptions.None).Length - 1 >= 3);
        Assert.True(workflow.Split("actions/upload-artifact@v4", StringSplitOptions.None).Length - 1 >= 3);
        Assert.Contains("source_sha=$GITHUB_SHA", workflow, StringComparison.Ordinal);
        Assert.Contains("source_sha=$env:GITHUB_SHA", workflow, StringComparison.Ordinal);
    }
}
