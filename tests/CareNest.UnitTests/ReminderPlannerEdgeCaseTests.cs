using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests;

public sealed class ReminderPlannerEdgeCaseTests
{
    private readonly ReminderPlanner _planner = new();

    [Fact]
    public void EveryNHours_InvalidDstGapAnchor_DoesNotInventShiftedStart()
    {
        var zone = TryFindZone("America/New_York");
        if (zone is null)
        {
            return;
        }

        var profile = new PersonProfile { Id = "profile", Name = "Profile" };
        var medicine = new Medicine
        {
            Id = "medicine",
            ProfileId = profile.Id,
            Name = "Record",
            Form = "Custom",
            StartDate = new DateTime(2026, 3, 8),
            State = MedicineState.Active
        };
        var schedule = new MedicineSchedule
        {
            Id = "schedule",
            MedicineId = medicine.Id,
            Kind = ScheduleKind.EveryNHours,
            IntervalHours = 8,
            StartDate = new DateTime(2026, 3, 8),
            TimeZoneId = zone.Id,
            Enabled = true
        };

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 2, Minute = 30 } },
            profile,
            Utc(2026, 3, 8),
            Utc(2026, 3, 10));

        Assert.Empty(result);
    }

    [Fact]
    public void CycleSchedule_LargeUserEnteredDayCounts_DoNotOverflowCycleArithmetic()
    {
        var profile = new PersonProfile { Id = "profile", Name = "Profile" };
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
            Kind = ScheduleKind.Cycle,
            StartDate = new DateTime(2026, 8, 1),
            CycleOnDays = int.MaxValue,
            CycleOffDays = int.MaxValue,
            TimeZoneId = TimeZoneInfo.Utc.Id,
            Enabled = true
        };

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 9, Minute = 0 } },
            profile,
            Utc(2026, 8, 1),
            Utc(2026, 8, 4));

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void EveryNHours_MaxDateEndBoundary_DoesNotOverflow()
    {
        var profile = new PersonProfile { Id = "profile", Name = "Profile" };
        var medicine = new Medicine
        {
            Id = "medicine",
            ProfileId = profile.Id,
            Name = "Record",
            Form = "Custom",
            StartDate = new DateTime(2026, 8, 1),
            EndDate = DateTime.MaxValue.Date,
            State = MedicineState.Active
        };
        var schedule = new MedicineSchedule
        {
            Id = "schedule",
            MedicineId = medicine.Id,
            Kind = ScheduleKind.EveryNHours,
            IntervalHours = 12,
            StartDate = new DateTime(2026, 8, 1),
            EndDate = DateTime.MaxValue.Date,
            TimeZoneId = TimeZoneInfo.Utc.Id,
            Enabled = true
        };

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 0, Minute = 0 } },
            profile,
            Utc(2026, 8, 1),
            Utc(2026, 8, 2));

        Assert.Equal(2, result.Count);
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
