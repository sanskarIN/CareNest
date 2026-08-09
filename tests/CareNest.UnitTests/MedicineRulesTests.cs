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
        var schedule = new MedicineSchedule
        {
            MedicineId = "m",
            Kind = ScheduleKind.EveryNHours,
            StartDate = DateTime.Today,
            TimeZoneId = TimeZoneInfo.Utc.Id,
            Enabled = true
        };
        var times = new[] { new ScheduleTime { Hour = 9, Minute = 0 } };
        Assert.Throws<ArgumentOutOfRangeException>(() => MedicineRules.ValidateSchedule(schedule, times));
    }

    [Fact]
    public void AsNeeded_DoesNotRequireAutomaticTimes()
    {
        var schedule = new MedicineSchedule
        {
            MedicineId = "m",
            Kind = ScheduleKind.AsNeeded,
            StartDate = DateTime.Today,
            TimeZoneId = TimeZoneInfo.Utc.Id,
            Enabled = true
        };

        MedicineRules.ValidateSchedule(schedule, Array.Empty<ScheduleTime>());
    }

    private static Medicine ValidMedicine() => new()
    {
        ProfileId = "profile",
        Name = "Medicine record",
        Form = "Custom",
        StartDate = DateTime.Today
    };
}
