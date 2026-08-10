using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests;

public sealed class ReminderPlannerBoundaryTests
{
    private readonly ReminderPlanner _planner = new();

    [Fact]
    public void Window_IncludesFromBoundaryAndExcludesToBoundary()
    {
        var (medicine, profile, schedule) = CreateDaily();
        var times = new[] { new ScheduleTime { Hour = 0, Minute = 0 } };
        var from = Utc(2026, 8, 10);
        var to = Utc(2026, 8, 11);

        var result = _planner.BuildOccurrences(medicine, schedule, times, profile, from, to);

        var occurrence = Assert.Single(result);
        Assert.Equal(from, occurrence.ScheduledUtc);
        Assert.True(occurrence.ScheduledUtc < to);
    }

    [Fact]
    public void DuplicateUserTimes_DoNotCreateDuplicateOccurrences()
    {
        var (medicine, profile, schedule) = CreateDaily();
        var times = new[]
        {
            new ScheduleTime { Hour = 9, Minute = 0 },
            new ScheduleTime { Hour = 9, Minute = 0 }
        };

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            times,
            profile,
            Utc(2026, 8, 10),
            Utc(2026, 8, 11));

        Assert.Single(result);
        Assert.Single(result.Select(x => x.OccurrenceKey).Distinct(StringComparer.Ordinal));
    }

    [Fact]
    public void OutOfOrderUserTimes_ReturnChronologicalOccurrences()
    {
        var (medicine, profile, schedule) = CreateDaily();
        var times = new[]
        {
            new ScheduleTime { Hour = 20, Minute = 0 },
            new ScheduleTime { Hour = 8, Minute = 0 }
        };

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            times,
            profile,
            Utc(2026, 8, 10),
            Utc(2026, 8, 11));

        Assert.Equal(2, result.Count);
        Assert.True(result[0].ScheduledUtc < result[1].ScheduledUtc);
        Assert.Equal(8, result[0].LocalScheduledTime.Hour);
        Assert.Equal(20, result[1].LocalScheduledTime.Hour);
    }

    private static (Medicine Medicine, PersonProfile Profile, MedicineSchedule Schedule) CreateDaily()
    {
        var profile = new PersonProfile { Id = "boundary-profile", Name = "Boundary profile" };
        var medicine = new Medicine
        {
            Id = "boundary-medicine",
            ProfileId = profile.Id,
            Name = "Boundary record",
            Form = "Custom",
            StartDate = new DateTime(2026, 8, 1),
            State = MedicineState.Active
        };
        var schedule = new MedicineSchedule
        {
            Id = "boundary-schedule",
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
}
