namespace CareNest.UiTests;

public sealed class GlobalExceptionPrivacyContractTests
{
    [Fact]
    public void GlobalExceptionHandler_LogsOnlyExceptionTypeMetadata()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "GlobalExceptionHandler.cs");

        Assert.Contains("GetType().FullName", source, StringComparison.Ordinal);
        Assert.DoesNotContain(".Message", source, StringComparison.Ordinal);
        Assert.DoesNotContain(".StackTrace", source, StringComparison.Ordinal);
        Assert.DoesNotContain("logger.LogCritical(exception", source, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("logger.LogError(exception", source, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void App_AttachesGlobalExceptionHandlerOnceDuringConstruction()
    {
        var app = RepositoryLocator.Read("src", "CareNest.App", "App.xaml.cs");
        var handler = RepositoryLocator.Read("src", "CareNest.App", "Services", "GlobalExceptionHandler.cs");

        Assert.Contains("GlobalExceptionHandler globalExceptions", app, StringComparison.Ordinal);
        Assert.Contains("globalExceptions.Attach();", app, StringComparison.Ordinal);
        Assert.Contains("Interlocked.Exchange", handler, StringComparison.Ordinal);
    }

    [Fact]
    public void GlobalExceptionHandler_ObservesUnhandledAndUnobservedTaskExceptions()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "GlobalExceptionHandler.cs");

        Assert.Contains("AppDomain.CurrentDomain.UnhandledException", source, StringComparison.Ordinal);
        Assert.Contains("TaskScheduler.UnobservedTaskException", source, StringComparison.Ordinal);
        Assert.Contains("args.SetObserved();", source, StringComparison.Ordinal);
    }
}
