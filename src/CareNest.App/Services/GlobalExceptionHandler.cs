using Microsoft.Extensions.Logging;

namespace CareNest.App.Services;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
{
    private int _attached;

    public void Attach()
    {
        if (Interlocked.Exchange(ref _attached, 1) != 0)
        {
            return;
        }

        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        var exceptionType = args.ExceptionObject is Exception exception
            ? exception.GetType().FullName
            : args.ExceptionObject?.GetType().FullName;

        logger.LogCritical(
            "An unhandled CareNest exception occurred. Type={ExceptionType}; Terminating={IsTerminating}.",
            exceptionType ?? "Unknown",
            args.IsTerminating);
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs args)
    {
        var exceptionType = args.Exception.GetType().FullName ?? "Unknown";

        logger.LogError(
            "An unobserved CareNest task exception occurred. Type={ExceptionType}.",
            exceptionType);

        args.SetObserved();
    }
}
