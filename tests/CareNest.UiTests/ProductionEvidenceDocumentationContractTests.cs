namespace CareNest.UiTests;

public sealed class ProductionEvidenceDocumentationContractTests
{
    private const string EvidenceStandard = "docs/releases/PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md";
    private const string EvidenceIndex = "docs/releases/PRODUCTION_EVIDENCE_INDEX.md";
    private const string AutomatedBaseline = "docs/releases/AUTOMATED_BASELINE.md";
    private const string ReleaseChecklist = "docs/releases/RELEASE_CHECKLIST.md";
    private const string ReleaseEvidence = "docs/releases/RELEASE_EVIDENCE.md";
    private const string DocumentationHub = "docs/README.md";
    private const string DocumentationCatalog = "docs/DOCUMENTATION_CATALOG.md";

    private static readonly string[] EvidenceTemplates =
    [
        "docs/releases/templates/ANDROID_DEVICE_VALIDATION_RECORD.md",
        "docs/releases/templates/WINDOWS_VALIDATION_RECORD.md",
        "docs/releases/templates/IOS_DEVICE_VALIDATION_RECORD.md",
        "docs/releases/templates/MACCATALYST_VALIDATION_RECORD.md",
        "docs/releases/templates/LINUX_DESKTOP_VALIDATION_RECORD.md",
        "docs/releases/templates/BROWSER_VALIDATION_RECORD.md",
        "docs/releases/templates/ACCESSIBILITY_VALIDATION_RECORD.md",
        "docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md",
        "docs/releases/templates/SIGNING_PROVENANCE_RECORD.md",
        "docs/releases/templates/STORE_SUBMISSION_RECORD.md",
        "docs/releases/templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md",
    ];

    private static readonly string[] MovingBaselineMarkers =
    [
        "94e867dce9519a8c1c71f1c4f1e5f833d6a3211f",
        "b6eecae66f74bd72bcb20d93508355542f9f3442",
        "30ee6c265104c64ec5a1a4013f592f7f058750e8",
        "**336/336**",
        "**355/355**",
        "**370/370**",
    ];

    [Fact]
    public void Production_evidence_standard_defines_fail_closed_result_states()
    {
        var root = FindRepositoryRoot();
        var standard = Read(root, EvidenceStandard);

        foreach (var state in new[] { "`PASS`", "`FAIL`", "`BLOCKED`", "`N/A`", "`NOT RUN`" })
        {
            Assert.Contains(state, standard, StringComparison.Ordinal);
        }

        Assert.Contains("must never be recorded as passed", standard, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("fictional or synthetic data", standard, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("private signing keys", standard, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("access tokens", standard, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(Path.GetFileName(AutomatedBaseline), standard, StringComparison.Ordinal);
    }

    [Fact]
    public void Production_evidence_templates_exist_and_start_unperformed()
    {
        var root = FindRepositoryRoot();

        foreach (var template in EvidenceTemplates)
        {
            var path = Path.Combine(root, template.Replace('/', Path.DirectorySeparatorChar));
            Assert.True(File.Exists(path), $"Missing production evidence template: {template}");

            var text = File.ReadAllText(path);
            Assert.Contains("NOT RUN", text, StringComparison.Ordinal);
            Assert.DoesNotContain("- [x]", text, StringComparison.OrdinalIgnoreCase);

            foreach (var marker in MovingBaselineMarkers)
            {
                Assert.DoesNotContain(marker, text, StringComparison.Ordinal);
            }
        }
    }

    [Fact]
    public void Production_evidence_index_links_every_canonical_template_without_pinning_moving_results()
    {
        var root = FindRepositoryRoot();
        var index = Read(root, EvidenceIndex);

        foreach (var template in EvidenceTemplates)
        {
            var relativeFromReleaseDirectory = template["docs/releases/".Length..];
            Assert.Contains(relativeFromReleaseDirectory, index, StringComparison.Ordinal);
        }

        Assert.Contains("PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md", index, StringComparison.Ordinal);
        Assert.Contains(Path.GetFileName(AutomatedBaseline), index, StringComparison.Ordinal);
        Assert.Contains("The templates are evidence containers, not evidence by themselves", index, StringComparison.OrdinalIgnoreCase);

        AssertNoMovingBaselineMarkers(index);
    }

    [Fact]
    public void Stable_release_evidence_documents_use_dynamic_automated_baseline_authority()
    {
        var root = FindRepositoryRoot();

        foreach (var relativePath in new[] { ReleaseChecklist, ReleaseEvidence })
        {
            var text = Read(root, relativePath);
            Assert.Contains(Path.GetFileName(AutomatedBaseline), text, StringComparison.Ordinal);
            Assert.Contains("PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md", text, StringComparison.Ordinal);
            Assert.Contains("PRODUCTION_EVIDENCE_INDEX.md", text, StringComparison.Ordinal);
            AssertNoMovingBaselineMarkers(text);
        }
    }

    [Fact]
    public void Release_checklist_links_canonical_production_records_without_claiming_template_execution()
    {
        var root = FindRepositoryRoot();
        var checklist = Read(root, ReleaseChecklist);

        Assert.Contains("templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md", checklist, StringComparison.Ordinal);
        Assert.Contains("templates/ANDROID_DEVICE_VALIDATION_RECORD.md", checklist, StringComparison.Ordinal);
        Assert.Contains("templates/WINDOWS_VALIDATION_RECORD.md", checklist, StringComparison.Ordinal);
        Assert.Contains("templates/IOS_DEVICE_VALIDATION_RECORD.md", checklist, StringComparison.Ordinal);
        Assert.Contains("templates/MACCATALYST_VALIDATION_RECORD.md", checklist, StringComparison.Ordinal);
        Assert.Contains("templates/ACCESSIBILITY_VALIDATION_RECORD.md", checklist, StringComparison.Ordinal);
        Assert.Contains("templates/SIGNING_PROVENANCE_RECORD.md", checklist, StringComparison.Ordinal);
        Assert.Contains("templates/STORE_SUBMISSION_RECORD.md", checklist, StringComparison.Ordinal);
        Assert.Contains("templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md", checklist, StringComparison.Ordinal);
        Assert.Contains("Templates live under `templates/` and must remain visibly unperformed", checklist, StringComparison.Ordinal);
    }

    [Fact]
    public void Release_evidence_links_current_production_records_and_forbids_false_approval()
    {
        var root = FindRepositoryRoot();
        var evidence = Read(root, ReleaseEvidence);

        Assert.Contains("templates/PRODUCTION_RELEASE_APPROVAL_RECORD.md", evidence, StringComparison.Ordinal);
        Assert.Contains("templates/STORE_SUBMISSION_RECORD.md", evidence, StringComparison.Ordinal);
        Assert.Contains("Green CI alone cannot mark production approved", evidence, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Canonical templates must remain visibly unperformed", evidence, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Documentation_navigation_surfaces_production_evidence_authorities_without_pinning_result_values_in_contracts()
    {
        var root = FindRepositoryRoot();
        var hub = Read(root, DocumentationHub);
        var catalog = Read(root, DocumentationCatalog);

        foreach (var document in new[]
                 {
                     "AUTOMATED_BASELINE.md",
                     "PRODUCTION_VALIDATION_EVIDENCE_STANDARD.md",
                     "PRODUCTION_EVIDENCE_INDEX.md",
                     "RELEASE_EVIDENCE.md",
                 })
        {
            Assert.Contains(document, hub, StringComparison.Ordinal);
            Assert.Contains(document, catalog, StringComparison.Ordinal);
        }

        Assert.Contains("ProductionEvidenceDocumentationContractTests.cs", hub, StringComparison.Ordinal);
        Assert.Contains("ProductionEvidenceDocumentationContractTests.cs", catalog, StringComparison.Ordinal);
    }

    [Fact]
    public void Packaged_compatibility_template_preserves_current_backup_resource_ceilings()
    {
        var root = FindRepositoryRoot();
        var template = Read(root, "docs/releases/templates/PACKAGED_COMPATIBILITY_VALIDATION_RECORD.md");

        Assert.Contains("2304 MiB", template, StringComparison.Ordinal);
        Assert.Contains("1 MiB", template, StringComparison.Ordinal);
        Assert.Contains("1 GiB", template, StringComparison.Ordinal);
        Assert.Contains("512 MiB", template, StringComparison.Ordinal);
        Assert.Contains("2 GiB", template, StringComparison.Ordinal);
        Assert.Contains("5,000", template, StringComparison.Ordinal);
        Assert.Contains("Never manufacture a current backup and label it historical evidence", template, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Signing_and_store_templates_keep_secrets_and_approval_states_explicit()
    {
        var root = FindRepositoryRoot();
        var signing = Read(root, "docs/releases/templates/SIGNING_PROVENANCE_RECORD.md");
        var store = Read(root, "docs/releases/templates/STORE_SUBMISSION_RECORD.md");

        Assert.Contains("Do not commit private keys", signing, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("access tokens", signing, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("recovery codes", signing, StringComparison.OrdinalIgnoreCase);

        foreach (var state in new[] { "NOT SUBMITTED", "SUBMITTED", "IN REVIEW", "REJECTED", "APPROVED", "PUBLISHED" })
        {
            Assert.Contains(state, store, StringComparison.Ordinal);
        }

        Assert.Contains("separates policy review, metadata completion, submission, approval and publication", store, StringComparison.OrdinalIgnoreCase);
    }

    private static void AssertNoMovingBaselineMarkers(string text)
    {
        foreach (var marker in MovingBaselineMarkers)
        {
            Assert.DoesNotContain(marker, text, StringComparison.Ordinal);
        }
    }

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

        throw new DirectoryNotFoundException("Could not locate the CareNest repository root for production-evidence documentation contract tests.");
    }
}