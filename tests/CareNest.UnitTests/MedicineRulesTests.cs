using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Domain.Rules;

namespace CareNest.UnitTests;

public sealed class MedicineRulesTests
{
    [Fact]
    public void Validate_DoesNotInterpretStrengthOrInstructions()
    {
        var medicine = ValidMedicine();
        medicine.StrengthText = "keep exactly: 12.5 custom units";
        medicine.InstructionText = "user-entered words";

        MedicineRules.Validate(medicine);

        Assert.Equal("keep exactly: 12.5 custom units", medicine.StrengthText);
        Assert.Equal("user-entered words", medicine.InstructionText);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Validate_RejectsNegativeUserStockChange(decimal value)
    {
        var medicine = ValidMedicine();
        medicine.StockChangePerTakenEvent = value;
        Assert.Throws<ArgumentOutOfRangeException>(() => MedicineRules.Validate(medicine));
    }

    [Fact]
    public void EveryNHours_RequiresAnExplicitInterval()
    {
        var schedule = ValidSchedule(ScheduleKind.EveryNHours);
        var times = new[] { new ScheduleTime { Hour = 9, Minute = 0 } };

        Assert.Throws<ArgumentOutOfRangeException>(() => MedicineRules.ValidateSchedule(schedule, times));
    }

    [Fact]
    public void EveryNHours_RequiresExactlyOneStartingTime()
    {
        var schedule = ValidSchedule(ScheduleKind.EveryNHours);
        schedule.IntervalHours = 8;
        var times = new[]
        {
            new ScheduleTime { Hour = 8, Minute = 0 },
            new ScheduleTime { Hour = 20, Minute = 0 }
        };

        Assert.Throws<ArgumentException>(() => MedicineRules.ValidateSchedule(schedule, times));
    }

    [Fact]
    public void AsNeeded_DoesNotRequireAutomaticTimes()
    {
        var schedule = ValidSchedule(ScheduleKind.AsNeeded);

        MedicineRules.ValidateSchedule(schedule, Array.Empty<ScheduleTime>());
    }

    [Fact]
    public void SelectedWeekdays_RequiresAtLeastOneSelectedDay()
    {
        var schedule = ValidSchedule(ScheduleKind.SelectedWeekdays);
        schedule.WeekdayMask = 0;
        var times = new[] { new ScheduleTime { Hour = 9, Minute = 0 } };

        Assert.Throws<ArgumentException>(() => MedicineRules.ValidateSchedule(schedule, times));
    }

    [Theory]
    [InlineData(null, 2)]
    [InlineData(3, null)]
    [InlineData(0, 2)]
    [InlineData(3, 0)]
    public void Cycle_RequiresExplicitPositiveOnAndOffDays(int? onDays, int? offDays)
    {
        var schedule = ValidSchedule(ScheduleKind.Cycle);
        schedule.CycleOnDays = onDays;
        schedule.CycleOffDays = offDays;
        var times = new[] { new ScheduleTime { Hour = 9, Minute = 0 } };

        Assert.Throws<ArgumentException>(() => MedicineRules.ValidateSchedule(schedule, times));
    }

    [Fact]
    public void Schedule_RejectsEndDateBeforeStartDate()
    {
        var schedule = ValidSchedule(ScheduleKind.Daily);
        schedule.StartDate = new DateTime(2026, 8, 10);
        schedule.EndDate = new DateTime(2026, 8, 9);
        var times = new[] { new ScheduleTime { Hour = 9, Minute = 0 } };

        Assert.Throws<ArgumentException>(() => MedicineRules.ValidateSchedule(schedule, times));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(24, 0)]
    [InlineData(12, -1)]
    [InlineData(12, 60)]
    public void Schedule_RejectsOutOfRangeClockTimes(int hour, int minute)
    {
        var schedule = ValidSchedule(ScheduleKind.Daily);
        var times = new[] { new ScheduleTime { Hour = hour, Minute = minute } };

        Assert.Throws<ArgumentOutOfRangeException>(() => MedicineRules.ValidateSchedule(schedule, times));
    }

    [Fact]
    public void Schedule_RejectsUnknownTimeZone()
    {
        var schedule = ValidSchedule(ScheduleKind.Daily);
        schedule.TimeZoneId = "CareNest/Not-A-Time-Zone";
        var times = new[] { new ScheduleTime { Hour = 9, Minute = 0 } };

        Assert.Throws<TimeZoneNotFoundException>(() => MedicineRules.ValidateSchedule(schedule, times));
    }

    private static Medicine ValidMedicine() => new()
    {
        ProfileId = "profile",
        Name = "Medicine record",
        Form = "Custom",
        StartDate = DateTime.Today
    };

    private static MedicineSchedule ValidSchedule(ScheduleKind kind) => new()
    {
        MedicineId = "m",
        Kind = kind,
        StartDate = DateTime.Today,
        TimeZoneId = TimeZoneInfo.Utc.Id,
        Enabled = true
    };
}
