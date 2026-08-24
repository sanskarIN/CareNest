using System.Xml.Linq;

namespace CareNest.UiTests;

public sealed class VersionConsistencyContractTests
{
    private const string ReleaseVersion = "2.18.12";
    private const string AssemblyVersion = "2.18.12.0";
    private const string ApplicationBuild = "21812";

    [Fact]
    public void Central_assembly_metadata_matches_release_target()
    {
        var root = FindRepositoryRoot();
        var document = XDocument.Load(Path.Combine(root, "Directory.Build.props"));

        Assert.Equal(ReleaseVersion, PropertyValue(document, "Version"));
        Assert.Equal(AssemblyVersion, PropertyValue(document, "AssemblyVersion"));
        Assert.Equal(AssemblyVersion, PropertyValue(document, "FileVersion"));
        Assert.Equal(ReleaseVersion, PropertyValue(document, "InformationalVersion"));
    }

    [Fact]
    public void Maui_package_metadata_matches_release_target()
    {
        var root = FindRepositoryRoot();
        var project = XDocument.Load(Path.Combine(root, "src", "CareNest.App", "CareNest.App.csproj"));

        Assert.Equal(ReleaseVersion, PropertyValue(project, "ApplicationDisplayVersion"));
        Assert.Equal(ApplicationBuild, PropertyValue(project, "ApplicationVersion"));
    }

    [Fact]
    public void Release_preparation_documents_match_target_without_claiming_publication()
    {
        var root = FindRepositoryRoot();
        var preparation = Read(root, "docs/releases/VERSION_2_18_12_PREPARATION.md");
        var notes = Read(root, "docs/releases/RELEASE_NOTES_2_18_12_DRAFT.md");
        var checklist = Read(root, "docs/releases/RELEASE_CHECKLIST_2_18_12.md");

        foreach (var text in new[] { preparation, notes, checklist })
        {
            Assert.Contains(ReleaseVersion, text, StringComparison.Ordinal);
            Assert.Contains(ApplicationBuild, text, StringComparison.Ordinal);
        }

        Assert.Contains("NOT PUBLISHED", preparation, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("NOT PUBLISHED", notes, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("NOT RELEASED", checklist, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("v2.18.12", checklist, StringComparison.Ordinal);
    }

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

        throw new DirectoryNotFoundException("Could not locate the CareNest repository root for version consistency tests.");
    }
}
