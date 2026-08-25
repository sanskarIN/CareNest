using System.Xml.Linq;

namespace CareNest.UiTests;

public sealed class ActiveReleaseLineContractTests
{
    [Fact]
    public void Dynamic_release_documents_follow_the_active_source_version_and_build()
    {
        var root = FindRepositoryRoot();
        var version = PropertyValue(
            XDocument.Load(Path.Combine(root, "Directory.Build.props")),
            "Version");
        var applicationBuild = PropertyValue(
            XDocument.Load(Path.Combine(root, "src", "CareNest.App", "CareNest.App.csproj")),
            "ApplicationVersion");

        foreach (var relativePath in DynamicReleaseDocuments)
        {
            var text = Read(root, relativePath);
            Assert.Contains(version, text, StringComparison.Ordinal);
            Assert.Contains(applicationBuild, text, StringComparison.Ordinal);
            Assert.Contains("NOT PUBLISHED", text, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void Active_version_specific_release_package_exists_and_remains_fail_closed()
    {
        var root = FindRepositoryRoot();
        var version = PropertyValue(
            XDocument.Load(Path.Combine(root, "Directory.Build.props")),
            "Version");
        var normalizedVersion = version.Replace('.', '_', StringComparison.Ordinal);

        var preparation = Read(root, $"docs/releases/VERSION_{normalizedVersion}_PREPARATION.md");
        var notes = Read(root, $"docs/releases/RELEASE_NOTES_{normalizedVersion}_DRAFT.md");
        var checklist = Read(root, $"docs/releases/RELEASE_CHECKLIST_{normalizedVersion}.md");

        Assert.Contains(version, preparation, StringComparison.Ordinal);
        Assert.Contains(version, notes, StringComparison.Ordinal);
        Assert.Contains(version, checklist, StringComparison.Ordinal);

        Assert.Contains("NOT PUBLISHED", preparation, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("NOT PUBLISHED", notes, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("NOT RELEASED", checklist, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Active_handoff_requires_fresh_exact_head_verification_before_promotion()
    {
        var root = FindRepositoryRoot();
        var handoff = Read(root, "what_changed.md");

        Assert.Contains(
            "Fresh exact-head verification has not yet been observed",
            handoff,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains(
            "Historical workflow success is never transferred to a newer source",
            handoff,
            StringComparison.OrdinalIgnoreCase);
    }

    private static readonly string[] DynamicReleaseDocuments =
    [
        "PROJECT_STATUS.md",
        "docs/releases/NEXT_STEPS.md",
        "what_changed.md",
    ];

    private static string PropertyValue(XDocument document, string name) =>
        document.Descendants(name).Single().Value.Trim();

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

        throw new DirectoryNotFoundException("Could not locate the CareNest repository root for active release-line contract tests.");
    }
}
