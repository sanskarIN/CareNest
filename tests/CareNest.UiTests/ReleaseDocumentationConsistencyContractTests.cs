namespace CareNest.UiTests;

public sealed class ReleaseDocumentationConsistencyContractTests
{
    private const string VerifiedGumroadSource = "94e867dce9519a8c1c71f1c4f1e5f833d6a3211f";
    private const string VerifiedCoreTotal = "336/336";
    private const string BuyMeACoffeeMarker = "buymeacoffee.com/sanskarIN";
    private const string GumroadMarker = "ramsandesh.gumroad.com";
    private const string StorePolicyReview = "docs/releases/STORE_POLICY_REVIEW_20260818.md";
    private const string PackageEvidenceGuide = "docs/releases/PACKAGE_EVIDENCE_TOOLING.md";

    [Fact]
    public void Current_release_documents_reference_the_verified_gumroad_baseline()
    {
        var root = FindRepositoryRoot();
        foreach (var relativePath in CurrentBaselineDocuments)
        {
            var text = Read(root, relativePath);
            Assert.Contains(VerifiedGumroadSource, text, StringComparison.Ordinal);
            Assert.Contains(VerifiedCoreTotal, text, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Current_release_documents_do_not_promote_the_superseded_331_test_baseline()
    {
        var root = FindRepositoryRoot();
        foreach (var relativePath in CurrentBaselineDocuments)
        {
            var text = Read(root, relativePath);
            Assert.DoesNotContain("Current verified executable source: `e8f4aa0a", text, StringComparison.Ordinal);
            Assert.DoesNotContain("Current accepted PR #74 source evidence", text, StringComparison.Ordinal);
            Assert.DoesNotContain("**331/331**", text, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Final_package_evidence_requires_both_repository_only_external_commerce_markers()
    {
        var root = FindRepositoryRoot();
        foreach (var relativePath in ExternalCommerceEvidenceDocuments)
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

        Assert.Contains("not store approval", review, StringComparison.OrdinalIgnoreCase);
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
    public void Live_store_declarations_and_submission_day_review_remain_open_production_gates()
    {
        var root = FindRepositoryRoot();
        var checklist = Read(root, "docs/releases/RELEASE_CHECKLIST.md");
        var submission = Read(root, "docs/releases/STORE_SUBMISSION_CHECKLIST.md");
        var nextSteps = Read(root, "docs/releases/NEXT_STEPS.md");

        Assert.Contains("- [ ] Complete live Google Play Health apps declaration", checklist, StringComparison.Ordinal);
        Assert.Contains("- [ ] Complete live Google Play Data safety", checklist, StringComparison.Ordinal);
        Assert.Contains("- [ ] Live Google Play Health apps declaration", nextSteps, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("- [ ] Re-open current Apple policy sources", submission, StringComparison.Ordinal);
        Assert.Contains("- [ ] Re-open current Google Play policy sources", submission, StringComparison.Ordinal);
        Assert.Contains("- [ ] Re-open current Microsoft Store policy sources", submission, StringComparison.Ordinal);
    }

    [Fact]
    public void Package_evidence_guide_is_part_of_current_release_governance()
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
    public void Production_release_gate_requires_current_evidence_documents_and_package_tooling()
    {
        var root = FindRepositoryRoot();
        var workflow = Read(root, ".github/workflows/release-gate.yml");

        foreach (var requiredPath in ReleaseGateRequiredPaths)
        {
            Assert.Contains(requiredPath, workflow, StringComparison.Ordinal);
        }

        Assert.Contains("python3 -m py_compile", workflow, StringComparison.Ordinal);
        Assert.Contains("python3 build/scripts/test-create-package-evidence.py", workflow, StringComparison.Ordinal);
    }

    private static readonly string[] CurrentBaselineDocuments =
    [
        "PROJECT_STATUS.md",
        "docs/COMPLETE_PROJECT_DOCUMENTATION.md",
        "docs/releases/STORE_BUILD_POLICY.md",
        "docs/releases/RELEASE_CHECKLIST.md",
        "docs/releases/RELEASE_EVIDENCE.md",
        "docs/releases/RELEASE_PROCESS.md",
        "docs/releases/STORE_SUBMISSION_CHECKLIST.md",
        "docs/releases/NEXT_STEPS.md",
        "docs/releases/QUALITY_GATE.md",
        "docs/releases/SECURITY_RELEASE_REVIEW.md",
        "docs/releases/MANUAL_TEST_MATRIX.md",
        "docs/releases/PACKAGED_RELEASE_VALIDATION.md",
        "docs/releases/VERIFICATION_BRANCH_PROTOCOL.md",
        "docs/DOCUMENTATION_CATALOG.md",
        "docs/README.md",
        "CHANGELOG.md",
        "what_changed.md",
    ];

    private static readonly string[] ExternalCommerceEvidenceDocuments =
    [
        "docs/COMPLETE_PROJECT_DOCUMENTATION.md",
        "docs/releases/STORE_BUILD_POLICY.md",
        "docs/releases/RELEASE_CHECKLIST.md",
        "docs/releases/RELEASE_EVIDENCE.md",
        "docs/releases/RELEASE_PROCESS.md",
        "docs/releases/STORE_SUBMISSION_CHECKLIST.md",
        "docs/releases/NEXT_STEPS.md",
        "docs/releases/QUALITY_GATE.md",
        "docs/releases/SECURITY_RELEASE_REVIEW.md",
        "docs/releases/MANUAL_TEST_MATRIX.md",
        "docs/releases/PACKAGED_RELEASE_VALIDATION.md",
    ];

    private static readonly string[] PolicyReviewLinkDocuments =
    [
        "PROJECT_STATUS.md",
        "docs/COMPLETE_PROJECT_DOCUMENTATION.md",
        "docs/releases/STORE_BUILD_POLICY.md",
        "docs/releases/RELEASE_CHECKLIST.md",
        "docs/releases/RELEASE_EVIDENCE.md",
        "docs/releases/RELEASE_PROCESS.md",
        "docs/releases/STORE_SUBMISSION_CHECKLIST.md",
        "docs/releases/NEXT_STEPS.md",
        "docs/releases/QUALITY_GATE.md",
        "docs/releases/SECURITY_RELEASE_REVIEW.md",
        "docs/releases/MANUAL_TEST_MATRIX.md",
        "docs/releases/PACKAGED_RELEASE_VALIDATION.md",
        "docs/DOCUMENTATION_CATALOG.md",
        "docs/README.md",
    ];

    private static readonly string[] PackageEvidenceLinkDocuments =
    [
        "docs/COMPLETE_PROJECT_DOCUMENTATION.md",
        "docs/releases/RELEASE_EVIDENCE.md",
        "docs/releases/EXECUTABLE_BUILD_CHECKLIST.md",
        "docs/releases/QUALITY_GATE.md",
        "docs/releases/SECURITY_RELEASE_REVIEW.md",
        "docs/releases/MANUAL_TEST_MATRIX.md",
        "docs/releases/PACKAGED_RELEASE_VALIDATION.md",
    ];

    private static readonly string[] ReleaseGateRequiredPaths =
    [
        "docs/releases/STORE_SUBMISSION_CHECKLIST.md",
        "docs/releases/STORE_POLICY_REVIEW_20260818.md",
        "docs/releases/PACKAGE_EVIDENCE_TOOLING.md",
        "build/scripts/create-package-evidence.py",
        "build/scripts/test-create-package-evidence.py",
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
