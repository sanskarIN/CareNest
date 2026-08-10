namespace CareNest.UiTests;

public sealed class AsyncSafetyContractTests
{
    [Fact]
    public void RuntimeSource_DoesNotSynchronouslyBlockOnTasks()
    {
        var sourceRoot = Path.Combine(RepositoryLocator.Root, "src");
        var files = Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories).ToArray();
        Assert.NotEmpty(files);

        var prohibited = new[]
        {
            ".GetAwaiter().GetResult()",
            ".Wait()",
            "Thread.Sleep(",
            "Task.WaitAll(",
            "Task.WaitAny("
        };

        foreach (var path in files)
        {
            var source = File.ReadAllText(path);
            foreach (var token in prohibited)
            {
                Assert.DoesNotContain(token, source, StringComparison.Ordinal);
            }
        }
    }

    [Fact]
    public void RuntimeSource_DoesNotUseTaskResultPropertyForBlocking()
    {
        var sourceRoot = Path.Combine(RepositoryLocator.Root, "src");
        foreach (var path in Directory.EnumerateFiles(sourceRoot, "*.cs", SearchOption.AllDirectories))
        {
            var source = File.ReadAllText(path);
            Assert.DoesNotContain(".Result;", source, StringComparison.Ordinal);
            Assert.DoesNotContain(".Result)", source, StringComparison.Ordinal);
        }
    }
}
