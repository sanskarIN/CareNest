namespace CareNest.UiTests;

public sealed class FundingLinkContractTests
{
    [Fact]
    public void Funding_link_is_consistent_across_runtime_support_surfaces()
    {
        var root = FindRepositoryRoot();
        var constants = File.ReadAllText(Path.Combine(root, "src", "CareNest.Shared", "AppConstants.cs"));
        var viewModel = File.ReadAllText(Path.Combine(root, "src", "CareNest.App", "ViewModels", "AboutViewModel.cs"));
        var aboutPage = File.ReadAllText(Path.Combine(root, "src", "CareNest.App", "Views", "AboutPage.xaml"));

        Assert.Contains("https://buymeacoffee.com/sanskarIN", constants, StringComparison.Ordinal);
        Assert.Contains("AppConstants.FundingUrl", viewModel, StringComparison.Ordinal);
        Assert.Contains("SupportProjectCommand", viewModel, StringComparison.Ordinal);
        Assert.Contains("Command=\"{Binding SupportProjectCommand}\"", aboutPage, StringComparison.Ordinal);
        Assert.Contains("Support CareNest on Buy Me a Coffee", aboutPage, StringComparison.Ordinal);
    }

    [Fact]
    public void Funding_link_is_documented_as_voluntary_project_support()
    {
        var root = FindRepositoryRoot();
        var readme = File.ReadAllText(Path.Combine(root, "README.md"));
        var support = File.ReadAllText(Path.Combine(root, "SUPPORT.md"));
        var funding = File.ReadAllText(Path.Combine(root, ".github", "FUNDING.yml"));

        Assert.Contains("https://buymeacoffee.com/sanskarIN", readme, StringComparison.Ordinal);
        Assert.Contains("voluntarily support", readme, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("https://buymeacoffee.com/sanskarIN", support, StringComparison.Ordinal);
        Assert.Contains("Financial support is optional", support, StringComparison.Ordinal);
        Assert.Contains("https://buymeacoffee.com/sanskarIN", funding, StringComparison.Ordinal);
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
