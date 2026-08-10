using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests;

public sealed class ReminderPlannerUtcWindowTests
{
    private readonly ReminderPlanner _planner = new();

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified)]
    public void BuildOccurrences_RejectsNonUtcWindowStart(DateTimeKind kind)
    {
        var (medicine, schedule, times, profile) = CreateValidInputs();
        var from = DateTime.SpecifyKind(new DateTime(2026, 8, 10), kind);
        var to = DateTime.SpecifyKind(new DateTime(2026, 8, 11), DateTimeKind.Utc);

        var exception = Assert.Throws<ArgumentException>(() =>
            _planner.BuildOccurrences(medicine, schedule, times, profile, from, to));

        Assert.Equal("fromUtc", exception.ParamName);
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified)]
    public void BuildOccurrences_RejectsNonUtcWindowEnd(DateTimeKind kind)
    {
        var (medicine, schedule, times, profile) = CreateValidInputs();
        var from = DateTime.SpecifyKind(new DateTime(2026, 8, 10), DateTimeKind.Utc);
        var to = DateTime.SpecifyKind(new DateTime(2026, 8, 11), kind);

        var exception = Assert.Throws<ArgumentException>(() =>
            _planner.BuildOccurrences(medicine, schedule, times, profile, from, to));

        Assert.Equal("toUtc", exception.ParamName);
    }

    [Fact]
    public void BuildOccurrences_StillTreatsEndAsExclusiveForUtcWindow()
    {
        var (medicine, schedule, times, profile) = CreateValidInputs();
        times = new[] { new ScheduleTime { Hour = 0, Minute = 0 } };
        var from = Utc(2026, 8, 10);
        var to = Utc(2026, 8, 11);

        var result = _planner.BuildOccurrences(medicine, schedule, times, profile, from, to);

        var occurrence = Assert.Single(result);
        Assert.Equal(from, occurrence.ScheduledUtc);
        Assert.DoesNotContain(result, item => item.ScheduledUtc == to);
    }

    private static (Medicine Medicine, MedicineSchedule Schedule, IReadOnlyCollection<ScheduleTime> Times, PersonProfile Profile) CreateValidInputs()
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
        IReadOnlyCollection<ScheduleTime> times = new[]
        {
            new ScheduleTime { Hour = 9, Minute = 0 }
        };
        return (medicine, schedule, times, profile);
    }

    private static DateTime Utc(int year, int month, int day) =>
        DateTime.SpecifyKind(new DateTime(year, month, day), DateTimeKind.Utc);
}
