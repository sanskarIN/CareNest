using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.UnitTests.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;

namespace CareNest.UnitTests;

public sealed class ReminderCoordinatorActionRecoveryTests
{
    private static readonly DateTimeOffset Now =
        new(2026, 8, 14, 3, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task HandleOccurrenceAsync_PlatformCancelFails_DoesNotPersistHandledState()
    {
        var occurrence = ScheduledOccurrence();
        var repository = new RecordingRepository(occurrence);
        var notifications = new NotificationServiceSpy
        {
            CancelFailure = new InvalidOperationException("test cancellation failure")
        };
        var coordinator = CreateCoordinator(repository, notifications);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            coordinator.HandleOccurrenceAsync(occurrence.Id, ReminderState.Taken));

        Assert.Equal(ReminderState.Scheduled, occurrence.State);
        Assert.Equal(0, repository.SaveOccurrenceCalls);
        Assert.Single(notifications.CancelledOccurrenceIds);
    }

    [Fact]
    public async Task HandleOccurrenceAsync_FirstStateSaveFails_RestoresPreviousStateAndRebuilds()
    {
        var occurrence = ScheduledOccurrence();
        var repository = new RecordingRepository(occurrence)
        {
            RemainingSaveFailures = 1
        };
        var notifications = new NotificationServiceSpy();
        var coordinator = CreateCoordinator(repository, notifications);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            coordinator.HandleOccurrenceAsync(occurrence.Id, ReminderState.Taken));

        Assert.Equal(ReminderState.Scheduled, occurrence.State);
        Assert.Null(occurrence.SnoozedUntilUtc);
        Assert.True(repository.SaveOccurrenceCalls >= 2);
        Assert.Single(notifications.CancelledOccurrenceIds);
    }

    private static ReminderCoordinator CreateCoordinator(
        RecordingRepository repository,
        NotificationServiceSpy notifications) =>
        new(
            repository,
            notifications,
            new ReminderPlanner(),
            new FixedTimeProvider(Now),
            NullLogger<ReminderCoordinator>.Instance);

    private static ReminderOccurrence ScheduledOccurrence() =>
        new()
        {
            Id = "occurrence-1",
            ScheduleId = "schedule-1",
            MedicineId = "medicine-1",
            ProfileId = "profile-1",
            OccurrenceKey = "key-1",
            ScheduledUtc = Now.UtcDateTime.AddHours(1),
            LocalScheduledTime = Now.UtcDateTime.AddHours(1),
            TimeZoneId = TimeZoneInfo.Utc.Id,
            State = ReminderState.Scheduled,
            PlatformNotificationId = "occurrence-1"
        };

    private sealed class RecordingRepository(ReminderOccurrence occurrence) : RepositoryStub
    {
        public int RemainingSaveFailures { get; set; }

        public int SaveOccurrenceCalls { get; private set; }

        public override Task<ReminderOccurrence?> GetOccurrenceAsync(
            string id,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult<ReminderOccurrence?>(
                string.Equals(id, occurrence.Id, StringComparison.Ordinal)
                    ? occurrence
                    : null);
        }

        public override Task SaveOccurrenceAsync(
            ReminderOccurrence value,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SaveOccurrenceCalls++;
            if (RemainingSaveFailures > 0)
            {
                RemainingSaveFailures--;
                return Task.FromException(new InvalidOperationException("test save failure"));
            }

            return Task.CompletedTask;
        }
    }
}
