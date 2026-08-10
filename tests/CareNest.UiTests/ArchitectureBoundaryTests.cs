using System.Xml.Linq;

namespace CareNest.UiTests;

public sealed class ArchitectureBoundaryTests
{
    [Fact]
    public void Shared_HasNoProjectDependencies()
    {
        var refs = ProjectReferences("CareNest.Shared");
        Assert.Empty(refs);
    }

    [Fact]
    public void Domain_DependsOnlyOnShared()
    {
        var refs = ProjectReferences("CareNest.Domain");
        Assert.Equal(new[] { "CareNest.Shared" }, refs.Order(StringComparer.Ordinal).ToArray());
    }

    [Fact]
    public void Application_DependsOnlyOnDomainAndShared()
    {
        var refs = ProjectReferences("CareNest.Application");
        Assert.Equal(
            new[] { "CareNest.Domain", "CareNest.Shared" },
            refs.Order(StringComparer.Ordinal).ToArray());
    }

    [Fact]
    public void Infrastructure_DependsOnlyOnApplicationDomainAndShared()
    {
        var refs = ProjectReferences("CareNest.Infrastructure");
        Assert.Equal(
            new[] { "CareNest.Application", "CareNest.Domain", "CareNest.Shared" },
            refs.Order(StringComparer.Ordinal).ToArray());
    }

    [Fact]
    public void PlatformNeutralProjects_DoNotReferenceMaui()
    {
        foreach (var project in new[]
                 {
                     "CareNest.Shared",
                     "CareNest.Domain",
                     "CareNest.Application",
                     "CareNest.Infrastructure"
                 })
        {
            var source = RepositoryLocator.Read("src", project, $"{project}.csproj");
            Assert.DoesNotContain("Microsoft.Maui", source, StringComparison.Ordinal);
            Assert.DoesNotContain("<UseMaui>true</UseMaui>", source, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void MauiApp_IsTheOnlyRuntimeCompositionRoot()
    {
        var project = RepositoryLocator.Read("src", "CareNest.App", "CareNest.App.csproj");

        Assert.Contains("<UseMaui>true</UseMaui>", project, StringComparison.Ordinal);
        Assert.Contains("CareNest.Application", project, StringComparison.Ordinal);
        Assert.Contains("CareNest.Domain", project, StringComparison.Ordinal);
        Assert.Contains("CareNest.Infrastructure", project, StringComparison.Ordinal);
        Assert.Contains("CareNest.Shared", project, StringComparison.Ordinal);
    }

    private static string[] ProjectReferences(string projectName)
    {
        var path = Path.Combine(RepositoryLocator.Root, "src", projectName, $"{projectName}.csproj");
        var document = XDocument.Load(path);

        return document
            .Descendants("ProjectReference")
            .Select(node => (string?)node.Attribute("Include"))
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => Path.GetFileNameWithoutExtension(value!))
            .ToArray();
    }
}
