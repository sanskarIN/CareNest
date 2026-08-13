using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Enums;
using CareNest.Shared;

namespace CareNest.App.ViewModels;

public sealed record RedactedScheduleItem(string OccurrenceId, DateTime DueUtc, ReminderState State)
{
    public string ShortId => OccurrenceId.Length <= 8 ? OccurrenceId : OccurrenceId[..8];
}

public sealed class SettingsViewModel : ObservableViewModel
{
    private static readonly string[] CacheDirectoryNames =
        ["Reports", "Backups", "Restore", "Exports", "ProfilePreviews"];

    private readonly ICareNestRepository _repository;
    private readonly AppStateService _state;
    private readonly INotificationService _notifications;
    private readonly IReminderCoordinator _reminders;
    private readonly IAppointmentService _appointments;
    private readonly BackupReminderCoordinator _backupReminder;
    private readonly IBackupService _backup;
    private readonly IDocumentStore _documents;
    private readonly ISecretStore _secretStore;
    private readonly IAppFileGateway _files;
    private readonly IAppLockService _lock;
    private readonly IAppNavigator _navigator;

    private ThemePreference _theme;
    private bool _reducedMotion;
    private bool _largeInterface;
    private bool _quietHoursEnabled;
    private TimeSpan _quietHoursStart = TimeSpan.FromHours(22);
    private TimeSpan _quietHoursEnd = TimeSpan.FromHours(7);
    private bool _genericLabels = true;
    private bool _persistentNotifications;
    private bool _soundEnabled = true;
    private bool _vibrationEnabled = true;
    private bool _backupReminderEnabled;
    private bool _appLockEnabled;
    private string _diagnostics = "Not checked yet.";
    private string _storageUsage = "Calculating…";
    private string _schemaVersion = "—";
    private string _timeZoneSimulation = TimeZoneInfo.Local.Id;
    private string _simulationResult = string.Empty;

    public SettingsViewModel(
        ICareNestRepository repository,
        AppStateService state,
        INotificationService notifications,
        IReminderCoordinator reminders,
        IAppointmentService appointments,
        BackupReminderCoordinator backupReminder,
        IBackupService backup,
        IDocumentStore documents,
        ISecretStore secretStore,
        IAppFileGateway files,
        IAppLockService appLock,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _repository = repository;
        _state = state;
        _notifications = notifications;
        _reminders = reminders;
        _appointments = appointments;
        _backupReminder = backupReminder;
        _backup = backup;
        _documents = documents;
        _secretStore = secretStore;
        _files = files;
        _lock = appLock;
        _navigator = navigator;

        SavePreferencesCommand = new AsyncCommand(SavePreferencesAsync);
        NotificationDiagnosticsCommand = new AsyncCommand(RefreshDiagnosticsAsync);
        TestReminderCommand = new AsyncCommand(TestReminderAsync);
        RebuildRemindersCommand = new AsyncCommand(RebuildRemindersAsync);
        ExportSanitizedDiagnosticsCommand = new AsyncCommand(ExportSanitizedDiagnosticsAsync);
        SimulateTimeZoneCommand = new AsyncCommand(SimulateTimeZoneAsync);
        VacuumCommand = new AsyncCommand(VacuumAsync);
        ClearCacheCommand = new AsyncCommand(ClearCacheAsync);
    }

    public IReadOnlyList<ThemePreference> ThemeOptions { get; } =
        Enum.GetValues<ThemePreference>();
    public ObservableCollection<RedactedScheduleItem> RedactedSchedule { get; } = [];
    public string CurrentTimeZone => TimeZoneInfo.Local.Id;

    public ThemePreference Theme { get => _theme; set => SetProperty(ref _theme, value); }
    public bool ReducedMotion { get => _reducedMotion; set => SetProperty(ref _reducedMotion, value); }
    public bool LargeInterface { get => _largeInterface; set => SetProperty(ref _largeInterface, value); }
    public bool QuietHoursEnabled { get => _quietHoursEnabled; set => SetProperty(ref _quietHoursEnabled, value); }
    public TimeSpan QuietHoursStart { get => _quietHoursStart; set => SetProperty(ref _quietHoursStart, value); }
    public TimeSpan QuietHoursEnd { get => _quietHoursEnd; set => SetProperty(ref _quietHoursEnd, value); }
    public bool GenericLabels { get => _genericLabels; set => SetProperty(ref _genericLabels, value); }
    public bool PersistentNotifications { get => _persistentNotifications; set => SetProperty(ref _persistentNotifications, value); }
    public bool SoundEnabled { get => _soundEnabled; set => SetProperty(ref _soundEnabled, value); }
    public bool VibrationEnabled { get => _vibrationEnabled; set => SetProperty(ref _vibrationEnabled, value); }
    public bool BackupReminderEnabled { get => _backupReminderEnabled; set => SetProperty(ref _backupReminderEnabled, value); }
    public bool AppLockEnabled { get => _appLockEnabled; private set => SetProperty(ref _appLockEnabled, value); }
    public string Diagnostics { get => _diagnostics; private set => SetProperty(ref _diagnostics, value); }
    public string StorageUsage { get => _storageUsage; private set => SetProperty(ref _storageUsage, value); }
    public string SchemaVersion { get => _schemaVersion; private set => SetProperty(ref _schemaVersion, value); }
    public string TimeZoneSimulation { get => _timeZoneSimulation; set => SetProperty(ref _timeZoneSimulation, value); }
    public string SimulationResult { get => _simulationResult; private set => SetProperty(ref _simulationResult, value); }

    public ICommand SavePreferencesCommand { get; }
    public ICommand NotificationDiagnosticsCommand { get; }
    public ICommand TestReminderCommand { get; }
    public ICommand RebuildRemindersCommand { get; }
    public ICommand ExportSanitizedDiagnosticsCommand { get; }
    public ICommand SimulateTimeZoneCommand { get; }
    public ICommand VacuumCommand { get; }
    public ICommand ClearCacheCommand { get; }

    public Task LoadAsync() =>
        RunAsync(async ct =>
        {
            Theme = await _state.GetThemeAsync(ct);
            ReducedMotion = await BoolAsync(SettingKeys.ReducedMotion, false, ct);
            LargeInterface = await BoolAsync(SettingKeys.LargeInterface, false, ct);
            QuietHoursEnabled = await BoolAsync(SettingKeys.QuietHoursEnabled, false, ct);
            GenericLabels = await BoolAsync(SettingKeys.GenericNotificationLabels, true, ct);
            PersistentNotifications = await BoolAsync(SettingKeys.PersistentNotifications, false, ct);
            SoundEnabled = await BoolAsync(SettingKeys.SoundEnabled, true, ct);
            VibrationEnabled = await BoolAsync(SettingKeys.VibrationEnabled, true, ct);
            BackupReminderEnabled = await BoolAsync(SettingKeys.BackupReminderEnabled, false, ct);
            QuietHoursStart = ParseTime(await _repository.GetSettingAsync(SettingKeys.QuietHoursStart, ct), new TimeSpan(22, 0, 0));
            QuietHoursEnd = ParseTime(await _repository.GetSettingAsync(SettingKeys.QuietHoursEnd, ct), new TimeSpan(7, 0, 0));
            AppLockEnabled = await _lock.IsEnabledAsync(ct);
            SchemaVersion = (await _repository.GetSchemaVersionAsync(ct)).ToString();
            StorageUsage = FormatBytes(await _documents.GetStorageUsageBytesAsync(ct));
            await LoadDiagnosticsCoreAsync(ct);
            await LoadRedactedScheduleAsync(ct);
        }, "CareNest could not load settings.");

    public Task EnableAppLockAsync(string pin) =>
        RunAsync(async ct =>
        {
            await _lock.SetPinAsync(pin, ct);
            AppLockEnabled = true;
            StatusMessage = "App lock enabled. Keep your PIN safe; CareNest cannot recover it.";
        }, "CareNest could not enable app lock. Use a numeric PIN of 6–32 digits.");

    public Task DisableAppLockAsync(string pin) =>
        RunAsync(async ct =>
        {
            if (!await _lock.VerifyPinAsync(pin, ct))
            {
                throw new InvalidOperationException("The app-lock PIN is incorrect.");
            }
            await _lock.DisableAsync(ct);
            AppLockEnabled = false;
            StatusMessage = "App lock disabled.";
        }, "CareNest could not disable app lock.");

    public Task CreateBackupAsync(string password) =>
        RunAsync(async ct =>
        {
            ValidateBackupPassword(password);
            var directory = Path.Combine(FileSystem.Current.CacheDirectory, "Backups");
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, $"CareNest-{DateTime.UtcNow:yyyyMMdd-HHmmss}{AppConstants.BackupExtension}");
            await using (var destination = File.Create(path))
            {
                await _backup.CreateEncryptedBackupAsync(destination, password, AppInfo.Current.VersionString, ct);
            }

            await _repository.SetSettingAsync(SettingKeys.LastBackupUtc, DateTime.UtcNow.ToString("O"), ct);
            await _backupReminder.SyncAsync(requestPermission: false, cancellationToken: ct);
            await _files.ShareFileAsync(path, "Save CareNest encrypted backup", ct);
            StatusMessage = "Encrypted backup created. The destination is user-chosen through the system share/save surface.";
        }, "CareNest could not create the encrypted backup.");

    public Task RestoreBackupAsync(string password) =>
        RunAsync(async ct =>
        {
            ValidateBackupPassword(password);
            var picked = await _files.PickBackupForRestoreAsync(ct);
            if (picked is null) return;

            var restoreDirectory = Path.Combine(FileSystem.Current.CacheDirectory, "Restore");
            Directory.CreateDirectory(restoreDirectory);
            var restorePath = Path.Combine(restoreDirectory, $"{Guid.NewGuid():N}{AppConstants.BackupExtension}");
            try
            {
                await using (var pickedStream = await picked.OpenReadAsync(ct))
                await using (var copy = File.Create(restorePath))
                {
                    await pickedStream.CopyToAsync(copy, ct);
                }

                BackupInspection inspection;
                await using (var inspectStream = File.OpenRead(restorePath))
                {
                    inspection = await _backup.InspectAsync(inspectStream, password, ct);
                }
                if (inspection.SchemaVersion > await _repository.GetSchemaVersionAsync(ct))
                {
                    throw new InvalidOperationException("This backup uses a newer database schema.");
                }

                await _notifications.CancelAllAsync(ct);
                await using var restoreStream = File.OpenRead(restorePath);
                await _backup.RestoreEncryptedBackupAsync(restoreStream, password, ct);
                await _reminders.RebuildAsync(cancellationToken: ct);
                await _appointments.RebuildRemindersAsync(ct);
                await _backupReminder.SyncAsync(requestPermission: false, cancellationToken: ct);
                await LoadAsyncAfterRestore(ct);
                StatusMessage = $"Backup from {inspection.CreatedUtc:u} restored after integrity validation.";
            }
            finally
            {
                if (File.Exists(restorePath))
                {
                    File.Delete(restorePath);
                }
            }
        }, "CareNest could not restore the backup. Check the password, file, and available storage.");

    public Task ResetAllDataAsync() =>
        RunAsync(async ct =>
        {
            await _notifications.CancelAllAsync(ct);
            var storedFiles = await _documents.ListStoredFilesAsync(ct);

            // Clear structured records first. If this fails, encrypted payloads and keys remain
            // available rather than leaving live database rows that point to deleted files.
            await _repository.ClearAllAsync(ct);

            // After the database is clear, remove encrypted payloads while their key is retained.
            // If file cleanup fails, a retry can still decrypt/remove any remaining orphan payload.
            foreach (var file in storedFiles)
            {
                await _documents.DeleteAsync(file, ct);
            }

            // Remove secure material only after document cleanup has succeeded.
            await _secretStore.RemoveAsync(SecretKeys.DocumentMasterKey, ct);
            await _lock.DisableAsync(ct);
            AppLockEnabled = false;
            await _navigator.ResetToOnboardingAsync(ct);
        }, "CareNest could not fully reset local data.");

    private Task SavePreferencesAsync() =>
        RunAsync(async ct =>
        {
            await _state.SetThemeAsync(Theme, ct);
            await _state.SetBoolAsync(SettingKeys.ReducedMotion, ReducedMotion, ct);
            await _state.SetLargeInterfaceAsync(LargeInterface, ct);
            await _state.SetBoolAsync(SettingKeys.QuietHoursEnabled, QuietHoursEnabled, ct);
            await _state.SetBoolAsync(SettingKeys.GenericNotificationLabels, GenericLabels, ct);
            await _state.SetBoolAsync(SettingKeys.PersistentNotifications, PersistentNotifications, ct);
            await _state.SetBoolAsync(SettingKeys.SoundEnabled, SoundEnabled, ct);
            await _state.SetBoolAsync(SettingKeys.VibrationEnabled, VibrationEnabled, ct);
            await _state.SetBoolAsync(SettingKeys.BackupReminderEnabled, BackupReminderEnabled, ct);
            await _repository.SetSettingAsync(SettingKeys.QuietHoursStart, TimeOnly.FromTimeSpan(QuietHoursStart).ToString("HH:mm"), ct);
            await _repository.SetSettingAsync(SettingKeys.QuietHoursEnd, TimeOnly.FromTimeSpan(QuietHoursEnd).ToString("HH:mm"), ct);
            await _reminders.RebuildAsync(cancellationToken: ct);
            await _appointments.RebuildRemindersAsync(ct);
            await _backupReminder.SyncAsync(requestPermission: BackupReminderEnabled, cancellationToken: ct);
            StatusMessage = "Settings saved. Quiet hours suppress reminder notifications without changing the recorded schedule time.";
        }, "CareNest could not save settings.");

    private Task RefreshDiagnosticsAsync() =>
        RunAsync(async ct =>
        {
            await LoadDiagnosticsCoreAsync(ct);
            await LoadRedactedScheduleAsync(ct);
            StatusMessage = "Notification diagnostics refreshed.";
        }, "CareNest could not read notification diagnostics.");

    private Task TestReminderAsync() =>
        RunAsync(async ct =>
        {
            if (!await _notifications.RequestPermissionAsync(ct))
            {
                throw new InvalidOperationException("Notification permission was not granted.");
            }
            await _notifications.ShowTestAsync(ct);
            await LoadDiagnosticsCoreAsync(ct);
            StatusMessage = "Test notification requested.";
        }, "CareNest could not show a test notification.");

    private Task RebuildRemindersAsync() =>
        RunAsync(async ct =>
        {
            await _reminders.MarkOverdueAsMissedAsync(ct);
            await _reminders.RebuildAsync(cancellationToken: ct);
            await _appointments.RebuildRemindersAsync(ct);
            StatusMessage = "Future reminder requests were rebuilt from user-entered schedules.";
        }, "CareNest could not rebuild reminder requests.");

    private Task ClearCacheAsync() =>
        RunAsync(ct =>
        {
            ct.ThrowIfCancellationRequested();
            foreach (var name in CacheDirectoryNames)
            {
                var path = Path.Combine(FileSystem.Current.CacheDirectory, name);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, recursive: true);
                }
            }
            StatusMessage = "Temporary CareNest export/preview cache cleared. Encrypted stored records and documents were not deleted.";
            return Task.CompletedTask;
        }, "CareNest could not clear all temporary cache files.");

    private Task VacuumAsync() =>
        RunAsync(async ct =>
        {
            await _repository.VacuumAsync(ct);
            StatusMessage = "Local database maintenance completed.";
        }, "CareNest could not compact the local database.");

    private Task SimulateTimeZoneAsync() =>
        RunAsync(ct =>
        {
            ct.ThrowIfCancellationRequested();
            var zone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneSimulation.Trim());
            var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, zone);
            SimulationResult = $"Simulation only: {zone.DisplayName} → {now:yyyy-MM-dd HH:mm zzz}. No stored schedules were changed.";
            return Task.CompletedTask;
        }, "That time-zone identifier is unavailable on this device.");

    private Task ExportSanitizedDiagnosticsAsync() =>
        RunAsync(async ct =>
        {
            var upcoming = await _reminders.GetUpcomingAsync(null, 30, ct);
            var d = await _notifications.GetDiagnosticsAsync(ct);
            var text = new StringBuilder()
                .AppendLine("CareNest sanitized diagnostics")
                .AppendLine($"GeneratedUtc: {DateTime.UtcNow:O}")
                .AppendLine($"AppVersion: {AppInfo.Current.VersionString}")
                .AppendLine($"Build: {AppInfo.Current.BuildString}")
                .AppendLine($"SchemaVersion: {await _repository.GetSchemaVersionAsync(ct)}")
                .AppendLine($"OS: {DeviceInfo.Current.Platform} {DeviceInfo.Current.VersionString}")
                .AppendLine($"TimeZone: {TimeZoneInfo.Local.Id}")
                .AppendLine($"PermissionGranted: {d.PermissionGranted}")
                .AppendLine($"SchedulingAvailable: {d.SchedulingAvailable}")
                .AppendLine($"ExactSchedulingAvailable: {d.ExactSchedulingAvailable}")
                .AppendLine($"BatteryOptimizationExempt: {d.BatteryOptimizationExempt}")
                .AppendLine($"FutureOccurrenceCountPreview: {upcoming.Count}")
                .AppendLine("Upcoming schedule inspector (redacted):");

            foreach (var item in upcoming)
            {
                text.AppendLine($"- occurrence={ShortId(item.OccurrenceId)} dueUtc={item.ScheduledUtc:O} state={item.State}");
            }
            text.AppendLine("Health names, document contents, notes, and contact details are intentionally omitted.");
            await _files.ShareTextAsync(text.ToString(), "CareNest sanitized diagnostics", ct);
        }, "CareNest could not export sanitized diagnostics.");

    private async Task LoadRedactedScheduleAsync(CancellationToken ct)
    {
        var upcoming = await _reminders.GetUpcomingAsync(null, 30, ct);
        RedactedSchedule.Clear();
        foreach (var item in upcoming)
        {
            RedactedSchedule.Add(new RedactedScheduleItem(item.OccurrenceId, item.ScheduledUtc, item.State));
        }
    }

    private async Task LoadDiagnosticsCoreAsync(CancellationToken ct)
    {
        var d = await _notifications.GetDiagnosticsAsync(ct);
        Diagnostics = string.Join(Environment.NewLine,
            new[]
            {
                $"Permission: {(d.PermissionGranted ? "granted" : "not granted")}",
                $"Scheduling: {(d.SchedulingAvailable ? "available" : "limited")}",
                $"Exact timing: {(d.ExactSchedulingAvailable ? "available" : "not guaranteed")}",
                $"Battery optimization exemption: {(d.BatteryOptimizationExempt ? "yes" : "no/unknown")}",
                d.PlatformSummary
            }.Concat(d.Warnings.Select(x => "• " + x)));
    }

    private async Task LoadAsyncAfterRestore(CancellationToken ct)
    {
        SchemaVersion = (await _repository.GetSchemaVersionAsync(ct)).ToString();
        StorageUsage = FormatBytes(await _documents.GetStorageUsageBytesAsync(ct));
        await LoadDiagnosticsCoreAsync(ct);
    }

    private Task<bool> BoolAsync(string key, bool fallback, CancellationToken ct) =>
        _state.GetBoolAsync(key, fallback, ct);

    private static TimeSpan ParseTime(string? value, TimeSpan fallback) =>
        TimeOnly.TryParse(value, out var parsed) ? parsed.ToTimeSpan() : fallback;

    private static string ShortId(string value) =>
        value.Length <= 8 ? value : value[..8];

    private static string FormatBytes(long bytes)
    {
        string[] units = ["B", "KB", "MB", "GB"];
        var value = (double)Math.Max(0, bytes);
        var unit = 0;
        while (value >= 1024 && unit < units.Length - 1)
        {
            value /= 1024;
            unit++;
        }
        return $"{value:0.##} {units[unit]}";
    }

    private static void ValidateBackupPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 10)
        {
            throw new ArgumentException("Use a backup password containing at least 10 characters.");
        }
    }
}
