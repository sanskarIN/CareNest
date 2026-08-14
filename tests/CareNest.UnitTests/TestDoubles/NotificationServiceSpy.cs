using CareNest.Application.Contracts;

namespace CareNest.UnitTests.TestDoubles;

internal sealed class NotificationServiceSpy : INotificationService
{
    public NotificationDiagnostics Diagnostics { get; set; } = new(
        PermissionGranted: true,
        SchedulingAvailable: true,
        ExactSchedulingAvailable: true,
        BatteryOptimizationExempt: true,
        PlatformSummary: "test",
        Warnings: Array.Empty<string>());

    public bool PermissionRequestResult { get; set; } = true;

    public Exception? ScheduleFailure { get; set; }

    public Exception? CancelFailure { get; set; }

    public int PermissionRequestCount { get; private set; }

    public int TestNotificationCount { get; private set; }

    public List<NotificationRequest> Scheduled { get; } = [];

    public List<string> CancelledOccurrenceIds { get; } = [];

    public Task<bool> RequestPermissionAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        PermissionRequestCount++;
        return Task.FromResult(PermissionRequestResult);
    }

    public Task<NotificationDiagnostics> GetDiagnosticsAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Diagnostics);
    }

    public Task ScheduleAsync(NotificationRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (ScheduleFailure is not null)
        {
            return Task.FromException(ScheduleFailure);
        }

        Scheduled.Add(request);
        return Task.CompletedTask;
    }

    public Task CancelAsync(string occurrenceId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        CancelledOccurrenceIds.Add(occurrenceId);
        return CancelFailure is null
            ? Task.CompletedTask
            : Task.FromException(CancelFailure);
    }

    public Task CancelAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        CancelledOccurrenceIds.Clear();
        Scheduled.Clear();
        return Task.CompletedTask;
    }

    public Task ShowTestAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        TestNotificationCount++;
        return Task.CompletedTask;
    }
}
