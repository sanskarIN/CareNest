namespace CareNest.UiTests;

public sealed class RepositoryPolicyTests
{
    private static readonly string[] NetworkAndTelemetryTokens =
    [
        "System.Net.Http",
        "HttpClient",
        "Grpc.Net.Client",
        "Firebase.Analytics",
        "Microsoft.AppCenter.Analytics",
        "SentrySdk",
        "TelemetryClient"
    ];

    private static readonly string[] ClinicalDecisionTokens =
    [
        "CalculateDose",
        "CalculateDosage",
        "DoseCalculator",
        "DrugInteractionChecker",
        "MedicationInteractionChecker",
        "TreatmentRecommendation",
        "ClinicalRiskScore",
        "SymptomDiagnosis"
    ];

    private static readonly string[] ProhibitedSecretExtensions = [".p12", ".pfx", ".jks", ".keystore"];
    private static readonly string[] ProhibitedSecretNames = ["google-services.json", "GoogleService-Info.plist", ".env"];

    private static readonly string[] RequiredRepositoryFiles =
    [
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
        Path.Combine(".github", "workflows", "ci.yml"),
        Path.Combine(".github", "workflows", "codeql.yml"),
        Path.Combine(".github", "workflows", "dependency-review.yml"),
        Path.Combine(".github", "workflows", "release-gate.yml"),
        Path.Combine(".github", "workflows", "release-evidence.yml"),
        Path.Combine("docs", "security", "THREAT_MODEL.md"),
        Path.Combine("docs", "security", "LOGGING_PRIVACY.md"),
        Path.Combine("docs", "security", "DEPENDENCY_RISK_REGISTER.md"),
        Path.Combine("docs", "releases", "RELEASE_CHECKLIST.md"),
        Path.Combine("docs", "releases", "QUALITY_GATE.md"),
        Path.Combine("docs", "releases", "SECURITY_RELEASE_REVIEW.md"),
        Path.Combine("docs", "testing", "TEST_PLAN.md")
    ];

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

        foreach (var path in files)
        {
            var text = File.ReadAllText(path);
            foreach (var token in NetworkAndTelemetryTokens)
            {
                Assert.DoesNotContain(token, text, StringComparison.Ordinal);
            }
        }
    }

    [Fact]
    public void RuntimeSource_DoesNotContainClinicalDecisionFeatureNames()
    {
        var files = EnumerateRuntimeTextFiles("*.cs", "*.xaml");

        foreach (var path in files)
        {
            var text = File.ReadAllText(path);
            foreach (var token in ClinicalDecisionTokens)
            {
                Assert.DoesNotContain(token, text, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    [Fact]
    public void Repository_DoesNotContainCommonPrivateKeyOrSecretFiles()
    {
        foreach (var path in Directory.EnumerateFiles(RepositoryLocator.Root, "*", SearchOption.AllDirectories)
                     .Where(IsCommittedWorkspaceFile))
        {
            var fileName = Path.GetFileName(path);
            var extension = Path.GetExtension(path);

            Assert.False(
                ProhibitedSecretNames.Any(name => string.Equals(name, fileName, StringComparison.OrdinalIgnoreCase)),
                $"Prohibited secret/config file found: {Path.GetRelativePath(RepositoryLocator.Root, path)}");
            Assert.False(
                ProhibitedSecretExtensions.Any(item => string.Equals(item, extension, StringComparison.OrdinalIgnoreCase)),
                $"Prohibited signing/secret file found: {Path.GetRelativePath(RepositoryLocator.Root, path)}");
        }
    }

    [Fact]
    public void RequiredGovernanceAndReleaseFiles_ArePresent()
    {
        foreach (var relativePath in RequiredRepositoryFiles)
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
            .Where(IsCommittedWorkspaceFile)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static bool IsCommittedWorkspaceFile(string path)
    {
        var relative = Path.GetRelativePath(RepositoryLocator.Root, path);
        var segments = relative.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        return !segments.Any(segment =>
            string.Equals(segment, "bin", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(segment, "obj", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(segment, ".git", StringComparison.OrdinalIgnoreCase));
    }
}
