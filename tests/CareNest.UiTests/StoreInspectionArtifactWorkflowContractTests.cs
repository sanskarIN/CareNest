namespace CareNest.UiTests;

public sealed class StoreInspectionArtifactWorkflowContractTests
{
    private static string Workflow =>
        RepositoryLocator.Read(".github", "workflows", "store-inspection-artifacts.yml");

    [Fact]
    public void Workflow_UsesFundingFreeSourcePolicyAndReleaseTriggers()
    {
        var workflow = Workflow;

        Assert.Contains("workflow_dispatch:", workflow, StringComparison.Ordinal);
        Assert.Contains("pull_request:", workflow, StringComparison.Ordinal);
        Assert.Contains("branches: [main]", workflow, StringComparison.Ordinal);
        Assert.Contains("branches: [\"release/**\"]", workflow, StringComparison.Ordinal);
        Assert.Contains("tags: [\"v*\"]", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("CARENEST_STORE_FUNDING_LINK", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("CareNestShowFundingLink", workflow, StringComparison.Ordinal);
        Assert.True(workflow.Split("external_funding_surface=absent_by_source_policy", StringSplitOptions.None).Length - 1 >= 3);
    }

    [Fact]
    public void Workflow_SeparatesVerificationSourceFromPullRequestEventMergeIdentity()
    {
        var workflow = Workflow;

        Assert.Contains("CARENEST_SOURCE_SHA: ${{ github.event.pull_request.head.sha || github.sha }}", workflow, StringComparison.Ordinal);
        Assert.Contains("CARENEST_SOURCE_REF: ${{ github.head_ref || github.ref_name }}", workflow, StringComparison.Ordinal);
        Assert.True(workflow.Split("ref: ${{ env.CARENEST_SOURCE_SHA }}", StringSplitOptions.None).Length - 1 >= 4);
        Assert.True(workflow.Split("source_sha=$CARENEST_SOURCE_SHA", StringSplitOptions.None).Length - 1 >= 2);
        Assert.Contains("source_sha=$env:CARENEST_SOURCE_SHA", workflow, StringComparison.Ordinal);
        Assert.True(workflow.Split("event_sha=$GITHUB_SHA", StringSplitOptions.None).Length - 1 >= 2);
        Assert.Contains("event_sha=$env:GITHUB_SHA", workflow, StringComparison.Ordinal);
        Assert.True(workflow.Split("${{ env.CARENEST_SOURCE_SHA }}", StringSplitOptions.None).Length - 1 >= 7);
    }

    [Fact]
    public void PayloadScannerSelfTest_ProvesCleanPassesAndForbiddenPayloadsFail()
    {
        var workflow = Workflow;

        Assert.Contains("payload-scanner-self-test:", workflow, StringComparison.Ordinal);
        Assert.Contains("Store-safe payload scanner self-test", workflow, StringComparison.Ordinal);
        Assert.Contains("clean.bin", workflow, StringComparison.Ordinal);
        Assert.Contains("utf8.bin", workflow, StringComparison.Ordinal);
        Assert.Contains("utf16.bin", workflow, StringComparison.Ordinal);
        Assert.Contains("nested.aab", workflow, StringComparison.Ordinal);
        Assert.Contains("does-not-exist", workflow, StringComparison.Ordinal);
        Assert.Contains("Scanner failed to reject a UTF-8 funding marker.", workflow, StringComparison.Ordinal);
        Assert.Contains("Scanner failed to reject a UTF-16 funding marker.", workflow, StringComparison.Ordinal);
        Assert.Contains("Scanner failed to reject a funding marker inside a ZIP/AAB entry.", workflow, StringComparison.Ordinal);
        Assert.Contains("Scanner failed open for a missing payload path.", workflow, StringComparison.Ordinal);
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
        Assert.Contains("python3 build/scripts/verify-store-safe-payload.py \"$bundle\"", workflow, StringComparison.Ordinal);
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
        Assert.Contains("python build/scripts/verify-store-safe-payload.py \"$publishDir\"", workflow, StringComparison.Ordinal);
        Assert.Contains("Store-safe payload funding scan failed for Windows.", workflow, StringComparison.Ordinal);
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
        Assert.Contains("python3 build/scripts/verify-store-safe-payload.py \"$ios_app\"", workflow, StringComparison.Ordinal);
        Assert.Contains("python3 build/scripts/verify-store-safe-payload.py \"$mac_app\"", workflow, StringComparison.Ordinal);
        Assert.Contains("shasum -a 256 ./*.tar.gz > SHA256SUMS.txt", workflow, StringComparison.Ordinal);
        Assert.Contains("code_signing=disabled_or_simulator_only", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("CodesignKey", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("CodesignProvision", workflow, StringComparison.Ordinal);
        Assert.DoesNotContain("PackageSigningKey", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void EveryInspectionArtifact_RequiresFundingUrlPayloadScanBeforeUpload()
    {
        var workflow = Workflow;

        Assert.True(workflow.Split("verify-store-safe-payload.py", StringSplitOptions.None).Length - 1 >= 9);
        Assert.True(workflow.Split("funding_url_payload_scan=passed", StringSplitOptions.None).Length - 1 >= 3);
        Assert.True(workflow.Split("external_funding_surface=absent_by_source_policy", StringSplitOptions.None).Length - 1 >= 3);
    }

    [Fact]
    public void EveryInspectionArtifact_IsExplicitlyNonProductionAndUploadIsFailClosed()
    {
        var workflow = Workflow;

        Assert.True(workflow.Split("artifact_purpose=internal-inspection-only", StringSplitOptions.None).Length - 1 >= 3);
        Assert.True(workflow.Split("store_submission_ready=false", StringSplitOptions.None).Length - 1 >= 3);
        Assert.True(workflow.Split("if-no-files-found: error", StringSplitOptions.None).Length - 1 >= 3);
        Assert.True(workflow.Split("actions/upload-artifact@v4", StringSplitOptions.None).Length - 1 >= 3);
    }
}
