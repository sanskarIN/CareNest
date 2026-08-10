using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests;

public sealed class ReminderPlannerTests
{
    private readonly ReminderPlanner _planner = new();

    [Fact]
    public void DailySchedule_CreatesEveryExplicitTime()
    {
        var (medicine, profile, schedule) = CreateDaily();
        var times = new[]
        {
            new ScheduleTime { Hour = 8, Minute = 0 },
            new ScheduleTime { Hour = 20, Minute = 30 }
        };

        var from = DateTime.SpecifyKind(new DateTime(2026, 8, 10), DateTimeKind.Utc);
        var to = from.AddDays(2);

        var result = _planner.BuildOccurrences(medicine, schedule, times, profile, from, to);

        Assert.Equal(4, result.Count);
        Assert.Equal(4, result.Select(x => x.OccurrenceKey).Distinct().Count());
        Assert.All(result, x => Assert.Equal(ReminderState.Scheduled, x.State));
    }

    [Fact]
    public void AsNeededSchedule_DoesNotCreateAutomaticOccurrences()
    {
        var (medicine, profile, schedule) = CreateDaily();
        schedule.Kind = ScheduleKind.AsNeeded;

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            Array.Empty<ScheduleTime>(),
            profile,
            Utc(2026, 8, 10),
            Utc(2026, 8, 20));

        Assert.Empty(result);
    }

    [Fact]
    public void SelectedWeekdays_OnlyCreatesChosenDays()
    {
        var (medicine, profile, schedule) = CreateDaily();
        schedule.Kind = ScheduleKind.SelectedWeekdays;
        schedule.WeekdayMask = 1 << (int)DayOfWeek.Monday;

        var from = Utc(2026, 8, 9);
        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 9, Minute = 0 } },
            profile,
            from,
            from.AddDays(8));

        Assert.Single(result);
        Assert.Equal(DayOfWeek.Monday, result[0].LocalScheduledTime.DayOfWeek);
    }

    [Fact]
    public void CycleSchedule_UsesOnlyExplicitOnAndOffDays()
    {
        var (medicine, profile, schedule) = CreateDaily();
        schedule.Kind = ScheduleKind.Cycle;
        schedule.StartDate = new DateTime(2026, 8, 10);
        medicine.StartDate = schedule.StartDate;
        schedule.CycleOnDays = 2;
        schedule.CycleOffDays = 1;

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 9, Minute = 0 } },
            profile,
            Utc(2026, 8, 10),
            Utc(2026, 8, 16));

        var activeDates = result.Select(x => x.LocalScheduledTime.Date).ToArray();
        var expected = new[]
        {
            new DateTime(2026, 8, 10),
            new DateTime(2026, 8, 11),
            new DateTime(2026, 8, 13),
            new DateTime(2026, 8, 14)
        };
        Assert.Equal(expected, activeDates);
    }

    [Fact]
    public void CustomDateRange_StopsAtUserEnteredScheduleEndDate()
    {
        var (medicine, profile, schedule) = CreateDaily();
        schedule.Kind = ScheduleKind.CustomDateRange;
        schedule.StartDate = new DateTime(2026, 8, 10);
        schedule.EndDate = new DateTime(2026, 8, 11);
        medicine.StartDate = schedule.StartDate;

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 9, Minute = 0 } },
            profile,
            Utc(2026, 8, 9),
            Utc(2026, 8, 14));

        Assert.Equal(2, result.Count);
        Assert.All(result, occurrence => Assert.InRange(occurrence.LocalScheduledTime.Date, schedule.StartDate, schedule.EndDate.Value));
    }

    [Fact]
    public void MedicineEndDate_StopsOccurrencesAtUserEnteredBoundary()
    {
        var (medicine, profile, schedule) = CreateDaily();
        medicine.EndDate = new DateTime(2026, 8, 11);

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 9, Minute = 0 } },
            profile,
            Utc(2026, 8, 10),
            Utc(2026, 8, 14));

        Assert.Equal(2, result.Count);
        Assert.Equal(new DateTime(2026, 8, 11), result[^1].LocalScheduledTime.Date);
    }

    [Theory]
    [InlineData(MedicineState.Paused)]
    [InlineData(MedicineState.Completed)]
    [InlineData(MedicineState.Archived)]
    public void NonActiveMedicine_DoesNotCreateOccurrences(MedicineState state)
    {
        var (medicine, profile, schedule) = CreateDaily();
        medicine.State = state;

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 9, Minute = 0 } },
            profile,
            Utc(2026, 8, 10),
            Utc(2026, 8, 11));

        Assert.Empty(result);
    }

    [Fact]
    public void EveryNHours_UsesOnlyExplicitInterval()
    {
        var (medicine, profile, schedule) = CreateDaily();
        schedule.Kind = ScheduleKind.EveryNHours;
        schedule.IntervalHours = 8;
        schedule.StartDate = new DateTime(2026, 8, 10);

        var from = Utc(2026, 8, 10);
        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 0, Minute = 0 } },
            profile,
            from,
            from.AddDays(1));

        Assert.Equal(3, result.Count);
        Assert.Equal(TimeSpan.FromHours(8), result[1].ScheduledUtc - result[0].ScheduledUtc);
    }

    [Fact]
    public void FollowUp_IsSeparateOccurrenceWithoutChangingOriginalTime()
    {
        var (medicine, profile, schedule) = CreateDaily();
        schedule.FollowUpMinutes = 15;

        var from = Utc(2026, 8, 10);
        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 9, Minute = 0 } },
            profile,
            from,
            from.AddDays(1));

        Assert.Equal(2, result.Count);
        Assert.False(result[0].FollowUp);
        Assert.True(result[1].FollowUp);
        Assert.Equal(TimeSpan.FromMinutes(15), result[1].ScheduledUtc - result[0].ScheduledUtc);
    }

    [Fact]
    public void DisabledSchedule_CreatesNothing()
    {
        var (medicine, profile, schedule) = CreateDaily();
        schedule.Enabled = false;

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 9 } },
            profile,
            Utc(2026, 8, 10),
            Utc(2026, 8, 11));

        Assert.Empty(result);
    }

    [Fact]
    public void RebuildingSameWindow_ProducesStableOccurrenceKeys()
    {
        var (medicine, profile, schedule) = CreateDaily();
        var times = new[] { new ScheduleTime { Hour = 8, Minute = 30 } };
        var from = Utc(2026, 8, 10);
        var to = from.AddDays(3);

        var first = _planner.BuildOccurrences(medicine, schedule, times, profile, from, to);
        var second = _planner.BuildOccurrences(medicine, schedule, times, profile, from, to);

        Assert.Equal(first.Select(x => x.OccurrenceKey), second.Select(x => x.OccurrenceKey));
    }

    [Fact]
    public void AmbiguousLocalTime_SelectsDeterministicOccurrence()
    {
        var zone = TryFindZone("America/New_York");
        if (zone is null)
        {
            return;
        }

        var (medicine, profile, schedule) = CreateDaily();
        schedule.TimeZoneId = zone.Id;
        schedule.StartDate = new DateTime(2026, 11, 1);
        medicine.StartDate = schedule.StartDate;

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 1, Minute = 30 } },
            profile,
            Utc(2026, 11, 1),
            Utc(2026, 11, 2));

        Assert.Single(result);
        Assert.Equal(new DateTime(2026, 11, 1, 1, 30, 0), result[0].LocalScheduledTime);
    }

    [Fact]
    public void InvalidSpringForwardLocalTime_DoesNotInventReplacementTime()
    {
        var zone = TryFindZone("America/New_York");
        if (zone is null)
        {
            return;
        }

        var (medicine, profile, schedule) = CreateDaily();
        schedule.TimeZoneId = zone.Id;
        schedule.StartDate = new DateTime(2026, 3, 8);
        medicine.StartDate = schedule.StartDate;

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 2, Minute = 30 } },
            profile,
            Utc(2026, 3, 8),
            Utc(2026, 3, 9));

        Assert.Empty(result);
    }

    private static (Medicine, PersonProfile, MedicineSchedule) CreateDaily()
    {
        var profile = new PersonProfile { Id = "profile", Name = "Local profile" };
        var medicine = new Medicine
        {
            Id = "medicine",
            ProfileId = profile.Id,
            Name = "Record",
            Form = "Custom",
            StartDate = new DateTime(2026, 8, 1),
            State = MedicineState.Active
        };
        var schedule = new MedicineSchedule
        {
            Id = "schedule",
            MedicineId = medicine.Id,
            Kind = ScheduleKind.Daily,
            StartDate = new DateTime(2026, 8, 1),
            TimeZoneId = TimeZoneInfo.Utc.Id,
            Enabled = true
        };
        return (medicine, profile, schedule);
    }

    private static DateTime Utc(int year, int month, int day) =>
        DateTime.SpecifyKind(new DateTime(year, month, day), DateTimeKind.Utc);

    private static TimeZoneInfo? TryFindZone(string id)
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }
        catch (TimeZoneNotFoundException)
        {
            return null;
        }
    }
}
