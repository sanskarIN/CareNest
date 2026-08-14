using CareNest.Domain.Enums;

namespace CareNest.Application.Contracts;

public interface IReminderCoordinator
{
    Task RebuildAsync(DateTime? fromUtc = null, CancellationToken cancellationToken = default);
    Task HandleOccurrenceAsync(string occurrenceId, ReminderState newState, DateTime? snoozedUntilUtc = null, string? note = null, CancellationToken cancellationToken = default);
    Task MarkOverdueAsMissedAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReminderPreview>> GetUpcomingAsync(string? profileId, int take = 20, CancellationToken cancellationToken = default);
    Task CancelFutureForMedicineAsync(string medicineId, CancellationToken cancellationToken = default);
    Task CancelFutureForProfileAsync(string profileId, CancellationToken cancellationToken = default);
}

public sealed record ReminderPreview(
    string OccurrenceId,
    string MedicineId,
    string MedicineName,
    string ProfileId,
    string ProfileName,
    DateTime ScheduledUtc,
    DateTime LocalScheduledTime,
    string TimeZoneId,
    ReminderState State);
