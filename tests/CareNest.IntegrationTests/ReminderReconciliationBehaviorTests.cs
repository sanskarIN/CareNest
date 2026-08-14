using CareNest.Application.Contracts;
using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using Microsoft.Extensions.Logging.Abstractions;

namespace CareNest.IntegrationTests;

public sealed class ReminderReconciliationBehaviorTests
{
    private static readonly DateTimeOffset Now =
        new(2026, 8, 14, 2, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Upcoming_IncludesFutureSnoozeWhenOriginalScheduleTimeIsPast()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile { Id = "profile", Name = "Profile", IsPrimary = true };
        var medicine = new Medicine
        {
            Id = "medicine",
            ProfileId = profile.Id,
            Name = "Record",
            Form = "Custom",
            StartDate = Now.UtcDateTime.Date,
            State = MedicineState.Active
        };
        await store.Repository.SaveProfileAsync(profile);
        await store.Repository.SaveMedicineAsync(medicine);

        var occurrence = new ReminderOccurrence
        {
            Id = "occurrence",
            OccurrenceKey = "key",
            ScheduleId = "schedule",
            MedicineId = medicine.Id,
            ProfileId = profile.Id,
            ScheduledUtc = Now.UtcDateTime.AddHours(-1),
            LocalScheduledTime = Now.UtcDateTime.AddHours(-1),
            TimeZoneId = TimeZoneInfo.Utc.Id,
            State = ReminderState.Snoozed,
            SnoozedUntilUtc = Now.UtcDateTime.AddHours(1)
        };
        await store.Repository.SaveOccurrenceAsync(occurrence);

        var coordinator = CreateCoordinator(store.Repository, out _);
        var upcoming = await coordinator.GetUpcomingAsync(profile.Id, 20);

        var item = Assert.Single(upcoming);
        Assert.Equal(occurrence.Id, item.OccurrenceId);
        Assert.Equal(ReminderState.Snoozed, item.State);
        Assert.Equal(occurrence.SnoozedUntilUtc, item.ScheduledUtc);
    }

    [Fact]
    public async Task MarkOverdueAsMissed_UsesSnoozedDueTime()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile { Id = "profile", Name = "Profile", IsPrimary = true };
        var medicine = new Medicine
        {
            Id = "medicine",
            ProfileId = profile.Id,
            Name = "Record",
            Form = "Custom",
            StartDate = Now.UtcDateTime.Date.AddDays(-1),
            State = MedicineState.Active
        };
        await store.Repository.SaveProfileAsync(profile);
        await store.Repository.SaveMedicineAsync(medicine);

        var occurrence = new ReminderOccurrence
        {
            Id = "occurrence",
            OccurrenceKey = "key",
            ScheduleId = "schedule",
            MedicineId = medicine.Id,
            ProfileId = profile.Id,
            ScheduledUtc = Now.UtcDateTime.AddHours(-2),
            LocalScheduledTime = Now.UtcDateTime.AddHours(-2),
            TimeZoneId = TimeZoneInfo.Utc.Id,
            State = ReminderState.Snoozed,
            SnoozedUntilUtc = Now.UtcDateTime.AddMinutes(-10)
        };
        await store.Repository.SaveOccurrenceAsync(occurrence);

        var coordinator = CreateCoordinator(store.Repository, out var notifications);
        await coordinator.MarkOverdueAsMissedAsync();

        var reloaded = await store.Repository.GetOccurrenceAsync(occurrence.Id);
        Assert.NotNull(reloaded);
        Assert.Equal(ReminderState.Missed, reloaded!.State);
        Assert.Null(reloaded.SnoozedUntilUtc);
        Assert.Contains(occurrence.Id, notifications.CancelledIds);

        var log = Assert.Single(await store.Repository.GetMedicationLogAsync(
            profile.Id,
            medicine.Id));
        Assert.Equal(MedicationLogStatus.Missed, log.Status);
        Assert.Equal(occurrence.Id, log.ReminderOccurrenceId);
    }

    [Fact]
    public async Task Rebuild_CancelsAndMarksStaleFutureOccurrenceBeforeSchedulingReplacement()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile { Id = "profile", Name = "Profile", IsPrimary = true };
        var medicine = new Medicine
        {
            Id = "medicine",
            ProfileId = profile.Id,
            Name = "Record",
            Form = "Custom",
            StartDate = Now.UtcDateTime.Date,
            State = MedicineState.Active
        };
        var schedule = new MedicineSchedule
        {
            Id = "schedule",
            MedicineId = medicine.Id,
            Kind = ScheduleKind.Daily,
            StartDate = Now.UtcDateTime.Date,
            TimeZoneId = TimeZoneInfo.Utc.Id,
            Enabled = true
        };
        await store.Repository.SaveProfileAsync(profile);
        await store.Repository.SaveMedicineAsync(medicine);
        await store.Repository.SaveScheduleAsync(
            schedule,
            new[]
            {
                new ScheduleTime
                {
                    Id = "time",
                    MedicineScheduleId = schedule.Id,
                    Hour = 8,
                    Minute = 0
                }
            });

        var stale = new ReminderOccurrence
        {
            Id = "stale",
            OccurrenceKey = "stale-key-not-produced-by-planner",
            ScheduleId = schedule.Id,
            MedicineId = medicine.Id,
            ProfileId = profile.Id,
            ScheduledUtc = Now.UtcDateTime.AddHours(3),
            LocalScheduledTime = Now.UtcDateTime.AddHours(3),
            TimeZoneId = TimeZoneInfo.Utc.Id,
            State = ReminderState.Scheduled,
            PlatformNotificationId = "stale"
        };
        await store.Repository.SaveOccurrenceAsync(stale);

        var coordinator = CreateCoordinator(store.Repository, out var notifications);
        await coordinator.RebuildAsync();

        var reloaded = await store.Repository.GetOccurrenceAsync(stale.Id);
        Assert.NotNull(reloaded);
        Assert.Equal(ReminderState.Cancelled, reloaded!.State);
        Assert.Null(reloaded.PlatformNotificationId);
        Assert.Contains(stale.Id, notifications.CancelledIds);
        Assert.DoesNotContain(notifications.Scheduled, x => x.OccurrenceId == stale.Id);
    }

    private static ReminderCoordinator CreateCoordinator(
        ICareNestRepository repository,
        out NotificationSpy notifications)
    {
        notifications = new NotificationSpy();
        return new ReminderCoordinator(
            repository,
            notifications,
            new ReminderPlanner(),
            new FixedTimeProvider(Now),
            NullLogger<ReminderCoordinator>.Instance);
    }

    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
    }

    private sealed class NotificationSpy : INotificationService
    {
        public List<NotificationRequest> Scheduled { get; } = [];
        public List<string> CancelledIds { get; } = [];

        public Task<bool> RequestPermissionAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(true);

        public Task<NotificationDiagnostics> GetDiagnosticsAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(new NotificationDiagnostics(
                true,
                true,
                true,
                true,
                "integration-test",
                []));

        public Task ScheduleAsync(NotificationRequest request, CancellationToken cancellationToken = default)
        {
            Scheduled.Add(request);
            return Task.CompletedTask;
        }

        public Task CancelAsync(string occurrenceId, CancellationToken cancellationToken = default)
        {
            CancelledIds.Add(occurrenceId);
            return Task.CompletedTask;
        }

        public Task CancelAllAsync(CancellationToken cancellationToken = default) =>
            Task.CompletedTask;

        public Task ShowTestAsync(CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}
