using System.Globalization;
using System.Text.Json;
using CareNest.Application.Contracts;
using CareNest.App.Services;
using CareNest.Shared;
using Microsoft.Extensions.Logging;

namespace CareNest.App.ViewModels;

public sealed class SettingsViewModel : ViewModelBase
{
    private readonly ICareNestRepository repository;
    private readonly INotificationService notifications;
    private readonly IBackupService backups;
    private readonly IBackupReminderCoordinator backupReminders;
    private readonly IAppLockService appLock;
    private readonly INavigationService navigation;
    private readonly IUserDialogService dialogs;
    private readonly IFileShareService fileShare;
    private readonly IDocumentStore documentStore;
    private readonly ILogger<SettingsViewModel> logger;
    private bool isLoaded;
    private bool appLockEnabled;
    private bool genericNotificationLabels = true;
    private bool persistentNotifications;
    private bool soundEnabled = true;
    private bool vibrationEnabled = true;
    private bool followUpEnabled;
    private int followUpMinutes = AppConstants.DefaultFollowUpMinutes;
    private bool quietHoursEnabled;
    private TimeSpan quietStart = new(22, 0, 0);
    private TimeSpan quietEnd = new(7, 0, 0);
    private string theme = "System";
    private bool largeInterface;
    private bool reducedMotion;
    private bool backupReminderEnabled;
    private string diagnosticSummary = string.Empty;
    private string storageSummary = string.Empty;

    public SettingsViewModel(
        ICareNestRepository repository,
        INotificationService notifications,
        IBackupService backups,
        IBackupReminderCoordinator backupReminders,
        IAppLockService appLock,
        INavigationService navigation,
        IUserDialogService dialogs,
        IFileShareService fileShare,
        IDocumentStore documentStore,
        ILogger<SettingsViewModel> logger,
        SafeUiErrorService errors)
        : base(errors)
    {
        this.repository = repository;
        this.notifications = notifications;
        this.backups = backups;
        this.backupReminders = backupReminders;
        this.appLock = appLock;
        this.navigation = navigation;
        this.dialogs = dialogs;
        this.fileShare = fileShare;
        this.documentStore = documentStore;
        this.logger = logger;

        RefreshCommand = AsyncCommand(LoadAsync);
        SaveCommand = AsyncCommand(SaveAsync);
        ChangePinCommand = AsyncCommand(ChangePinAsync);
        DisableAppLockCommand = AsyncCommand(DisableAppLockAsync);
        CreateBackupCommand = AsyncCommand(CreateBackupAsync);
        RestoreBackupCommand = AsyncCommand(RestoreBackupAsync);
        TestReminderCommand = AsyncCommand(TestReminderAsync);
        RebuildRemindersCommand = AsyncCommand(RebuildRemindersAsync);
        ExportDiagnosticsCommand = AsyncCommand(ExportDiagnosticsAsync);
        ClearCacheCommand = AsyncCommand(ClearCacheAsync);
        VacuumDatabaseCommand = AsyncCommand(VacuumDatabaseAsync);
        ResetAllDataCommand = AsyncCommand(ResetAllDataAsync);
        OpenAboutCommand = AsyncCommand(() => navigation.NavigateAsync("about"));
    }

    public bool AppLockEnabled
    {
        get => appLockEnabled;
        set => SetProperty(ref appLockEnabled, value);
    }

    public bool GenericNotificationLabels
    {
        get => genericNotificationLabels;
        set => SetProperty(ref genericNotificationLabels, value);
    }

    public bool PersistentNotifications
    {
        get => persistentNotifications;
        set => SetProperty(ref persistentNotifications, value);
    }

    public bool SoundEnabled
    {
        get => soundEnabled;
        set => SetProperty(ref soundEnabled, value);
    }

    public bool VibrationEnabled
    {
        get => vibrationEnabled;
        set => SetProperty(ref vibrationEnabled, value);
    }

    public bool FollowUpEnabled
    {
        get => followUpEnabled;
        set => SetProperty(ref followUpEnabled, value);
    }

    public int FollowUpMinutes
    {
        get => followUpMinutes;
        set => SetProperty(ref followUpMinutes, Math.Clamp(value, 1, 180));
    }

    public bool QuietHoursEnabled
    {
        get => quietHoursEnabled;
        set => SetProperty(ref quietHoursEnabled, value);
    }

    public TimeSpan QuietStart
    {
        get => quietStart;
        set => SetProperty(ref quietStart, value);
    }

    public TimeSpan QuietEnd
    {
        get => quietEnd;
        set => SetProperty(ref quietEnd, value);
    }

    public string Theme
    {
        get => theme;
        set => SetProperty(ref theme, value);
    }

    public bool LargeInterface
    {
        get => largeInterface;
        set => SetProperty(ref largeInterface, value);
    }

    public bool ReducedMotion
    {
        get => reducedMotion;
        set => SetProperty(ref reducedMotion, value);
    }

    public bool BackupReminderEnabled
    {
        get => backupReminderEnabled;
        set => SetProperty(ref backupReminderEnabled, value);
    }

    public string DiagnosticSummary
    {
        get => diagnosticSummary;
        private set => SetProperty(ref diagnosticSummary, value);
    }

    public string StorageSummary
    {
        get => storageSummary;
        private set => SetProperty(ref storageSummary, value);
    }

    public IAsyncCommand RefreshCommand { get; }
    public IAsyncCommand SaveCommand { get; }
    public IAsyncCommand ChangePinCommand { get; }
    public IAsyncCommand DisableAppLockCommand { get; }
    public IAsyncCommand CreateBackupCommand { get; }
    public IAsyncCommand RestoreBackupCommand { get; }
    public IAsyncCommand TestReminderCommand { get; }
    public IAsyncCommand RebuildRemindersCommand { get; }
    public IAsyncCommand ExportDiagnosticsCommand { get; }
    public IAsyncCommand ClearCacheCommand { get; }
    public IAsyncCommand VacuumDatabaseCommand { get; }
    public IAsyncCommand ResetAllDataCommand { get; }
    public IAsyncCommand OpenAboutCommand { get; }

    public async Task OnAppearingAsync()
    {
        if (!isLoaded)
        {
            await ExecuteSafeAsync(LoadAsync, "Unable to load settings.");
            isLoaded = true;
        }
    }

    private async Task LoadAsync()
    {
        AppLockEnabled = await appLock.IsEnabledAsync();
        GenericNotificationLabels = await GetBoolAsync(SettingKeys.GenericNotificationLabels, true);
        PersistentNotifications = await GetBoolAsync(SettingKeys.PersistentNotifications, false);
        SoundEnabled = await GetBoolAsync(SettingKeys.SoundEnabled, true);
        VibrationEnabled = await GetBoolAsync(SettingKeys.VibrationEnabled, true);
        FollowUpEnabled = await GetBoolAsync(SettingKeys.FollowUpEnabled, false);
        FollowUpMinutes = await GetIntAsync(SettingKeys.FollowUpMinutes, AppConstants.DefaultFollowUpMinutes);
        QuietHoursEnabled = await GetBoolAsync(SettingKeys.QuietHoursEnabled, false);
        QuietStart = await GetTimeAsync(SettingKeys.QuietHoursStart, new TimeSpan(22, 0, 0));
        QuietEnd = await GetTimeAsync(SettingKeys.QuietHoursEnd, new TimeSpan(7, 0, 0));
        Theme = await repository.GetSettingAsync(SettingKeys.Theme) ?? "System";
        LargeInterface = await GetBoolAsync(SettingKeys.LargeInterface, false);
        ReducedMotion = await GetBoolAsync(SettingKeys.ReducedMotion, false);
        BackupReminderEnabled = await GetBoolAsync(SettingKeys.BackupReminderEnabled, false);

        ApplyTheme(Theme);
        await RefreshDiagnosticsAsync();
    }

    private async Task SaveAsync()
    {
        await repository.SetSettingAsync(SettingKeys.GenericNotificationLabels, Bool(GenericNotificationLabels));
        await repository.SetSettingAsync(SettingKeys.PersistentNotifications, Bool(PersistentNotifications));
        await repository.SetSettingAsync(SettingKeys.SoundEnabled, Bool(SoundEnabled));
        await repository.SetSettingAsync(SettingKeys.VibrationEnabled, Bool(VibrationEnabled));
        await repository.SetSettingAsync(SettingKeys.FollowUpEnabled, Bool(FollowUpEnabled));
        await repository.SetSettingAsync(SettingKeys.FollowUpMinutes, FollowUpMinutes.ToString(CultureInfo.InvariantCulture));
        await repository.SetSettingAsync(SettingKeys.QuietHoursEnabled, Bool(QuietHoursEnabled));
        await repository.SetSettingAsync(SettingKeys.QuietHoursStart, QuietStart.ToString("c", CultureInfo.InvariantCulture));
        await repository.SetSettingAsync(SettingKeys.QuietHoursEnd, QuietEnd.ToString("c", CultureInfo.InvariantCulture));
        await repository.SetSettingAsync(SettingKeys.Theme, Theme);
        await repository.SetSettingAsync(SettingKeys.LargeInterface, Bool(LargeInterface));
        await repository.SetSettingAsync(SettingKeys.ReducedMotion, Bool(ReducedMotion));
        await repository.SetSettingAsync(SettingKeys.BackupReminderEnabled, Bool(BackupReminderEnabled));

        ApplyTheme(Theme);
        await backupReminders.SyncAsync(requestPermission: BackupReminderEnabled);
        await RefreshDiagnosticsAsync();
    }

    private async Task ChangePinAsync()
    {
        var first = await dialogs.PromptSecretAsync("App lock", "Enter a new 6-32 digit PIN.", "Set PIN");
        if (string.IsNullOrWhiteSpace(first))
        {
            return;
        }

        var second = await dialogs.PromptSecretAsync("Confirm PIN", "Enter the PIN again.", "Confirm");
        if (!string.Equals(first, second, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The PIN entries do not match.");
        }

        await appLock.EnableAsync(first);
        AppLockEnabled = true;
    }

    private async Task DisableAppLockAsync()
    {
        if (!await dialogs.ConfirmAsync("Disable app lock?", "The device/app sandbox still protects local data, but CareNest will no longer ask for a PIN.", "Disable", "Cancel"))
        {
            return;
        }

        await appLock.DisableAsync();
        AppLockEnabled = false;
    }

    private async Task CreateBackupAsync()
    {
        var password = await dialogs.PromptSecretAsync("Create encrypted backup", "Enter a backup password with at least 8 characters. CareNest cannot recover it if you forget it.", "Continue");
        if (string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var saved = await fileShare.SaveAsync(
            $"CareNest-backup-{DateTime.UtcNow:yyyyMMdd-HHmm}.cnbak",
            async (stream, cancellationToken) =>
                await backups.CreateEncryptedBackupAsync(stream, password, AppInfo.Current.VersionString, cancellationToken),
            CancellationToken.None);

        if (saved)
        {
            await repository.SetSettingAsync(SettingKeys.LastBackupUtc, DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
            await backupReminders.SyncAsync(requestPermission: false);
        }
    }

    private async Task RestoreBackupAsync()
    {
        var source = await fileShare.OpenReadAsync([".cnbak"], CancellationToken.None);
        if (source is null)
        {
            return;
        }

        await using (source)
        {
            var password = await dialogs.PromptSecretAsync("Restore encrypted backup", "Enter the backup password.", "Inspect");
            if (string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var inspection = await backups.InspectAsync(source, password);
            var approved = await dialogs.ConfirmAsync(
                "Replace local CareNest data?",
                $"Backup {inspection.AppVersion} from {inspection.CreatedUtc.ToLocalTime():g} contains {inspection.DocumentCount} encrypted document(s). Restore replaces current local records and document storage. This cannot be undone unless you made a separate backup.",
                "Restore",
                "Cancel");
            if (!approved)
            {
                return;
            }

            source.Position = 0;
            await notifications.CancelAllAsync();
            await backups.RestoreEncryptedBackupAsync(source, password);
            await repository.InitializeAsync();
            await backupReminders.SyncAsync(requestPermission: false);
            await navigation.NavigateAsync("//dashboard");
        }
    }

    private async Task TestReminderAsync()
    {
        var diagnostics = await notifications.GetDiagnosticsAsync();
        if (!diagnostics.PermissionGranted)
        {
            _ = await notifications.RequestPermissionAsync();
        }
        await notifications.ShowTestAsync();
        await RefreshDiagnosticsAsync();
    }

    private async Task RebuildRemindersAsync()
    {
        await repository.SetSettingAsync("reminders.last-manual-rebuild-utc", DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
        await backupReminders.SyncAsync(requestPermission: false);
        await RefreshDiagnosticsAsync();
    }

    private async Task ExportDiagnosticsAsync()
    {
        var diagnostics = await notifications.GetDiagnosticsAsync();
        var schema = await repository.GetSchemaVersionAsync();
        var payload = new
        {
            Product = AppConstants.ProductName,
            Version = AppInfo.Current.VersionString,
            Build = AppInfo.Current.BuildString,
            Platform = DeviceInfo.Current.Platform.ToString(),
            DeviceType = DeviceInfo.Current.DeviceType.ToString(),
            TimeZoneId = TimeZoneInfo.Local.Id,
            SchemaVersion = schema,
            Notification = diagnostics,
            GeneratedUtc = DateTime.UtcNow,
            Safety = AppConstants.ReminderLimitation
        };

        await fileShare.ShareTextAsync(
            "CareNest sanitized diagnostics",
            JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
    }

    private async Task ClearCacheAsync()
    {
        var cache = FileSystem.CacheDirectory;
        if (Directory.Exists(cache))
        {
            foreach (var directory in Directory.EnumerateDirectories(cache))
            {
                Directory.Delete(directory, recursive: true);
            }
            foreach (var file in Directory.EnumerateFiles(cache))
            {
                File.Delete(file);
            }
        }
        await RefreshDiagnosticsAsync();
    }

    private async Task VacuumDatabaseAsync()
    {
        await repository.VacuumAsync();
        await RefreshDiagnosticsAsync();
    }

    private async Task ResetAllDataAsync()
    {
        if (!await dialogs.ConfirmAsync("Reset all local CareNest data?", "This permanently removes local profiles, medicines, appointments, logs, settings, encrypted documents, and app-lock data on this device. Export or back up anything you need first.", "Reset", "Cancel"))
        {
            return;
        }

        await notifications.CancelAllAsync();
        var storedFiles = await documentStore.ListStoredFilesAsync();

        // Clear structured records first. If this fails, encrypted payloads remain intact
        // rather than leaving database records pointing at files already deleted.
        await repository.ClearAllAsync();
        await appLock.DisableAsync();

        // Encrypted files are removed only after structured data has been cleared.
        // A file-system failure can leave an encrypted orphan that a retry can remove,
        // but it cannot leave a live CareNest document row referencing a deleted payload.
        foreach (var file in storedFiles)
        {
            await documentStore.DeleteAsync(file);
        }

        await navigation.NavigateAsync("//onboarding");
    }

    private async Task RefreshDiagnosticsAsync()
    {
        var diagnostics = await notifications.GetDiagnosticsAsync();
        var schema = await repository.GetSchemaVersionAsync();
        var usage = await documentStore.GetStorageUsageBytesAsync();
        DiagnosticSummary = $"{diagnostics.PlatformSummary}\nPermission: {(diagnostics.PermissionGranted ? "granted" : "not granted")}\nExact scheduling: {(diagnostics.ExactSchedulingAvailable ? "available" : "limited")}\nBattery optimization exemption: {(diagnostics.BatteryOptimizationExempt ? "yes" : "no/unknown")}\nDatabase schema: {schema}\nTime zone: {TimeZoneInfo.Local.Id}";
        if (diagnostics.Warnings.Count > 0)
        {
            DiagnosticSummary += "\n" + string.Join("\n", diagnostics.Warnings.Select(x => $"• {x}"));
        }
        StorageSummary = $"Encrypted document storage: {FormatBytes(usage)}";
        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Settings diagnostics refreshed for {Platform} with schema {SchemaVersion}.", DeviceInfo.Current.Platform, schema);
        }
    }

    private async Task<bool> GetBoolAsync(string key, bool fallback)
    {
        var raw = await repository.GetSettingAsync(key);
        return raw is null ? fallback : string.Equals(raw, "1", StringComparison.Ordinal);
    }

    private async Task<int> GetIntAsync(string key, int fallback)
    {
        var raw = await repository.GetSettingAsync(key);
        return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : fallback;
    }

    private async Task<TimeSpan> GetTimeAsync(string key, TimeSpan fallback)
    {
        var raw = await repository.GetSettingAsync(key);
        return TimeSpan.TryParseExact(raw, "c", CultureInfo.InvariantCulture, out var value) ? value : fallback;
    }

    private static string Bool(bool value) => value ? "1" : "0";

    private static string FormatBytes(long bytes)
    {
        if (bytes < 1024)
        {
            return $"{bytes} B";
        }
        if (bytes < 1024 * 1024)
        {
            return $"{bytes / 1024d:F1} KB";
        }
        return $"{bytes / (1024d * 1024d):F1} MB";
    }

    private static void ApplyTheme(string value)
    {
        Application.Current!.UserAppTheme = value switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }
}
