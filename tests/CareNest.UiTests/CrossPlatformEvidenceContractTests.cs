namespace CareNest.UiTests;

public sealed class CrossPlatformEvidenceContractTests
{
    private const string LinuxTemplate = "docs/releases/templates/LINUX_DESKTOP_VALIDATION_RECORD.md";
    private const string BrowserTemplate = "docs/releases/templates/BROWSER_VALIDATION_RECORD.md";
    private const string EvidenceIndex = "docs/releases/PRODUCTION_EVIDENCE_INDEX.md";
    private const string CrossPlatformGuide = "docs/setup/CROSS_PLATFORM.md";
    private const string ReleaseGate = ".github/workflows/release-gate.yml";

    [Fact]
    public void Linux_template_starts_unperformed_and_forbids_build_to_parity_inference()
    {
        var text = Read(FindRepositoryRoot(), LinuxTemplate);

        Assert.Contains("Result status: `NOT RUN`", text, StringComparison.Ordinal);
        Assert.DoesNotContain("- [x]", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("configured desktop presentation/build reach", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Do not mark feature-parity rows `PASS`", text, StringComparison.Ordinal);
        Assert.Contains("Native/background notification capability is not inferred", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Secure-storage behavior is not claimed", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("fictional/synthetic", text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Browser_template_starts_unperformed_and_preserves_browser_sandbox_boundary()
    {
        var text = Read(FindRepositoryRoot(), BrowserTemplate);

        Assert.Contains("Result status: `NOT RUN`", text, StringComparison.Ordinal);
        Assert.DoesNotContain("- [x]", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("configured WebAssembly presentation/build reach", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Do not mark native or full-feature parity `PASS`", text, StringComparison.Ordinal);
        Assert.Contains("native MAUI behavior cannot be copied forward as browser evidence", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("browser storage", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("No hidden analytics/telemetry/network upload", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("fictional/synthetic", text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Evidence_index_and_release_gate_require_both_cross_platform_records()
    {
        var root = FindRepositoryRoot();
        var index = Read(root, EvidenceIndex);
        var releaseGate = Read(root, ReleaseGate);

        foreach (var fileName in new[]
                 {
                     Path.GetFileName(LinuxTemplate),
                     Path.GetFileName(BrowserTemplate),
                 })
        {
            Assert.Contains(fileName, index, StringComparison.Ordinal);
            Assert.Contains(fileName, releaseGate, StringComparison.Ordinal);
        }

        Assert.Contains("Linux build or WebAssembly publish is not production evidence", index, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Cross_platform_guide_links_validation_records_and_keeps_parity_explicit()
    {
        var text = Read(FindRepositoryRoot(), CrossPlatformGuide);

        Assert.Contains(Path.GetFileName(LinuxTemplate), text, StringComparison.Ordinal);
        Assert.Contains(Path.GetFileName(BrowserTemplate), text, StringComparison.Ordinal);
        Assert.Contains("Production feature parity is not implied by configured build support", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Both canonical files start `NOT RUN`", text, StringComparison.Ordinal);
        Assert.Contains("browser publish is automated build evidence", text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Repository_readme_surfaces_all_configured_platform_families_without_overclaiming_parity()
    {
        var text = Read(FindRepositoryRoot(), "README.md");

        foreach (var token in new[]
                 {
                     "net10.0-android",
                     "net10.0-ios",
                     "net10.0-maccatalyst",
                     "net10.0-windows10.0.19041.0",
                     "Linux desktop",
                     "net10.0-browser",
                     "docs/setup/CROSS_PLATFORM.md",
                 })
        {
            Assert.Contains(token, text, StringComparison.Ordinal);
        }

        Assert.Contains("configured build/presentation reach", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("production feature parity", text, StringComparison.OrdinalIgnoreCase);
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

        throw new DirectoryNotFoundException("Could not locate the CareNest repository root for cross-platform evidence contract tests.");
    }
}
