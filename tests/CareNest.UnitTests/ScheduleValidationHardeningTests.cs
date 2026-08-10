using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Domain.Rules;

namespace CareNest.UnitTests;

public sealed class ScheduleValidationHardeningTests
{
    [Fact]
    public void ValidateSchedule_RejectsUnknownScheduleKind()
    {
        var schedule = ValidSchedule();
        schedule.Kind = (ScheduleKind)999;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            MedicineRules.ValidateSchedule(schedule, ValidTimes()));
    }

    [Theory]
    [InlineData(1 << 7)]
    [InlineData((1 << 1) | (1 << 8))]
    [InlineData(-1)]
    public void ValidateSchedule_RejectsUnsupportedWeekdayMaskBits(int mask)
    {
        var schedule = ValidSchedule();
        schedule.Kind = ScheduleKind.SelectedWeekdays;
        schedule.WeekdayMask = mask;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            MedicineRules.ValidateSchedule(schedule, ValidTimes()));
    }

    [Fact]
    public void ValidateSchedule_AllowsAllSevenSupportedWeekdays()
    {
        var schedule = ValidSchedule();
        schedule.Kind = ScheduleKind.SelectedWeekdays;
        schedule.WeekdayMask = 0b1111111;

        MedicineRules.ValidateSchedule(schedule, ValidTimes());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ValidateSchedule_RejectsBlankTimeZone(string value)
    {
        var schedule = ValidSchedule();
        schedule.TimeZoneId = value;

        Assert.Throws<ArgumentException>(() =>
            MedicineRules.ValidateSchedule(schedule, ValidTimes()));
    }

    [Fact]
    public void ValidateSchedule_TrimsTimeZoneIdentifierBeforeValidation()
    {
        var schedule = ValidSchedule();
        schedule.TimeZoneId = $"  {TimeZoneInfo.Utc.Id}  ";

        MedicineRules.ValidateSchedule(schedule, ValidTimes());

        Assert.Equal(TimeZoneInfo.Utc.Id, schedule.TimeZoneId);
    }

    private static MedicineSchedule ValidSchedule() => new()
    {
        Id = "schedule",
        MedicineId = "medicine",
        Kind = ScheduleKind.Daily,
        StartDate = new DateTime(2026, 8, 10),
        TimeZoneId = TimeZoneInfo.Utc.Id,
        Enabled = true
    };

    private static IReadOnlyCollection<ScheduleTime> ValidTimes() =>
        new[] { new ScheduleTime { Hour = 9, Minute = 0 } };
}
