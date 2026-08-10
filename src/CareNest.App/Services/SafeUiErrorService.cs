using Microsoft.Extensions.Logging;

namespace CareNest.App.Services;

public sealed class SafeUiErrorService(ILogger<SafeUiErrorService> logger)
{
    public async Task ShowAsync(string safeMessage, Exception? exception = null)
    {
        if (exception is not null)
        {
            var exceptionType = exception.GetType().FullName ?? "Unknown";
            logger.LogError(
                "CareNest operation failed. ExceptionType={ExceptionType}. Sensitive exception details are not logged.",
                exceptionType);
        }

        var windows = Microsoft.Maui.Controls.Application.Current?.Windows;
        var page = windows is { Count: > 0 } ? windows[0].Page : null;
        if (page is null)
        {
            return;
        }

        await MainThread.InvokeOnMainThreadAsync(
            () => page.DisplayAlertAsync("CareNest", safeMessage, "OK"));
    }
}
