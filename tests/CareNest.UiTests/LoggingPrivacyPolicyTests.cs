using System.Text.RegularExpressions;

namespace CareNest.UiTests;

public sealed partial class LoggingPrivacyPolicyTests
{
    [Fact]
    public void RuntimeLogCalls_DoNotPassExceptionObjectsToStructuredLogger()
    {
        var sourceRoot = Path.Combine(RepositoryLocator.Root, "src");
        var violations = new List<string>();

        foreach (var path in Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories))
        {
            var source = File.ReadAllText(path);
            if (ExceptionLoggingPattern().IsMatch(source))
            {
                violations.Add(Path.GetRelativePath(RepositoryLocator.Root, path));
            }
        }

        Assert.True(
            violations.Count == 0,
            $"Exception objects can expose sensitive messages/paths through logs. Violations: {string.Join(", ", violations)}");
    }

    [GeneratedRegex(@"\b(?:logger|_logger)\.Log(?:Trace|Debug|Information|Warning|Error|Critical)\s*\(\s*(?:ex|exception)\s*,", RegexOptions.CultureInvariant)]
    private static partial Regex ExceptionLoggingPattern();
}
