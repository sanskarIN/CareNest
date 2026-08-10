namespace CareNest.UiTests;

public sealed class RepositoryPolicyTests
{
    [Fact]
    public void RuntimeSource_HasNoImplementationPlaceholders()
    {
        var files = EnumerateRuntimeTextFiles("*.cs", "*.xaml");
        Assert.NotEmpty(files);

        foreach (var path in files)
        {
            var text = File.ReadAllText(path);
            Assert.DoesNotContain("TODO", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("FIXME", text, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("NotImplementedException", text, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void RuntimeSource_DoesNotIntroduceNetworkOrTelemetryClients()
    {
        var files = EnumerateRuntimeTextFiles("*.cs", "*.csproj");
        var prohibited = new[]
        {
            "System.Net.Http",
            "HttpClient",
            "Grpc.Net.Client",
            "Firebase.Analytics",
            "Microsoft.AppCenter.Analytics",
            "SentrySdk",
            "TelemetryClient"
        };

        foreach (var path in files)
        {
            var text = File.ReadAllText(path);
            foreach (var token in prohibited)
            {
                Assert.DoesNotContain(token, text, StringComparison.Ordinal);
            }
        }
    }

    [Fact]
    public void RuntimeSource_DoesNotContainClinicalDecisionFeatureNames()
    {
        var files = EnumerateRuntimeTextFiles("*.cs", "*.xaml");
        var prohibited = new[]
        {
            "CalculateDose",
            "CalculateDosage",
            "DoseCalculator",
            "DrugInteractionChecker",
            "MedicationInteractionChecker",
            "TreatmentRecommendation",
            "ClinicalRiskScore",
            "SymptomDiagnosis"
        };

        foreach (var path in files)
        {
            var text = File.ReadAllText(path);
            foreach (var token in prohibited)
            {
                Assert.DoesNotContain(token, text, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    [Fact]
    public void Repository_DoesNotContainCommonPrivateKeyOrSecretFiles()
    {
        var prohibitedExtensions = new[] { ".p12", ".pfx", ".jks", ".keystore" };
        var prohibitedNames = new[] { "google-services.json", "GoogleService-Info.plist", ".env" };

        foreach (var path in Directory.EnumerateFiles(RepositoryLocator.Root, "*", SearchOption.AllDirectories))
        {
            var fileName = Path.GetFileName(path);
            var extension = Path.GetExtension(path);

            Assert.DoesNotContain(prohibitedNames, name => string.Equals(name, fileName, StringComparison.OrdinalIgnoreCase));
            Assert.DoesNotContain(prohibitedExtensions, item => string.Equals(item, extension, StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public void RequiredGovernanceAndReleaseFiles_ArePresent()
    {
        var required = new[]
        {
            "README.md",
            "LICENSE",
            "NOTICE",
            "CONTRIBUTING.md",
            "CODE_OF_CONDUCT.md",
            "SECURITY.md",
            "SUPPORT.md",
            "PRIVACY.md",
            "TERMS.md",
            "CHANGELOG.md",
            "PROJECT_STATUS.md",
            "DECISIONS.md",
            ".editorconfig",
            ".gitignore",
            "Directory.Build.props",
            "Directory.Packages.props",
            Path.Combine("docs", "security", "THREAT_MODEL.md"),
            Path.Combine("docs", "releases", "RELEASE_CHECKLIST.md"),
            Path.Combine("docs", "testing", "TEST_PLAN.md")
        };

        foreach (var relativePath in required)
        {
            Assert.True(
                File.Exists(Path.Combine(RepositoryLocator.Root, relativePath)),
                $"Required repository file is missing: {relativePath}");
        }
    }

    private static string[] EnumerateRuntimeTextFiles(params string[] patterns)
    {
        var src = Path.Combine(RepositoryLocator.Root, "src");
        return patterns
            .SelectMany(pattern => Directory.EnumerateFiles(src, pattern, SearchOption.AllDirectories))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}
