#if MACCATALYST
using CareNest.Application.Contracts;
using Foundation;
using UserNotifications;

namespace CareNest.App.Services;

public partial class PlatformNotificationService
{
    private partial async Task<bool> RequestPermissionCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = await UNUserNotificationCenter.Current.RequestAuthorizationAsync(
            UNAuthorizationOptions.Alert |
            UNAuthorizationOptions.Sound |
            UNAuthorizationOptions.Badge);

        return result.Item1;
    }

    private partial async Task<NotificationDiagnostics> GetDiagnosticsCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var settings = await UNUserNotificationCenter.Current.GetNotificationSettingsAsync();

        var granted = settings.AuthorizationStatus is
            UNAuthorizationStatus.Authorized or
            UNAuthorizationStatus.Provisional or
            UNAuthorizationStatus.Ephemeral;

        var warnings = granted
            ? Array.Empty<string>()
            : new[] { "Notification permission is not currently authorized." };

        return new NotificationDiagnostics(
            granted,
            true,
            true,
            true,
            $"Mac Catalyst notification authorization: {settings.AuthorizationStatus}",
            warnings);
    }

    private partial async Task ScheduleCoreAsync(
        NotificationRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var due = new DateTimeOffset(
            DateTime.SpecifyKind(request.ScheduledUtc, DateTimeKind.Utc));

        var seconds = (due - DateTimeOffset.UtcNow).TotalSeconds;
        if (seconds <= 0)
        {
            return;
        }

        var content = new UNMutableNotificationContent
        {
            Title = request.Title,
            Body = request.Body,
            Sound = request.PlaySound ? UNNotificationSound.Default : null
        };

        var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(
            Math.Max(1, seconds),
            false);

        var nativeRequest = UNNotificationRequest.FromIdentifier(
            request.OccurrenceId,
            content,
            trigger);

        await UNUserNotificationCenter.Current.AddNotificationRequestAsync(
            nativeRequest);
    }

    private partial Task CancelCoreAsync(
        string occurrenceId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        UNUserNotificationCenter.Current.RemovePendingNotificationRequests(
            new[] { occurrenceId });

        UNUserNotificationCenter.Current.RemoveDeliveredNotifications(
            new[] { occurrenceId });

        return Task.CompletedTask;
    }

    private partial Task CancelAllCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        UNUserNotificationCenter.Current.RemoveAllPendingNotificationRequests();
        UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
        return Task.CompletedTask;
    }

    private partial async Task ShowTestCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var content = new UNMutableNotificationContent
        {
            Title = "CareNest test reminder",
            Body = "Notifications are available on this device.",
            Sound = UNNotificationSound.Default
        };

        var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
        var request = UNNotificationRequest.FromIdentifier(
            $"test-{Guid.NewGuid():N}",
            content,
            trigger);

        await UNUserNotificationCenter.Current.AddNotificationRequestAsync(
            request);
    }
}
#endif
