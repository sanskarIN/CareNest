using System.Text.RegularExpressions;

namespace CareNest.UiTests;

public sealed partial class LoggingPrivacyPolicyTests
{
    [Fact]
    public void RuntimeLogCalls_DoNotPassExceptionObjectsToStructuredLogger()
    {
        var sourceRoot = Path.Combine(RepositoryLocator.Root, "src");
        var violations = new List<string>();

        foreach (var path in Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories)
                     .Where(IsCommittedRuntimeFile))
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

    private static bool IsCommittedRuntimeFile(string path)
    {
        var relative = Path.GetRelativePath(RepositoryLocator.Root, path);
        var segments = relative.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        return !segments.Any(segment =>
            string.Equals(segment, "bin", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(segment, "obj", StringComparison.OrdinalIgnoreCase));
    }

    [GeneratedRegex(@"\b(?:logger|_logger)\.Log(?:Trace|Debug|Information|Warning|Error|Critical)\s*\(\s*(?:ex|exception)\s*,", RegexOptions.CultureInvariant)]
    private static partial Regex ExceptionLoggingPattern();
}
