namespace CareNest.UiTests;

public sealed class DocumentationIntegrityToolContractTests
{
    [Fact]
    public void Documentation_integrity_tooling_files_exist()
    {
        var root = FindRepositoryRoot();

        Assert.True(File.Exists(Path.Combine(root, "build", "scripts", "verify-documentation-links.py")));
        Assert.True(File.Exists(Path.Combine(root, "build", "scripts", "test-verify-documentation-links.py")));
        Assert.True(File.Exists(Path.Combine(root, "docs", "testing", "DOCUMENTATION_INTEGRITY.md")));
    }

    [Fact]
    public void Stable_checker_excludes_dynamic_evidence_and_history_by_default()
    {
        var root = FindRepositoryRoot();
        var tool = Read(root, "build/scripts/verify-documentation-links.py");

        Assert.Contains("DYNAMIC_EVIDENCE_PATHS", tool, StringComparison.Ordinal);
        Assert.Contains("PROJECT_STATUS.md", tool, StringComparison.Ordinal);
        Assert.Contains("what_changed.md", tool, StringComparison.Ordinal);
        Assert.Contains("docs/releases/AUTOMATED_BASELINE.md", tool, StringComparison.Ordinal);
        Assert.Contains("docs/releases/NEXT_STEPS.md", tool, StringComparison.Ordinal);
        Assert.Contains("--include-dynamic", tool, StringComparison.Ordinal);
        Assert.Contains("--include-history", tool, StringComparison.Ordinal);
        Assert.Contains("is_dynamic_evidence_path", tool, StringComparison.Ordinal);
        Assert.Contains("is_history_path", tool, StringComparison.Ordinal);
    }

    [Fact]
    public void Documentation_checker_ignores_non_live_code_and_comment_examples()
    {
        var root = FindRepositoryRoot();
        var tool = Read(root, "build/scripts/verify-documentation-links.py");
        var selfTest = Read(root, "build/scripts/test-verify-documentation-links.py");

        Assert.Contains("strip_fenced_code_blocks", tool, StringComparison.Ordinal);
        Assert.Contains("HTML_COMMENT_RE", tool, StringComparison.Ordinal);
        Assert.Contains("INLINE_CODE_RE", tool, StringComparison.Ordinal);
        Assert.Contains("example-only-missing.svg", selfTest, StringComparison.Ordinal);
        Assert.Contains("inline-example-missing.md", selfTest, StringComparison.Ordinal);
        Assert.Contains("comment-example-missing.md", selfTest, StringComparison.Ordinal);
        Assert.Contains(
            "Clean synthetic documentation and example-only links should pass",
            selfTest,
            StringComparison.Ordinal);
    }

    [Fact]
    public void Documentation_checker_fails_closed_for_missing_and_repository_escaping_targets()
    {
        var root = FindRepositoryRoot();
        var tool = Read(root, "build/scripts/verify-documentation-links.py");
        var selfTest = Read(root, "build/scripts/test-verify-documentation-links.py");

        Assert.Contains("candidate.exists()", tool, StringComparison.Ordinal);
        Assert.Contains("escapes repository root", tool, StringComparison.Ordinal);
        Assert.Contains("Missing live local targets must fail closed", selfTest, StringComparison.Ordinal);
        Assert.Contains("Repository-escaping links must fail closed", selfTest, StringComparison.Ordinal);
        Assert.Contains("--include-dynamic must audit dynamic evidence/status links", selfTest, StringComparison.Ordinal);
        Assert.Contains("--include-history must audit historical snapshots", selfTest, StringComparison.Ordinal);
    }

    [Fact]
    public void CareNest_ci_runs_documentation_syntax_self_test_and_stable_link_check()
    {
        var root = FindRepositoryRoot();
        var workflow = Read(root, ".github/workflows/ci.yml");

        Assert.Contains("build/scripts/verify-documentation-links.py", workflow, StringComparison.Ordinal);
        Assert.Contains("build/scripts/test-verify-documentation-links.py", workflow, StringComparison.Ordinal);
        Assert.Contains("Self-test documentation link tooling", workflow, StringComparison.Ordinal);
        Assert.Contains("Verify active documentation local links", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void Release_gate_and_release_evidence_enforce_documentation_integrity()
    {
        var root = FindRepositoryRoot();
        var releaseGate = Read(root, ".github/workflows/release-gate.yml");
        var releaseEvidence = Read(root, ".github/workflows/release-evidence.yml");

        Assert.Contains("build/scripts/verify-documentation-links.py", releaseGate, StringComparison.Ordinal);
        Assert.Contains("build/scripts/test-verify-documentation-links.py", releaseGate, StringComparison.Ordinal);
        Assert.Contains("documentation-link-self-test.txt", releaseEvidence, StringComparison.Ordinal);
        Assert.Contains("documentation-link-check.txt", releaseEvidence, StringComparison.Ordinal);
        Assert.Contains("documentation=${{ steps.documentation.outcome }}", releaseEvidence, StringComparison.Ordinal);
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

        throw new DirectoryNotFoundException("Could not locate the CareNest repository root for documentation-integrity contract tests.");
    }
}
