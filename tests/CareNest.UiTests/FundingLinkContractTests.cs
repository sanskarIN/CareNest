namespace CareNest.UiTests;

public sealed class FundingLinkContractTests
{
    [Fact]
    public void Funding_link_exists_only_in_repository_support_materials()
    {
        var root = FindRepositoryRoot();
        var readme = File.ReadAllText(Path.Combine(root, "README.md"));
        var support = File.ReadAllText(Path.Combine(root, "SUPPORT.md"));
        var funding = File.ReadAllText(Path.Combine(root, ".github", "FUNDING.yml"));
        var aboutViewModel = File.ReadAllText(Path.Combine(root, "src", "CareNest.App", "ViewModels", "AboutViewModel.cs"));
        var aboutPage = File.ReadAllText(Path.Combine(root, "src", "CareNest.App", "Views", "AboutPage.xaml"));

        Assert.Contains("https://buymeacoffee.com/sanskarIN", readme, StringComparison.Ordinal);
        Assert.Contains("voluntarily support", readme, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("https://buymeacoffee.com/sanskarIN", support, StringComparison.Ordinal);
        Assert.Contains("Financial support is optional", support, StringComparison.Ordinal);
        Assert.Contains("https://buymeacoffee.com/sanskarIN", funding, StringComparison.Ordinal);

        Assert.DoesNotContain("buymeacoffee.com", aboutViewModel, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SupportProjectCommand", aboutViewModel, StringComparison.Ordinal);
        Assert.DoesNotContain("FundingLinkPolicy", aboutViewModel, StringComparison.Ordinal);
        Assert.DoesNotContain("buymeacoffee.com", aboutPage, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Buy Me a Coffee", aboutPage, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SupportProjectCommand", aboutPage, StringComparison.Ordinal);
    }

    [Fact]
    public void Funding_link_is_documented_as_optional_and_not_a_health_entitlement()
    {
        var root = FindRepositoryRoot();
        var readme = File.ReadAllText(Path.Combine(root, "README.md"));
        var support = File.ReadAllText(Path.Combine(root, "SUPPORT.md"));

        Assert.Contains("https://buymeacoffee.com/sanskarIN", readme, StringComparison.Ordinal);
        Assert.Contains("voluntarily support", readme, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Financial support is optional", support, StringComparison.Ordinal);
        Assert.Contains("does not change", support, StringComparison.OrdinalIgnoreCase);
    }

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

        throw new DirectoryNotFoundException("Could not locate the CareNest repository root for UI contract tests.");
    }
}
