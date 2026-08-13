using CareNest.Application.Contracts;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests.TestDoubles;

internal sealed class ReminderCoordinatorSpy : IReminderCoordinator
{
    public int RebuildCount { get; private set; }

    public DateTime? LastRebuildFromUtc { get; private set; }

    public Task RebuildAsync(DateTime? fromUtc = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        RebuildCount++;
        LastRebuildFromUtc = fromUtc;
        return Task.CompletedTask;
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
}
