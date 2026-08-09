using Microsoft.Extensions.Logging;

namespace CareNest.App.Services;

public sealed class SafeUiErrorService(ILogger<SafeUiErrorService> logger)
{
    public async Task ShowAsync(string safeMessage, Exception? exception = null)
    {
        if (exception is not null)
        {
            logger.LogError(
                exception,
                "CareNest operation failed. Sensitive user fields are intentionally excluded from this log message.");
        }

        var page = Microsoft.Maui.Controls.Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page is null)
        {
            return;
        }

        await MainThread.InvokeOnMainThreadAsync(
            () => page.DisplayAlertAsync("CareNest", safeMessage, "OK"));
    }
}
