using CareNest.Application.Contracts;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests.TestDoubles;

internal sealed class ReminderCoordinatorSpy : IReminderCoordinator
{
    public int RebuildCount { get; private set; }

    public int RestoreCount { get; private set; }

    public DateTime? LastRebuildFromUtc { get; private set; }

    public List<string> CancelledMedicineIds { get; } = [];

    public List<string> CancelledProfileIds { get; } = [];

    public Exception? RebuildFailure { get; set; }

    public Exception? CancelMedicineFailure { get; set; }

    public Exception? CancelProfileFailure { get; set; }

    public Task RebuildAsync(DateTime? fromUtc = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        RebuildCount++;
        LastRebuildFromUtc = fromUtc;
        return RebuildFailure is null
            ? Task.CompletedTask
            : Task.FromException(RebuildFailure);
    }

    public Task HandleOccurrenceAsync(
        string occurrenceId,
        ReminderState newState,
        DateTime? snoozedUntilUtc = null,
        string? note = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }

    public Task MarkOverdueAsMissedAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<ReminderPreview>> GetUpcomingAsync(
        string? profileId,
        int take = 20,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<ReminderPreview>>(Array.Empty<ReminderPreview>());
    }

    public Task CancelFutureForMedicineAsync(
        string medicineId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        CancelledMedicineIds.Add(medicineId);
        return CancelMedicineFailure is null
            ? Task.CompletedTask
            : Task.FromException(CancelMedicineFailure);
    }

    public Task CancelFutureForProfileAsync(
        string profileId,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        CancelledProfileIds.Add(profileId);
        return CancelProfileFailure is null
            ? Task.CompletedTask
            : Task.FromException(CancelProfileFailure);
    }

    public async Task TryRestoreReminderRequestsAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        RestoreCount++;
        try
        {
            await RebuildAsync(cancellationToken: cancellationToken);
        }
        catch
        {
            // Mirrors the production compensation contract: restoration is best effort.
        }
    }
}
