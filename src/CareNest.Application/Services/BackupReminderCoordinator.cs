using CareNest.Application.Contracts;
using CareNest.Shared;

namespace CareNest.Application.Services;

public sealed class BackupReminderCoordinator(
    ICareNestRepository repository,
    INotificationService notifications,
    TimeProvider timeProvider)
{
    public const string NotificationId = "backup-reminder";

    public async Task SyncAsync(
        bool requestPermission,
        CancellationToken cancellationToken = default)
    {
        await notifications.CancelAsync(NotificationId, cancellationToken);

        var enabled = string.Equals(
            await repository.GetSettingAsync(SettingKeys.BackupReminderEnabled, cancellationToken),
            "1",
            StringComparison.Ordinal);
        if (!enabled)
        {
            return;
        }

        var diagnostics = await notifications.GetDiagnosticsAsync(cancellationToken);
        if (!diagnostics.PermissionGranted && requestPermission)
        {
            _ = await notifications.RequestPermissionAsync(cancellationToken);
            diagnostics = await notifications.GetDiagnosticsAsync(cancellationToken);
        }

        if (!diagnostics.PermissionGranted)
        {
            return;
        }

        var now = timeProvider.GetUtcNow().UtcDateTime;
        var lastText = await repository.GetSettingAsync(SettingKeys.LastBackupUtc, cancellationToken);
        var last = DateTime.TryParse(
            lastText,
            null,
            System.Globalization.DateTimeStyles.RoundtripKind,
            out var parsed)
            ? parsed.ToUniversalTime()
            : now;

        var due = last.AddDays(AppConstants.BackupReminderDays);
        if (due <= now)
        {
            due = now.AddMinutes(1);
        }

        var playSound = !string.Equals(
            await repository.GetSettingAsync(SettingKeys.SoundEnabled, cancellationToken),
            "0",
            StringComparison.Ordinal);
        var vibrate = !string.Equals(
            await repository.GetSettingAsync(SettingKeys.VibrationEnabled, cancellationToken),
            "0",
            StringComparison.Ordinal);

        await notifications.ScheduleAsync(
            new NotificationRequest(
                NotificationId,
                due,
                "CareNest backup reminder",
                "Consider creating a manual encrypted CareNest backup.",
                false,
                "backup",
                playSound,
                vibrate),
            cancellationToken);
    }
}
