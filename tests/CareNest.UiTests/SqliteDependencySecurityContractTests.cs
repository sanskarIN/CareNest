using System.Xml.Linq;

namespace CareNest.UiTests;

public sealed class SqliteDependencySecurityContractTests
{
    [Fact]
    public void NativeSqlitePins_StayBeyondKnownVulnerableRelease()
    {
        var document = XDocument.Parse(RepositoryLocator.Read("Directory.Packages.props"));
        var versions = document
            .Descendants("PackageVersion")
            .Where(element => element.Attribute("Include") is not null)
            .ToDictionary(
                element => element.Attribute("Include")!.Value,
                element => element.Attribute("Version")?.Value ?? string.Empty,
                StringComparer.OrdinalIgnoreCase);

        AssertVersionAtLeast(versions, "SQLitePCLRaw.lib.e_sqlite3", new Version(3, 53, 3));
        AssertVersionAtLeast(versions, "SQLitePCLRaw.lib.e_sqlite3.android", new Version(2, 1, 12));
        AssertVersionAtLeast(versions, "SQLitePCLRaw.provider.e_sqlite3", new Version(2, 1, 12));
        AssertVersionAtLeast(versions, "SQLitePCLRaw.provider.sqlite3", new Version(2, 1, 12));
        AssertVersionAtLeast(versions, "SQLitePCLRaw.provider.dynamic_cdecl", new Version(2, 1, 12));
    }

    [Fact]
    public void ResolvedSqliteAdvisory_IsNotSuppressedFromNugetAudit()
    {
        var buildProps = RepositoryLocator.Read("Directory.Build.props");

        Assert.DoesNotContain("GHSA-2m69-gcr7-jv3q", buildProps, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("NuGetAuditSuppress", buildProps, StringComparison.OrdinalIgnoreCase);
    }

    private static void AssertVersionAtLeast(
        IReadOnlyDictionary<string, string> versions,
        string packageId,
        Version minimum)
    {
        Assert.True(versions.TryGetValue(packageId, out var versionText));
        Assert.True(Version.TryParse(versionText, out var version));
        Assert.True(version >= minimum, $"{packageId} must be at least {minimum}; found {versionText}.");
    }
}
