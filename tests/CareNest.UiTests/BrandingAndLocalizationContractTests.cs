using System.Xml.Linq;

namespace CareNest.UiTests;

public sealed class BrandingAndLocalizationContractTests
{
    private static readonly string[] RequiredResourceKeys =
    [
        "ProductName",
        "OnboardingWelcome",
        "LocalFirstDescription",
        "BackupResponsibility",
        "MedicalDisclaimer",
        "ReminderLimitations",
        "MadeBy"
    ];

    [Fact]
    public void MauiProject_DeclaresAdaptiveIconSplashAndImageResources()
    {
        var project = RepositoryLocator.Read("src", "CareNest.App", "CareNest.App.csproj");

        Assert.Contains("<MauiIcon", project, StringComparison.Ordinal);
        Assert.Contains("ForegroundFile=\"Resources\\AppIcon\\appiconfg.svg\"", project, StringComparison.Ordinal);
        Assert.Contains("<MauiSplashScreen", project, StringComparison.Ordinal);
        Assert.Contains("<MauiImage Include=\"Resources\\Images\\*\"", project, StringComparison.Ordinal);
    }

    [Fact]
    public void RequiredBrandingAssets_ArePresentAndWellFormedSvgWithoutFundingArtwork()
    {
        var relativePaths = new[]
        {
            Path.Combine("src", "CareNest.App", "Resources", "AppIcon", "appicon.svg"),
            Path.Combine("src", "CareNest.App", "Resources", "AppIcon", "appiconfg.svg"),
            Path.Combine("src", "CareNest.App", "Resources", "Splash", "splash.svg"),
            Path.Combine("src", "CareNest.App", "Resources", "Images", "carenest_monochrome.svg"),
            Path.Combine("src", "CareNest.App", "Resources", "Images", "carenest_mark_light.svg"),
            Path.Combine("src", "CareNest.App", "Resources", "Images", "carenest_mark_dark.svg")
        };

        foreach (var relativePath in relativePaths)
        {
            var fullPath = Path.Combine(RepositoryLocator.Root, relativePath);
            Assert.True(File.Exists(fullPath), $"Required branding asset is missing: {relativePath}");
            var exception = Record.Exception(() => XDocument.Load(fullPath));
            Assert.True(exception is null, $"Branding asset is not valid XML/SVG: {relativePath}: {exception}");
        }

        Assert.False(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "Images", "carenest_support.svg")));
        Assert.False(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "Images", "buy_me_a_coffee_carenest.svg")));
    }

    [Fact]
    public void EnglishResources_ContainRequiredSafetyAndBrandingKeys()
    {
        var path = Path.Combine(
            RepositoryLocator.Root,
            "src",
            "CareNest.App",
            "Resources",
            "Strings",
            "AppResources.resx");
        var document = XDocument.Load(path);
        var names = document
            .Descendants("data")
            .Select(node => (string?)node.Attribute("name"))
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var required in RequiredResourceKeys)
        {
            Assert.Contains(required, names);
        }
    }

    [Fact]
    public void RepositoryFundingPages_UseOnlyTextLinksOutsideAppRuntime()
    {
        var expected = "https://buymeacoffee.com/sanskarIN";
        var pages = new[]
        {
            RepositoryLocator.Read("BUY_ME_A_COFFEE.md"),
            RepositoryLocator.Read("docs", "SUPPORT_CARENEST.md")
        };

        foreach (var page in pages)
        {
            Assert.Contains(expected, page, StringComparison.Ordinal);
            Assert.Contains("application package does not include or expose this external funding destination", page, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("buy_me_a_coffee_carenest.svg", page, StringComparison.Ordinal);
            Assert.DoesNotContain("carenest_support.svg", page, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void AboutPage_PreservesProductSupportWithoutFundingBadge()
    {
        var about = RepositoryLocator.Read("src", "CareNest.App", "Views", "AboutPage.xaml");

        Assert.Contains("Open source and support", about, StringComparison.Ordinal);
        Assert.Contains("Command=\"{Binding OpenRepositoryCommand}\"", about, StringComparison.Ordinal);
        Assert.Contains("Command=\"{Binding SupportEmailCommand}\"", about, StringComparison.Ordinal);
        Assert.DoesNotContain("carenest_support.svg", about, StringComparison.Ordinal);
        Assert.DoesNotContain("buy_me_a_coffee_carenest.svg", about, StringComparison.Ordinal);
        Assert.DoesNotContain("SupportProjectCommand", about, StringComparison.Ordinal);
        Assert.DoesNotContain("Buy Me a Coffee", about, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("buymeacoffee.com", about, StringComparison.OrdinalIgnoreCase);
    }
}
