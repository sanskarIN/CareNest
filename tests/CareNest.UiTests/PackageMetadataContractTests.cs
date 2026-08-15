using System.Xml.Linq;

namespace CareNest.UiTests;

public sealed class PackageMetadataContractTests
{
    private static readonly XNamespace AndroidNs = "http://schemas.android.com/apk/res/android";
    private static readonly XNamespace WindowsNs = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";

    [Fact]
    public void MauiProject_DeclaresStableIdentityVersionAndSupportedTargets()
    {
        var path = RepositoryLocator.PathOf("src", "CareNest.App", "CareNest.App.csproj");
        var project = XDocument.Load(path);

        Assert.Equal("CareNest", Property(project, "ApplicationTitle"));
        Assert.Equal("com.sanskar.carenest", Property(project, "ApplicationId"));
        Assert.Matches("^\\d+\\.\\d+\\.\\d+([-.][0-9A-Za-z.-]+)?$", Property(project, "ApplicationDisplayVersion"));
        Assert.True(int.TryParse(Property(project, "ApplicationVersion"), out var build) && build > 0);

        var targets = Property(project, "TargetFrameworks");
        Assert.Contains("net10.0-android", targets, StringComparison.Ordinal);
        Assert.Contains("net10.0-ios", targets, StringComparison.Ordinal);
        Assert.Contains("net10.0-maccatalyst", targets, StringComparison.Ordinal);
        Assert.Contains("net10.0-windows10.0.19041.0", targets, StringComparison.Ordinal);

        Assert.Equal("24.0", ConditionalProperty(project, "android", "SupportedOSPlatformVersion"));
        Assert.Equal("15.0", ConditionalProperty(project, "ios", "SupportedOSPlatformVersion"));
        Assert.Equal("15.0", ConditionalProperty(project, "maccatalyst", "SupportedOSPlatformVersion"));
        Assert.Equal("10.0.19041.0", ConditionalProperty(project, "windows", "SupportedOSPlatformVersion"));
    }

    [Fact]
    public void AndroidManifest_PreservesLocalFirstAndReminderSafetyDeclarations()
    {
        var manifest = XDocument.Load(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Platforms", "Android", "AndroidManifest.xml"));

        var permissions = manifest.Root!
            .Elements("uses-permission")
            .Select(element => (string?)element.Attribute(AndroidNs + "name"))
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToHashSet(StringComparer.Ordinal);

        Assert.Contains("android.permission.POST_NOTIFICATIONS", permissions);
        Assert.Contains("android.permission.RECEIVE_BOOT_COMPLETED", permissions);
        Assert.Contains("android.permission.SCHEDULE_EXACT_ALARM", permissions);
        Assert.Contains("android.permission.CAMERA", permissions);
        Assert.DoesNotContain("android.permission.INTERNET", permissions);

        var application = manifest.Root.Element("application");
        Assert.NotNull(application);
        Assert.Equal("false", (string?)application.Attribute(AndroidNs + "allowBackup"));
        Assert.Equal("false", (string?)application.Attribute(AndroidNs + "fullBackupContent"));
        Assert.Equal("false", (string?)application.Attribute(AndroidNs + "usesCleartextTraffic"));
        Assert.Equal("CareNest", (string?)application.Attribute(AndroidNs + "label"));
    }

    [Fact]
    public void ApplePlists_HavePurposeStringsAndNoArbitraryTransportOptOut()
    {
        var ios = XDocument.Load(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Platforms", "iOS", "Info.plist"));
        var mac = XDocument.Load(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Platforms", "MacCatalyst", "Info.plist"));

        Assert.False(string.IsNullOrWhiteSpace(PlistString(ios, "NSCameraUsageDescription")));
        Assert.False(string.IsNullOrWhiteSpace(PlistString(ios, "NSPhotoLibraryUsageDescription")));
        Assert.False(string.IsNullOrWhiteSpace(PlistString(mac, "NSCameraUsageDescription")));

        Assert.DoesNotContain("NSAllowsArbitraryLoads", ios.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("NSAllowsArbitraryLoads", mac.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void WindowsManifest_MatchesProductIdentityAndMinimumPlatform()
    {
        var manifest = XDocument.Load(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Platforms", "Windows", "Package.appxmanifest"));

        var identity = manifest.Root!.Element(WindowsNs + "Identity");
        Assert.NotNull(identity);
        Assert.Equal("com.sanskar.carenest", (string?)identity.Attribute("Name"));
        Assert.False(string.IsNullOrWhiteSpace((string?)identity.Attribute("Publisher")));

        var properties = manifest.Root.Element(WindowsNs + "Properties");
        Assert.NotNull(properties);
        Assert.Equal("CareNest", properties.Element(WindowsNs + "DisplayName")?.Value);
        Assert.False(string.IsNullOrWhiteSpace(properties.Element(WindowsNs + "PublisherDisplayName")?.Value));

        var family = manifest.Root
            .Element(WindowsNs + "Dependencies")?
            .Element(WindowsNs + "TargetDeviceFamily");
        Assert.NotNull(family);
        Assert.Equal("10.0.19041.0", (string?)family.Attribute("MinVersion"));
    }

    [Fact]
    public void MauiBrandAssets_ArePresent()
    {
        Assert.True(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "AppIcon", "appicon.svg")));
        Assert.True(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "AppIcon", "appiconfg.svg")));
        Assert.True(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "Splash", "splash.svg")));
        Assert.True(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "Images", "carenest_mark.svg")));
        Assert.True(File.Exists(RepositoryLocator.PathOf(
            "src", "CareNest.App", "Resources", "Images", "carenest_support.svg")));
    }

    private static string Property(XDocument project, string name) =>
        project.Descendants(name).First().Value.Trim();

    private static string ConditionalProperty(XDocument project, string platform, string name)
    {
        var group = project.Root!
            .Elements("PropertyGroup")
            .Single(element => ((string?)element.Attribute("Condition"))?.Contains(
                $"== '{platform}'", StringComparison.Ordinal) == true);

        return group.Element(name)!.Value.Trim();
    }

    private static string? PlistString(XDocument plist, string key)
    {
        var dict = plist.Root?.Element("dict");
        if (dict is null)
        {
            return null;
        }

        var elements = dict.Elements().ToList();
        for (var index = 0; index < elements.Count - 1; index++)
        {
            if (elements[index].Name.LocalName == "key" &&
                string.Equals(elements[index].Value, key, StringComparison.Ordinal))
            {
                return elements[index + 1].Name.LocalName == "string"
                    ? elements[index + 1].Value
                    : null;
            }
        }

        return null;
    }
}
