#if ANDROID
using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text.Json;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using CareNest.Application.Contracts;

namespace CareNest.App.Services;

public partial class PlatformNotificationService
{
    internal const string ChannelId = "carenest_reminders";
    internal const string ReminderAction = "com.sanskar.carenest.REMINDER";
    private const string ScheduledIdsKey = "notifications.android.scheduled-ids";

    private partial async Task<bool> RequestPermissionCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureChannel();

        if (Build.VERSION.SdkInt < BuildVersionCodes.Tiramisu)
        {
            return NotificationManagerCompat
                .From(Android.App.Application.Context)
                .AreNotificationsEnabled();
        }

        var status = await Permissions.RequestAsync<Permissions.PostNotifications>();
        return status == PermissionStatus.Granted;
    }

    private partial Task<NotificationDiagnostics> GetDiagnosticsCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureChannel();

        var context = Android.App.Application.Context;
        var manager = (AlarmManager?)context.GetSystemService(Context.AlarmService);
        var notificationGranted = NotificationManagerCompat
            .From(context)
            .AreNotificationsEnabled();

        var exactAvailable = Build.VERSION.SdkInt < BuildVersionCodes.S ||
            manager?.CanScheduleExactAlarms() == true;

        var power = (PowerManager?)context.GetSystemService(Context.PowerService);
        var batteryExempt = power?.IsIgnoringBatteryOptimizations(context.PackageName) ?? false;

        var warnings = new List<string>();
        if (!notificationGranted)
        {
            warnings.Add("Notification permission is disabled.");
        }
        if (!exactAvailable)
        {
            warnings.Add("Exact alarms are unavailable; CareNest will use the operating system's inexact alarm fallback.");
        }
        if (!batteryExempt)
        {
            warnings.Add("Battery optimization can delay reminders on some Android devices.");
        }

        return Task.FromResult(new NotificationDiagnostics(
            notificationGranted,
            manager is not null,
            exactAvailable,
            batteryExempt,
            $"Android API {(int)Build.VERSION.SdkInt}",
            warnings));
    }

    private partial Task ScheduleCoreAsync(
        NotificationRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureChannel();

        var context = Android.App.Application.Context;
        var manager = (AlarmManager?)context.GetSystemService(Context.AlarmService)
            ?? throw new InvalidOperationException("Android alarm manager is unavailable.");

        var due = new DateTimeOffset(
            DateTime.SpecifyKind(request.ScheduledUtc, DateTimeKind.Utc));

        if (due <= DateTimeOffset.UtcNow)
        {
            return Task.CompletedTask;
        }

        var pending = CreateReminderPendingIntent(context, request);
        var triggerMs = due.ToUnixTimeMilliseconds();

        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.S ||
                manager.CanScheduleExactAlarms())
            {
                manager.SetExactAndAllowWhileIdle(
                    AlarmType.RtcWakeup,
                    triggerMs,
                    pending);
            }
            else
            {
                manager.SetAndAllowWhileIdle(
                    AlarmType.RtcWakeup,
                    triggerMs,
                    pending);
            }
        }
        else
        {
            manager.SetExact(
                AlarmType.RtcWakeup,
                triggerMs,
                pending);
        }

        AddScheduledId(request.OccurrenceId);
        return Task.CompletedTask;
    }

    private partial Task CancelCoreAsync(
        string occurrenceId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var context = Android.App.Application.Context;
        var manager = (AlarmManager?)context.GetSystemService(Context.AlarmService);
        var pending = CreateCancellationPendingIntent(context, occurrenceId);

        if (pending is not null)
        {
            manager?.Cancel(pending);
            pending.Cancel();
        }

        NotificationManagerCompat
            .From(context)
            .Cancel(ToRequestCode(occurrenceId));

        RemoveScheduledId(occurrenceId);
        return Task.CompletedTask;
    }

    private partial async Task CancelAllCoreAsync(
        CancellationToken cancellationToken)
    {
        foreach (var id in GetScheduledIds())
        {
            cancellationToken.ThrowIfCancellationRequested();
            await CancelCoreAsync(id, cancellationToken);
        }

        NotificationManagerCompat
            .From(Android.App.Application.Context)
            .CancelAll();
    }

    private partial Task ShowTestCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureChannel();

        ShowNotification(
            "test",
            "CareNest test reminder",
            "Notifications are available on this device.",
            persistent: false,
            playSound: true,
            vibrate: true);

        return Task.CompletedTask;
    }

    internal static void ShowNotification(
        string occurrenceId,
        string title,
        string body,
        bool persistent,
        bool playSound,
        bool vibrate)
    {
        EnsureChannel();

        var context = Android.App.Application.Context;
        var launchIntent = context.PackageManager?
            .GetLaunchIntentForPackage(context.PackageName);

        PendingIntent? launchPending = null;
        if (launchIntent is not null)
        {
            launchIntent.AddFlags(ActivityFlags.SingleTop);
            launchPending = PendingIntent.GetActivity(
                context,
                ToRequestCode(occurrenceId),
                launchIntent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
        }

        var builder = new NotificationCompat.Builder(context, ChannelId)
            .SetSmallIcon(Resource.Mipmap.appicon)
            .SetContentTitle(title)
            .SetContentText(body)
            .SetPriority(NotificationCompat.PriorityHigh)
            .SetAutoCancel(!persistent)
            .SetOngoing(persistent)
            .SetCategory(NotificationCompat.CategoryReminder)
            .SetSilent(!playSound);

        if (!vibrate)
        {
            builder.SetVibrate(new long[] { 0L });
        }

        if (launchPending is not null)
        {
            builder.SetContentIntent(launchPending);
        }

        NotificationManagerCompat
            .From(context)
            .Notify(ToRequestCode(occurrenceId), builder.Build());
    }

    private static PendingIntent CreateReminderPendingIntent(
        Context context,
        NotificationRequest request)
    {
        var intent = new Intent(context, typeof(CareNestReminderReceiver));
        intent.SetAction(ReminderAction);
        intent.PutExtra("occurrenceId", request.OccurrenceId);
        intent.PutExtra("title", request.Title);
        intent.PutExtra("body", request.Body);
        intent.PutExtra("persistent", request.Persistent);
        intent.PutExtra("playSound", request.PlaySound);
        intent.PutExtra("vibrate", request.Vibrate);

        return PendingIntent.GetBroadcast(
            context,
            ToRequestCode(request.OccurrenceId),
            intent,
            PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
    }

    private static PendingIntent? CreateCancellationPendingIntent(
        Context context,
        string occurrenceId)
    {
        var intent = new Intent(context, typeof(CareNestReminderReceiver));
        intent.SetAction(ReminderAction);

        return PendingIntent.GetBroadcast(
            context,
            ToRequestCode(occurrenceId),
            intent,
            PendingIntentFlags.NoCreate | PendingIntentFlags.Immutable);
    }

    private static void EnsureChannel()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        {
            return;
        }

        var context = Android.App.Application.Context;
        var manager = (NotificationManager?)context.GetSystemService(Context.NotificationService);
        if (manager is null)
        {
            return;
        }

        var channel = new NotificationChannel(
            ChannelId,
            "CareNest reminders",
            NotificationImportance.High)
        {
            Description = "User-created CareNest reminders"
        };

        manager.CreateNotificationChannel(channel);
    }

    private static int ToRequestCode(string value)
    {
        var digest = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(value));
        return BinaryPrimitives.ReadInt32LittleEndian(digest.AsSpan(0, 4)) & int.MaxValue;
    }

    private static HashSet<string> GetScheduledIds()
    {
        var json = Preferences.Default.Get(ScheduledIdsKey, "[]");
        try
        {
            return JsonSerializer.Deserialize<HashSet<string>>(json)
                ?? new HashSet<string>(StringComparer.Ordinal);
        }
        catch (JsonException)
        {
            return new HashSet<string>(StringComparer.Ordinal);
        }
    }

    private static void AddScheduledId(string id)
    {
        var ids = GetScheduledIds();
        ids.Add(id);
        Preferences.Default.Set(
            ScheduledIdsKey,
            JsonSerializer.Serialize(ids));
    }

    private static void RemoveScheduledId(string id)
    {
        var ids = GetScheduledIds();
        ids.Remove(id);
        Preferences.Default.Set(
            ScheduledIdsKey,
            JsonSerializer.Serialize(ids));
    }
}

[BroadcastReceiver(Enabled = true, Exported = false)]
[IntentFilter(new[] { PlatformNotificationService.ReminderAction })]
public sealed class CareNestReminderReceiver : BroadcastReceiver
{
    public override void OnReceive(Context? context, Intent? intent)
    {
        if (intent?.Action != PlatformNotificationService.ReminderAction)
        {
            return;
        }

        var id = intent.GetStringExtra("occurrenceId") ?? "reminder";
        var title = intent.GetStringExtra("title") ?? "CareNest reminder";
        var body = intent.GetStringExtra("body") ??
            "Open CareNest to review your reminder.";
        var persistent = intent.GetBooleanExtra("persistent", false);
        var playSound = intent.GetBooleanExtra("playSound", true);
        var vibrate = intent.GetBooleanExtra("vibrate", true);

        PlatformNotificationService.ShowNotification(
            id,
            title,
            body,
            persistent,
            playSound,
            vibrate);
    }
}

[BroadcastReceiver(Enabled = true, Exported = true)]
[IntentFilter(new[]
{
    Intent.ActionBootCompleted,
    Intent.ActionTimeChanged,
    Intent.ActionTimezoneChanged
})]
public sealed class CareNestSystemEventReceiver : BroadcastReceiver
{
    public override void OnReceive(Context? context, Intent? intent)
    {
        var services = Microsoft.Maui.IPlatformApplication.Current?.Services;
        var coordinator = services?.GetService<IReminderCoordinator>();
        var appointments = services?.GetService<IAppointmentService>();
        var backups = services?.GetService<CareNest.Application.Services.BackupReminderCoordinator>();
        if (coordinator is null)
        {
            return;
        }

        _ = Task.Run(async () =>
        {
            await coordinator.RebuildAsync(cancellationToken: CancellationToken.None);
            if (appointments is not null)
            {
                await appointments.RebuildRemindersAsync(CancellationToken.None);
            }
            if (backups is not null)
            {
                await backups.SyncAsync(requestPermission: false, cancellationToken: CancellationToken.None);
            }
        });
    }
}
#endif
