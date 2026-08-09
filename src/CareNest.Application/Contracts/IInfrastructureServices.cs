using CareNest.Domain.Entities;

namespace CareNest.Application.Contracts;

public sealed record NotificationRequest(
    string OccurrenceId,
    DateTime ScheduledUtc,
    string Title,
    string Body,
    bool Persistent,
    string Category,
    bool PlaySound = true,
    bool Vibrate = true);

public sealed record NotificationDiagnostics(
    bool PermissionGranted,
    bool SchedulingAvailable,
    bool ExactSchedulingAvailable,
    bool BatteryOptimizationExempt,
    string PlatformSummary,
    IReadOnlyList<string> Warnings);

public interface INotificationService
{
    Task<bool> RequestPermissionAsync(CancellationToken cancellationToken = default);
    Task<NotificationDiagnostics> GetDiagnosticsAsync(CancellationToken cancellationToken = default);
    Task ScheduleAsync(NotificationRequest request, CancellationToken cancellationToken = default);
    Task CancelAsync(string occurrenceId, CancellationToken cancellationToken = default);
    Task CancelAllAsync(CancellationToken cancellationToken = default);
    Task ShowTestAsync(CancellationToken cancellationToken = default);
}

public interface IDocumentStore
{
    Task<StoredDocument> ImportAsync(Stream source, string originalFileName, string? contentType, CancellationToken cancellationToken = default);
    Task ExportDecryptedAsync(string encryptedFileName, Stream destination, CancellationToken cancellationToken = default);
    Task DeleteAsync(string encryptedFileName, CancellationToken cancellationToken = default);
    Task<long> GetStorageUsageBytesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> ListStoredFilesAsync(CancellationToken cancellationToken = default);
}

public sealed record StoredDocument(string EncryptedFileName, long OriginalSizeBytes, string Sha256, int EncryptionVersion);

public interface ISecretStore
{
    Task<byte[]?> GetBytesAsync(string key, CancellationToken cancellationToken = default);
    Task SetBytesAsync(string key, byte[] value, CancellationToken cancellationToken = default);
    Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default);
    Task SetStringAsync(string key, string value, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}

public interface IBackupService
{
    Task CreateEncryptedBackupAsync(Stream destination, string password, string appVersion, CancellationToken cancellationToken = default);
    Task<BackupInspection> InspectAsync(Stream source, string password, CancellationToken cancellationToken = default);
    Task RestoreEncryptedBackupAsync(Stream source, string password, CancellationToken cancellationToken = default);
}

public sealed record BackupInspection(int FormatVersion, int SchemaVersion, DateTime CreatedUtc, string AppVersion, int DocumentCount);

public interface IReportService
{
    Task<string> CreateProfileDataJsonAsync(string profileId, string outputPath, CancellationToken cancellationToken = default);
    Task<string> CreateProfileSummaryPdfAsync(string profileId, string outputPath, CancellationToken cancellationToken = default);
    Task<string> CreateMedicationLogCsvAsync(string? profileId, string outputPath, CancellationToken cancellationToken = default);
    Task<string> CreateUpcomingScheduleCsvAsync(string? profileId, string outputPath, CancellationToken cancellationToken = default);
    Task<string> CreateAppointmentHistoryCsvAsync(string? profileId, string outputPath, CancellationToken cancellationToken = default);
    Task<string> CreateDocumentListCsvAsync(string? profileId, string outputPath, CancellationToken cancellationToken = default);
    Task<string> CreateStockRefillCsvAsync(string? profileId, string outputPath, CancellationToken cancellationToken = default);
    Task<string> CreateMissedRemindersCsvAsync(string? profileId, string outputPath, CancellationToken cancellationToken = default);
}

public interface IAppFileGateway
{
    Task<PickedFile?> PickDocumentAsync(CancellationToken cancellationToken = default);
    Task<PickedFile?> CapturePhotoAsync(CancellationToken cancellationToken = default);
    Task<PickedFile?> PickBackupForRestoreAsync(CancellationToken cancellationToken = default);
    Task ShareFileAsync(string filePath, string title, CancellationToken cancellationToken = default);
    Task ShareTextAsync(string text, string title, CancellationToken cancellationToken = default);
}

public sealed record PickedFile(string FileName, string? ContentType, Func<CancellationToken, Task<Stream>> OpenReadAsync);

public interface IAppLockService
{
    Task<bool> IsEnabledAsync(CancellationToken cancellationToken = default);
    Task SetPinAsync(string pin, CancellationToken cancellationToken = default);
    Task<bool> VerifyPinAsync(string pin, CancellationToken cancellationToken = default);
    Task DisableAsync(CancellationToken cancellationToken = default);
}

public interface IAppNavigator
{
    Task GoToAsync(string route, IDictionary<string, object>? parameters = null, CancellationToken cancellationToken = default);
    Task GoBackAsync(CancellationToken cancellationToken = default);
    Task ResetToShellAsync(CancellationToken cancellationToken = default);
    Task ResetToOnboardingAsync(CancellationToken cancellationToken = default);
}
