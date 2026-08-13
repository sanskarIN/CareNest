using CareNest.Application.Services;
using CareNest.Shared;
using CareNest.UnitTests.TestDoubles;

namespace CareNest.UnitTests;

public sealed class BackupReminderCoordinatorTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 9, 30, 0, TimeSpan.Zero);

    [Fact]
    public async Task SyncAsync_Disabled_CancelsExistingReminderAndStops()
    {
        var repository = new SettingsRepository();
        var notifications = new NotificationServiceSpy();
        var coordinator = new BackupReminderCoordinator(repository, notifications, new FixedTimeProvider(Now));

        await coordinator.SyncAsync(requestPermission: true);

        Assert.Contains(BackupReminderCoordinator.NotificationId, notifications.CancelledOccurrenceIds);
        Assert.Empty(notifications.Scheduled);
        Assert.Equal(0, notifications.PermissionRequestCount);
    }

    [Fact]
    public async Task SyncAsync_DeniedPermissionWithoutPrompt_DoesNotSchedule()
    {
        var repository = EnabledRepository();
        var notifications = new NotificationServiceSpy
        {
            Diagnostics = new(false, true, true, true, "test", Array.Empty<string>())
        };
        var coordinator = new BackupReminderCoordinator(repository, notifications, new FixedTimeProvider(Now));

        await coordinator.SyncAsync(requestPermission: false);

        Assert.Equal(0, notifications.PermissionRequestCount);
        Assert.Empty(notifications.Scheduled);
    }

    [Fact]
    public async Task SyncAsync_PermissionPromptStillDenied_DoesNotSchedule()
    {
        var repository = EnabledRepository();
        var notifications = new NotificationServiceSpy
        {
            Diagnostics = new(false, true, true, true, "test", Array.Empty<string>()),
            PermissionRequestResult = false
        };
        var coordinator = new BackupReminderCoordinator(repository, notifications, new FixedTimeProvider(Now));

        await coordinator.SyncAsync(requestPermission: true);

        Assert.Equal(1, notifications.PermissionRequestCount);
        Assert.Empty(notifications.Scheduled);
    }

    [Fact]
    public async Task SyncAsync_NoPreviousBackup_SchedulesFromCurrentTime()
    {
        var repository = EnabledRepository();
        var notifications = new NotificationServiceSpy();
        var coordinator = new BackupReminderCoordinator(repository, notifications, new FixedTimeProvider(Now));

        await coordinator.SyncAsync(requestPermission: false);

        var request = Assert.Single(notifications.Scheduled);
        Assert.Equal(BackupReminderCoordinator.NotificationId, request.OccurrenceId);
        Assert.Equal(Now.UtcDateTime.AddDays(AppConstants.BackupReminderDays), request.ScheduledUtc);
        Assert.Equal("backup", request.Category);
        Assert.Contains("manual encrypted CareNest backup", request.Body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task SyncAsync_OverdueBackup_SchedulesNearFutureInsteadOfPast()
    {
        var repository = EnabledRepository();
        repository.Settings[SettingKeys.LastBackupUtc] = Now.AddDays(-AppConstants.BackupReminderDays - 5).UtcDateTime.ToString("O");
        var notifications = new NotificationServiceSpy();
        var coordinator = new BackupReminderCoordinator(repository, notifications, new FixedTimeProvider(Now));

        await coordinator.SyncAsync(requestPermission: false);

        var request = Assert.Single(notifications.Scheduled);
        Assert.Equal(Now.UtcDateTime.AddMinutes(1), request.ScheduledUtc);
    }

    [Fact]
    public async Task SyncAsync_RespectsUserSoundAndVibrationPreferences()
    {
        var repository = EnabledRepository();
        repository.Settings[SettingKeys.SoundEnabled] = "0";
        repository.Settings[SettingKeys.VibrationEnabled] = "0";
        var notifications = new NotificationServiceSpy();
        var coordinator = new BackupReminderCoordinator(repository, notifications, new FixedTimeProvider(Now));

        await coordinator.SyncAsync(requestPermission: false);

        var request = Assert.Single(notifications.Scheduled);
        Assert.False(request.PlaySound);
        Assert.False(request.Vibrate);
    }

    private static SettingsRepository EnabledRepository()
    {
        var repository = new SettingsRepository();
        repository.Settings[SettingKeys.BackupReminderEnabled] = "1";
        return repository;
    }

    private sealed class SettingsRepository : RepositoryStub
    {
        public Dictionary<string, string> Settings { get; } = new(StringComparer.Ordinal);

        public override Task<string?> GetSettingAsync(string key, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(Settings.TryGetValue(key, out var value) ? value : null);
        }
    }
}
