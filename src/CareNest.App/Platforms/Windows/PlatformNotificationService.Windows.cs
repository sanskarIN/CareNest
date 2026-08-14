#if WINDOWS
using System.Collections.Concurrent;
using CareNest.Application.Contracts;
using Microsoft.Windows.AppNotifications;

namespace CareNest.App.Services;

public partial class PlatformNotificationService
{
    private static readonly ConcurrentDictionary<string, CancellationTokenSource>
        Scheduled = new(StringComparer.Ordinal);

    private partial Task<bool> RequestPermissionCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureRegistered();
        return Task.FromResult(true);
    }

    private partial Task<NotificationDiagnostics> GetDiagnosticsCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warnings = new[]
        {
            "This unpackaged Windows build uses an in-process scheduling fallback. Future reminders are rebuilt whenever CareNest starts, but Windows cannot deliver this fallback while the app is not running."
        };

        return Task.FromResult(new NotificationDiagnostics(
            true,
            true,
            false,
            true,
            "Windows App SDK in-process scheduling fallback",
            warnings));
    }

    private partial Task ScheduleCoreAsync(
        NotificationRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureRegistered();

        _ = CancelCoreAsync(request.OccurrenceId, cancellationToken);

        var due = DateTime.SpecifyKind(
            request.ScheduledUtc,
            DateTimeKind.Utc);

        var delay = due - DateTime.UtcNow;
        if (delay <= TimeSpan.Zero)
        {
            return Task.CompletedTask;
        }

        // The timer lifetime belongs to the scheduled reminder, not to the short-lived
        // caller operation. Cancellation after registration is controlled by CancelCoreAsync.
        var cts = new CancellationTokenSource();
        var timerToken = cts.Token;
        Scheduled[request.OccurrenceId] = cts;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(delay, timerToken);
                Show(
                    request.Title,
                    request.Body,
                    request.PlaySound);
            }
            catch (OperationCanceledException) when (timerToken.IsCancellationRequested)
            {
            }
            catch
            {
                // Notification display failures must not become unobserved background-task faults.
            }
            finally
            {
                RemoveOnlyIfCurrent(request.OccurrenceId, cts);
                cts.Dispose();
            }
        }, CancellationToken.None);

        return Task.CompletedTask;
    }

    private partial Task CancelCoreAsync(
        string occurrenceId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (Scheduled.TryRemove(occurrenceId, out var cts))
        {
            // The background timer owns disposal so immediate cancellation cannot race
            // a not-yet-started task reading the token.
            cts.Cancel();
        }

        return Task.CompletedTask;
    }

    private partial Task CancelAllCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var pair in Scheduled.ToArray())
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (Scheduled.TryRemove(pair.Key, out var cts))
            {
                cts.Cancel();
            }
        }

        return Task.CompletedTask;
    }

    private partial Task ShowTestCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Show(
            "CareNest test reminder",
            "Notifications are available while CareNest is running on this Windows build.",
            true);
        return Task.CompletedTask;
    }

    private static void RemoveOnlyIfCurrent(
        string occurrenceId,
        CancellationTokenSource owner)
    {
        ICollection<KeyValuePair<string, CancellationTokenSource>> entries = Scheduled;
        _ = entries.Remove(new KeyValuePair<string, CancellationTokenSource>(occurrenceId, owner));
    }

    private static void EnsureRegistered()
    {
        try
        {
            AppNotificationManager.Default.Register();
        }
        catch
        {
            // Register may already have occurred for this process.
        }
    }

    private static void Show(
        string title,
        string body,
        bool playSound)
    {
        EnsureRegistered();

        var safeTitle = EscapeXml(title);
        var safeBody = EscapeXml(body);

        var audio = playSound ? string.Empty : "<audio silent=\"true\"/>";
        var xml =
            "<toast><visual><binding template=\"ToastGeneric\">" +
            $"<text>{safeTitle}</text><text>{safeBody}</text>" +
            $"</binding></visual>{audio}</toast>";

        AppNotificationManager.Default.Show(
            new AppNotification(xml));
    }

    private static string EscapeXml(string value) =>
        System.Security.SecurityElement.Escape(value) ?? string.Empty;
}
#endif