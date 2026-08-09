using CareNest.Application.Contracts;

namespace CareNest.App.Services;

public partial class PlatformNotificationService : INotificationService
{
    public Task<bool> RequestPermissionAsync(
        CancellationToken cancellationToken = default) =>
        RequestPermissionCoreAsync(cancellationToken);

    public Task<NotificationDiagnostics> GetDiagnosticsAsync(
        CancellationToken cancellationToken = default) =>
        GetDiagnosticsCoreAsync(cancellationToken);

    public Task ScheduleAsync(
        NotificationRequest request,
        CancellationToken cancellationToken = default) =>
        ScheduleCoreAsync(request, cancellationToken);

    public Task CancelAsync(
        string occurrenceId,
        CancellationToken cancellationToken = default) =>
        CancelCoreAsync(occurrenceId, cancellationToken);

    public Task CancelAllAsync(
        CancellationToken cancellationToken = default) =>
        CancelAllCoreAsync(cancellationToken);

    public Task ShowTestAsync(
        CancellationToken cancellationToken = default) =>
        ShowTestCoreAsync(cancellationToken);

    private partial Task<bool> RequestPermissionCoreAsync(CancellationToken cancellationToken);
    private partial Task<NotificationDiagnostics> GetDiagnosticsCoreAsync(CancellationToken cancellationToken);
    private partial Task ScheduleCoreAsync(NotificationRequest request, CancellationToken cancellationToken);
    private partial Task CancelCoreAsync(string occurrenceId, CancellationToken cancellationToken);
    private partial Task CancelAllCoreAsync(CancellationToken cancellationToken);
    private partial Task ShowTestCoreAsync(CancellationToken cancellationToken);
}
