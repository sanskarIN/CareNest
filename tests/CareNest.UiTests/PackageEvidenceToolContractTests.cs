namespace CareNest.UiTests;

public sealed class PackageEvidenceToolContractTests
{
    [Fact]
    public void Package_evidence_tooling_files_exist()
    {
        var root = FindRepositoryRoot();

        Assert.True(File.Exists(Path.Combine(root, "build", "scripts", "create-package-evidence.py")));
        Assert.True(File.Exists(Path.Combine(root, "build", "scripts", "create-package-evidence.sh")));
        Assert.True(File.Exists(Path.Combine(root, "build", "scripts", "create-package-evidence.ps1")));
        Assert.True(File.Exists(Path.Combine(root, "build", "scripts", "test-create-package-evidence.py")));
        Assert.True(File.Exists(Path.Combine(root, "docs", "releases", "PACKAGE_EVIDENCE_TOOLING.md")));
    }

    [Fact]
    public void Production_evidence_requires_exact_tagged_clean_source_and_signing_provenance()
    {
        var root = FindRepositoryRoot();
        var tool = Read(root, "build/scripts/create-package-evidence.py");

        Assert.Contains("Production evidence requires --source-tag", tool, StringComparison.Ordinal);
        Assert.Contains("tag_sha != source_sha", tool, StringComparison.Ordinal);
        Assert.Contains("head_sha != source_sha", tool, StringComparison.Ordinal);
        Assert.Contains("Production evidence requires a clean tracked Git workspace", tool, StringComparison.Ordinal);
        Assert.Contains("Production evidence requires real non-secret signing/notarization provenance", tool, StringComparison.Ordinal);
        Assert.Contains("rev-parse", tool, StringComparison.Ordinal);
        Assert.Contains("--verify", tool, StringComparison.Ordinal);
    }

    [Fact]
    public void Package_evidence_generation_runs_store_safe_scanner_and_hashes_every_payload_file()
    {
        var root = FindRepositoryRoot();
        var tool = Read(root, "build/scripts/create-package-evidence.py");

        Assert.Contains("verify-store-safe-payload.py", tool, StringComparison.Ordinal);
        Assert.Contains("run_store_safe_scan", tool, StringComparison.Ordinal);
        Assert.Contains("hashlib.sha256", tool, StringComparison.Ordinal);
        Assert.Contains("payload.rglob", tool, StringComparison.Ordinal);
        Assert.Contains("files", tool, StringComparison.Ordinal);
        Assert.Contains("storeSafePayloadScan", tool, StringComparison.Ordinal);
        Assert.Contains("Evidence output must be outside the payload directory", tool, StringComparison.Ordinal);
    }

    [Fact]
    public void Synthetic_self_test_covers_success_and_fail_closed_paths()
    {
        var root = FindRepositoryRoot();
        var selfTest = Read(root, "build/scripts/test-create-package-evidence.py");

        Assert.Contains("test_safe_file_manifest", selfTest, StringComparison.Ordinal);
        Assert.Contains("test_safe_directory_manifest", selfTest, StringComparison.Ordinal);
        Assert.Contains("test_forbidden_marker_fails_closed", selfTest, StringComparison.Ordinal);
        Assert.Contains("test_output_inside_payload_is_rejected", selfTest, StringComparison.Ordinal);
        Assert.Contains("test_production_requires_tag", selfTest, StringComparison.Ordinal);
        Assert.Contains("ramsandesh.gumroad.com", selfTest, StringComparison.Ordinal);
    }

    [Fact]
    public void CareNest_ci_compiles_and_runs_package_evidence_self_test()
    {
        var root = FindRepositoryRoot();
        var workflow = Read(root, ".github/workflows/ci.yml");

        Assert.Contains("python3 -m py_compile", workflow, StringComparison.Ordinal);
        Assert.Contains("build/scripts/create-package-evidence.py", workflow, StringComparison.Ordinal);
        Assert.Contains("build/scripts/test-create-package-evidence.py", workflow, StringComparison.Ordinal);
        Assert.Contains("Self-test package evidence tooling", workflow, StringComparison.Ordinal);
    }

    [Fact]
    public void Package_evidence_guide_preserves_secret_and_release_boundaries()
    {
        var root = FindRepositoryRoot();
        var guide = Read(root, "docs/releases/PACKAGE_EVIDENCE_TOOLING.md");

        Assert.Contains("does **not**", guide, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("store approval", guide, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Do not put into evidence", guide, StringComparison.Ordinal);
        Assert.Contains("private signing keys", guide, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("buymeacoffee.com/sanskarIN", guide, StringComparison.Ordinal);
        Assert.Contains("ramsandesh.gumroad.com", guide, StringComparison.Ordinal);
        Assert.Contains("real-device", guide, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("accessibility", guide, StringComparison.OrdinalIgnoreCase);
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

        throw new DirectoryNotFoundException("Could not locate the CareNest repository root for package-evidence contract tests.");
    }
}
