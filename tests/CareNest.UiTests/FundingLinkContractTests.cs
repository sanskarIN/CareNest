namespace CareNest.UiTests;

public sealed class FundingLinkContractTests
{
    private const string BuyMeACoffeeUrl = "https://buymeacoffee.com/sanskarIN";
    private const string GumroadUrl = "https://ramsandesh.gumroad.com";

    [Fact]
    public void External_commercial_links_exist_in_repository_support_materials()
    {
        var root = FindRepositoryRoot();
        var readme = File.ReadAllText(Path.Combine(root, "README.md"));
        var support = File.ReadAllText(Path.Combine(root, "SUPPORT.md"));
        var funding = File.ReadAllText(Path.Combine(root, ".github", "FUNDING.yml"));
        var gumroad = File.ReadAllText(Path.Combine(root, "GUMROAD.md"));

        Assert.Contains(BuyMeACoffeeUrl, readme, StringComparison.Ordinal);
        Assert.Contains(BuyMeACoffeeUrl, support, StringComparison.Ordinal);
        Assert.Contains(BuyMeACoffeeUrl, funding, StringComparison.Ordinal);

        Assert.Contains(GumroadUrl, readme, StringComparison.Ordinal);
        Assert.Contains(GumroadUrl, support, StringComparison.Ordinal);
        Assert.Contains(GumroadUrl, funding, StringComparison.Ordinal);
        Assert.Contains(GumroadUrl, gumroad, StringComparison.Ordinal);
        Assert.Contains("separate from CareNest health functionality", gumroad, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void External_commercial_links_do_not_leak_into_about_runtime_surface()
    {
        var root = FindRepositoryRoot();
        var aboutViewModel = File.ReadAllText(Path.Combine(root, "src", "CareNest.App", "ViewModels", "AboutViewModel.cs"));
        var aboutPage = File.ReadAllText(Path.Combine(root, "src", "CareNest.App", "Views", "AboutPage.xaml"));

        Assert.DoesNotContain("buymeacoffee.com", aboutViewModel, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("gumroad.com", aboutViewModel, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SupportProjectCommand", aboutViewModel, StringComparison.Ordinal);
        Assert.DoesNotContain("FundingLinkPolicy", aboutViewModel, StringComparison.Ordinal);

        Assert.DoesNotContain("buymeacoffee.com", aboutPage, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("gumroad.com", aboutPage, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Buy Me a Coffee", aboutPage, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Gumroad", aboutPage, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("SupportProjectCommand", aboutPage, StringComparison.Ordinal);
    }

    [Fact]
    public void Commercial_links_are_documented_as_optional_and_not_health_entitlements()
    {
        var root = FindRepositoryRoot();
        var readme = File.ReadAllText(Path.Combine(root, "README.md"));
        var support = File.ReadAllText(Path.Combine(root, "SUPPORT.md"));
        var gumroad = File.ReadAllText(Path.Combine(root, "GUMROAD.md"));

        Assert.Contains(BuyMeACoffeeUrl, readme, StringComparison.Ordinal);
        Assert.Contains(GumroadUrl, readme, StringComparison.Ordinal);
        Assert.Contains("Financial support is optional", support, StringComparison.Ordinal);
        Assert.Contains("does not unlock medical advice", support, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("does not unlock", gumroad, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("diagnosis", gumroad, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("dosage", gumroad, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("user health data", gumroad, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Gumroad_repository_badge_is_repository_only_and_accessible()
    {
        var root = FindRepositoryRoot();
        var badgePath = Path.Combine(root, "docs", "assets", "gumroad_store_badge.svg");

        Assert.True(File.Exists(badgePath));

        var badge = File.ReadAllText(badgePath);
        Assert.Contains(GumroadUrl, badge, StringComparison.Ordinal);
        Assert.Contains("<title", badge, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("<desc", badge, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("repository", badge, StringComparison.OrdinalIgnoreCase);

        Assert.False(File.Exists(Path.Combine(
            root,
            "src",
            "CareNest.App",
            "Resources",
            "Images",
            "gumroad_store_badge.svg")));
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
