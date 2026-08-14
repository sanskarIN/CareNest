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
    private static readonly long[] SilentVibrationPattern = [0L];

    private partial async Task<bool> RequestPermissionCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureChannel();

        if (Build.VERSION.SdkInt < BuildVersionCodes.Tiramisu)
        {
            var context = GetApplicationContext();
            return GetNotificationManager(context).AreNotificationsEnabled();
        }

        var status = await Permissions.RequestAsync<Permissions.PostNotifications>();
        return status == PermissionStatus.Granted;
    }

    private partial Task<NotificationDiagnostics> GetDiagnosticsCoreAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        EnsureChannel();

        var context = GetApplicationContext();
        var manager = (AlarmManager?)context.GetSystemService(Context.AlarmService);
        var notificationGranted = GetNotificationManager(context).AreNotificationsEnabled();

        var exactAvailable = !OperatingSystem.IsAndroidVersionAtLeast(31) ||
            manager?.CanScheduleExactAlarms() == true;

        var power = (PowerManager?)context.GetSystemService(Context.PowerService);
        var packageName = GetPackageName(context);
        var batteryExempt = power?.IsIgnoringBatteryOptimizations(packageName) ?? false;

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

        var context = GetApplicationContext();
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
            if (!OperatingSystem.IsAndroidVersionAtLeast(31) ||
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

        var context = GetApplicationContext();
        var manager = (AlarmManager?)context.GetSystemService(Context.AlarmService);
        var pending = CreateCancellationPendingIntent(context, occurrenceId);

        if (pending is not null)
        {
            manager?.Cancel(pending);
            pending.Cancel();
        }

        GetNotificationManager(context).Cancel(ToRequestCode(occurrenceId));

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

        GetNotificationManager(GetApplicationContext()).CancelAll();
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

        var context = GetApplicationContext();
        var packageName = GetPackageName(context);
        var launchIntent = context.PackageManager?
            .GetLaunchIntentForPackage(packageName);

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

        var builder = new NotificationCompat.Builder(context, ChannelId);
        builder.SetSmallIcon(Resource.Mipmap.appicon);
        builder.SetContentTitle(title);
        builder.SetContentText(body);
        builder.SetPriority(NotificationCompat.PriorityHigh);
        builder.SetAutoCancel(!persistent);
        builder.SetOngoing(persistent);
        builder.SetCategory(NotificationCompat.CategoryReminder);
        builder.SetSilent(!playSound);

        if (!vibrate)
        {
            builder.SetVibrate(SilentVibrationPattern);
        }

        if (launchPending is not null)
        {
            builder.SetContentIntent(launchPending);
        }

        var notification = builder.Build()
            ?? throw new InvalidOperationException("Android notification construction failed.");
        GetNotificationManager(context).Notify(ToRequestCode(occurrenceId), notification);
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
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable)
            ?? throw new InvalidOperationException("Android reminder pending intent could not be created.");
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
        if (!OperatingSystem.IsAndroidVersionAtLeast(26))
        {
            return;
        }

        var context = GetApplicationContext();
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

    private static Context GetApplicationContext() =>
        Android.App.Application.Context
        ?? throw new InvalidOperationException("Android application context is unavailable.");

    private static NotificationManagerCompat GetNotificationManager(Context context) =>
        NotificationManagerCompat.From(context)
        ?? throw new InvalidOperationException("Android notification manager is unavailable.");

    private static string GetPackageName(Context context) =>
        !string.IsNullOrWhiteSpace(context.PackageName)
            ? context.PackageName
            : throw new InvalidOperationException("Android package name is unavailable.");

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

        var pendingResult = GoAsync();
        _ = Task.Run(async () =>
        {
            try
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
            }
            catch
            {
                // A later foreground/startup recovery pass can retry rebuilding platform requests.
            }
            finally
            {
                pendingResult?.Finish();
            }
        });
    }
}
#endif