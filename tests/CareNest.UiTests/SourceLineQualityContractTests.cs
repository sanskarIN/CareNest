using System.Text.Json;
using System.Xml.Linq;

namespace CareNest.UiTests;

public sealed class SourceLineQualityContractTests
{
    private static readonly string[] ForbiddenRuntimeTokens =
    [
        "TODO",
        "FIXME",
        "HACK",
        "NotImplementedException",
        ".GetAwaiter().GetResult()",
        "Thread.Sleep(",
        "Task.WaitAll(",
        "Task.WaitAny(",
        "throw ex;"
    ];

    [Fact]
    public void RuntimeCSharp_EveryLineAvoidsKnownDefectPatterns()
    {
        var sourceRoot = Path.Combine(RepositoryLocator.Root, "src");
        var files = Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(files);

        var violations = new List<string>();

        foreach (var path in files)
        {
            var relativePath = Path.GetRelativePath(RepositoryLocator.Root, path).Replace('\\', '/');
            var lines = File.ReadAllLines(path);

            for (var index = 0; index < lines.Length; index++)
            {
                var line = lines[index];
                var trimmed = line.TrimStart();
                var lineNumber = index + 1;

                if (trimmed.StartsWith("<<<<<<< ", StringComparison.Ordinal) ||
                    string.Equals(trimmed, "=======", StringComparison.Ordinal) ||
                    trimmed.StartsWith(">>>>>>> ", StringComparison.Ordinal))
                {
                    violations.Add($"{relativePath}:{lineNumber}: unresolved merge-conflict marker");
                }

                foreach (var token in ForbiddenRuntimeTokens)
                {
                    if (line.Contains(token, StringComparison.Ordinal))
                    {
                        violations.Add($"{relativePath}:{lineNumber}: prohibited token '{token}'");
                    }
                }
            }
        }

        Assert.True(
            violations.Count == 0,
            "Runtime source line audit failed:\n" + string.Join(Environment.NewLine, violations));
    }

    [Fact]
    public void RuntimeCSharp_DoesNotUseResultPropertyForSyncOverAsync()
    {
        var sourceRoot = Path.Combine(RepositoryLocator.Root, "src");
        var violations = new List<string>();

        foreach (var path in Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories).Order(StringComparer.Ordinal))
        {
            var relativePath = Path.GetRelativePath(RepositoryLocator.Root, path).Replace('\\', '/');
            var lines = File.ReadAllLines(path);

            for (var index = 0; index < lines.Length; index++)
            {
                var line = lines[index];
                if (line.Contains(".Result;", StringComparison.Ordinal) ||
                    line.Contains(".Result)", StringComparison.Ordinal))
                {
                    violations.Add($"{relativePath}:{index + 1}: synchronous Task.Result access");
                }
            }
        }

        Assert.True(
            violations.Count == 0,
            "Sync-over-async line audit failed:\n" + string.Join(Environment.NewLine, violations));
    }

    [Fact]
    public void RuntimeStructuredFiles_AreSyntacticallyWellFormed()
    {
        var sourceRoot = Path.Combine(RepositoryLocator.Root, "src");
        var xmlExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".xaml",
            ".csproj",
            ".props",
            ".targets",
            ".xml",
            ".plist",
            ".resx"
        };

        var failures = new List<string>();
        var files = Directory.EnumerateFiles(sourceRoot, "*", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal)
            .ToArray();

        foreach (var path in files)
        {
            var extension = Path.GetExtension(path);
            if (!xmlExtensions.Contains(extension) && !extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var relativePath = Path.GetRelativePath(RepositoryLocator.Root, path).Replace('\\', '/');

            try
            {
                if (extension.Equals(".json", StringComparison.OrdinalIgnoreCase))
                {
                    using var _ = JsonDocument.Parse(File.ReadAllText(path));
                }
                else
                {
                    _ = XDocument.Load(path, LoadOptions.SetLineInfo);
                }
            }
            catch (Exception exception) when (exception is InvalidDataException or JsonException or System.Xml.XmlException)
            {
                failures.Add($"{relativePath}: {exception.Message}");
            }
        }

        Assert.True(
            failures.Count == 0,
            "Structured runtime file validation failed:\n" + string.Join(Environment.NewLine, failures));
    }
}
