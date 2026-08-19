namespace CareNest.UiTests;

public sealed class ReleaseDocumentationConsistencyContractTests
{
    private const string BuyMeACoffeeMarker = "buymeacoffee.com/sanskarIN";
    private const string GumroadMarker = "ramsandesh.gumroad.com";
    private const string StorePolicyReview = "docs/releases/STORE_POLICY_REVIEW_20260818.md";
    private const string PackageEvidenceGuide = "docs/releases/PACKAGE_EVIDENCE_TOOLING.md";
    private const string AutomatedBaselineRecord = "docs/releases/AUTOMATED_BASELINE.md";

    [Fact]
    public void Stable_release_policy_documents_do_not_promote_superseded_intermediate_baselines()
    {
        var root = FindRepositoryRoot();
        foreach (var relativePath in StableReleasePolicyDocuments)
        {
            var text = Read(root, relativePath);
            Assert.DoesNotContain("Current verified executable source: `e8f4aa0a", text, StringComparison.Ordinal);
            Assert.DoesNotContain("Current accepted PR #74 source evidence", text, StringComparison.Ordinal);
            Assert.DoesNotContain("**331/331**", text, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Dynamic_automated_baseline_record_exists_without_becoming_a_content_assertion_input()
    {
        var root = FindRepositoryRoot();
        Assert.True(File.Exists(Path.Combine(
            root,
            AutomatedBaselineRecord.Replace('/', Path.DirectorySeparatorChar))));
    }

    [Fact]
    public void Stable_final_package_policy_requires_both_repository_only_external_commerce_markers()
    {
        var root = FindRepositoryRoot();
        foreach (var relativePath in ExternalCommercePolicyDocuments)
        {
            var text = Read(root, relativePath);
            Assert.Contains(BuyMeACoffeeMarker, text, StringComparison.Ordinal);
            Assert.Contains(GumroadMarker, text, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Store_policy_review_is_linked_but_not_misrepresented_as_store_approval()
    {
        var root = FindRepositoryRoot();
        var review = Read(root, StorePolicyReview);
        var reviewWithoutMarkdownEmphasis = review.Replace("**", string.Empty, StringComparison.Ordinal);

        Assert.Contains("not store approval", reviewWithoutMarkdownEmphasis, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("submission", review, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Google Play", review, StringComparison.Ordinal);
        Assert.Contains("Apple", review, StringComparison.Ordinal);
        Assert.Contains("Microsoft", review, StringComparison.Ordinal);

        foreach (var relativePath in PolicyReviewLinkDocuments)
        {
            var text = Read(root, relativePath);
            Assert.Contains(StorePolicyReview, text, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Stable_store_submission_policy_keeps_live_declarations_and_submission_day_review_open()
    {
        var root = FindRepositoryRoot();
        var submission = Read(root, "docs/releases/STORE_SUBMISSION_CHECKLIST.md");

        Assert.Contains("- [ ] Live Google Play Health apps declaration", submission, StringComparison.Ordinal);
        Assert.Contains("- [ ] Live Google Play Data safety", submission, StringComparison.Ordinal);
        Assert.Contains("- [ ] Re-open current Apple policy sources", submission, StringComparison.Ordinal);
        Assert.Contains("- [ ] Re-open current Google Play policy sources", submission, StringComparison.Ordinal);
        Assert.Contains("- [ ] Re-open current Microsoft Store policy sources", submission, StringComparison.Ordinal);
    }

    [Fact]
    public void Package_evidence_guide_is_part_of_stable_release_governance()
    {
        var root = FindRepositoryRoot();
        Assert.True(File.Exists(Path.Combine(root, "docs", "releases", "PACKAGE_EVIDENCE_TOOLING.md")));

        foreach (var relativePath in PackageEvidenceLinkDocuments)
        {
            var text = Read(root, relativePath);
            Assert.Contains(PackageEvidenceGuide, text, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Production_release_gate_requires_current_evidence_documents_and_tooling()
    {
        var root = FindRepositoryRoot();
        var workflow = Read(root, ".github/workflows/release-gate.yml");

        foreach (var requiredPath in ReleaseGateRequiredPaths)
        {
            Assert.Contains(requiredPath, workflow, StringComparison.Ordinal);
        }

        Assert.Contains("python3 -m py_compile", workflow, StringComparison.Ordinal);
        Assert.Contains("python3 build/scripts/test-create-package-evidence.py", workflow, StringComparison.Ordinal);
        Assert.Contains("python3 build/scripts/test-verify-documentation-links.py", workflow, StringComparison.Ordinal);
        Assert.Contains("python3 build/scripts/verify-documentation-links.py", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void Production_release_gate_requires_every_canonical_production_evidence_template()
    {
        var root = FindRepositoryRoot();
        var workflow = Read(root, ".github/workflows/release-gate.yml");

        foreach (var template in ProductionEvidenceTemplates)
        {
            Assert.Contains(template, workflow, StringComparison.Ordinal);
        }
    }

    private static readonly string[] StableReleasePolicyDocuments =
    [
        "docs/releases/STORE_BUILD_POLICY.md",
        "docs/releases/RELEASE_EVIDENCE.md",
        "docs/releases/RELEASE_PROCESS.md",
        "docs/releases/STORE_SUBMISSION_CHECKLIST.md",
        "docs/releases/QUALITY_GATE.md",
        "docs/releases/SECURITY_RELEASE_REVIEW.md",
        "docs/releases/MANUAL_TEST_MATRIX.md",
        "docs/releases/PACKAGED_RELEASE_VALIDATION.md",
        "docs/releases/VERIFICATION_BRANCH_PROTOCOL.md",
    ];

    private static readonly string[] ExternalCommercePolicyDocuments =
    [
        "docs/releases/STORE_BUILD_POLICY.md",
        "docs/releases/RELEASE_EVIDENCE.md",
        "docs/releases/RELEASE_PROCESS.md",
        "docs/releases/STORE_SUBMISSION_CHECKLIST.md",
        "docs/releases/QUALITY_GATE.md",
        "docs/releases/SECURITY_RELEASE_REVIEW.md",
        "docs/releases/MANUAL_TEST_MATRIX.md",
        "docs/releases/PACKAGED_RELEASE_VALIDATION.md",
    ];

    private static readonly string[] PolicyReviewLinkDocuments =
    [
        "docs/releases/STORE_BUILD_POLICY.md",
        "docs/releases/RELEASE_EVIDENCE.md",
        "docs/releases/RELEASE_PROCESS.md",
        "docs/releases/STORE_SUBMISSION_CHECKLIST.md",
        "docs/releases/QUALITY_GATE.md",
        "docs/releases/SECURITY_RELEASE_REVIEW.md",
        "docs/releases/MANUAL_TEST_MATRIX.md",
        "docs/releases/PACKAGED_RELEASE_VALIDATION.md",
    ];

    private static readonly string[] PackageEvidenceLinkDocuments =
    [
        "docs/releases/RELEASE_EVIDENCE.md",
        "docs/releases/EXECUTABLE_BUILD_CHECKLIST.md",
        "docs/releases/QUALITY_GATE.md",
        "docs/releases/SECURITY_RELEASE_REVIEW.md",
        "docs/releases/MANUAL_TEST_MATRIX.md",
        "docs/releases/PACKAGED_RELEASE_VALIDATION.md",
    ];

    private static readonly string[] ProductionEvidenceTemplates =
    [
        "docs/releases/templates/ANDROID_DEVICE_VALIDATION_RECORD.md",
        "docs/releases/templates/WINDOWS_VALIDATION_RECORD.md",
        "docs/releases/templates/IOS_DEVICE_VALIDATION_RECORD.md",
        "docs/releases/templates/MACCATALYST_VALIDATION_RECORD.md",
        "docs/releases/templates/ACCESSIBILITY_VALIDATION_RECORD.md",
        "docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md",
        "docs/releases/templates/SIGNING_PROVENANCE_RECORD.md",
        "docs/releases/templates/STORE_SUBMISSION_RECORD.md",
        "docs/releases/templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md",
    ];

    private static readonly string[] ReleaseGateRequiredPaths =
    [
        "docs/releases/STORE_SUBMISSION_CHECKLIST.md",
        "docs/releases/STORE_POLICY_REVIEW_20260818.md",
        "docs/releases/AUTOMATED_BASELINE.md",
        "docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md",
        "docs/releases/PRODUCTION_EVIDENCE_INDEX.md",
        "docs/releases/PACKAGE_EVIDENCE_TOOLING.md",
        "docs/testing/DOCUMENTATION_INTEGRITY.md",
        "build/scripts/create-package-evidence.py",
        "build/scripts/test-create-package-evidence.py",
        "build/scripts/verify-documentation-links.py",
        "build/scripts/test-verify-documentation-links.py",
    ];

    private static string Read(string root, string relativePath) =>
        File.ReadAllText(Path.Combine(root, relativePath.Replace('/', Path.DirectorySeparatorChar)));

    private static string FindRepositoryRoot()
    {
        foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            var directory = new DirectoryInfo(start);
            while (directory is not null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "CareNest.sln")))
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }
        }

        throw new DirectoryNotFoundException("Could not locate the CareNest repository root for release-documentation contract tests.");
    }
}
