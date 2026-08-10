using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests;

public sealed class ReminderPlannerOwnershipTests
{
    private readonly ReminderPlanner _planner = new();

    [Fact]
    public void BuildOccurrences_RejectsScheduleFromDifferentMedicine()
    {
        var (medicine, profile, schedule) = CreateValidGraph();
        schedule.MedicineId = "different-medicine";

        var exception = Assert.Throws<ArgumentException>(() =>
            _planner.BuildOccurrences(
                medicine,
                schedule,
                ValidTimes(),
                profile,
                Utc(2026, 8, 10),
                Utc(2026, 8, 11)));

        Assert.Equal("schedule", exception.ParamName);
    }

    [Fact]
    public void BuildOccurrences_RejectsMedicineFromDifferentProfile()
    {
        var (medicine, profile, schedule) = CreateValidGraph();
        medicine.ProfileId = "different-profile";

        var exception = Assert.Throws<ArgumentException>(() =>
            _planner.BuildOccurrences(
                medicine,
                schedule,
                ValidTimes(),
                profile,
                Utc(2026, 8, 10),
                Utc(2026, 8, 11)));

        Assert.Equal("profile", exception.ParamName);
    }

    [Fact]
    public void BuildOccurrences_RejectsTimeBoundToDifferentSchedule()
    {
        var (medicine, profile, schedule) = CreateValidGraph();
        var times = new[]
        {
            new ScheduleTime
            {
                MedicineScheduleId = "different-schedule",
                Hour = 9,
                Minute = 0
            }
        };

        var exception = Assert.Throws<ArgumentException>(() =>
            _planner.BuildOccurrences(
                medicine,
                schedule,
                times,
                profile,
                Utc(2026, 8, 10),
                Utc(2026, 8, 11)));

        Assert.Equal("times", exception.ParamName);
    }

    [Fact]
    public void BuildOccurrences_AcceptsPersistedTimeBoundToSuppliedSchedule()
    {
        var (medicine, profile, schedule) = CreateValidGraph();
        var times = new[]
        {
            new ScheduleTime
            {
                MedicineScheduleId = schedule.Id,
                Hour = 9,
                Minute = 0
            }
        };

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            times,
            profile,
            Utc(2026, 8, 10),
            Utc(2026, 8, 11));

        var occurrence = Assert.Single(result);
        Assert.Equal(profile.Id, occurrence.ProfileId);
        Assert.Equal(medicine.Id, occurrence.MedicineId);
        Assert.Equal(schedule.Id, occurrence.ScheduleId);
    }

    [Fact]
    public void BuildOccurrences_AcceptsUnboundEditorTimeForSuppliedSchedule()
    {
        var (medicine, profile, schedule) = CreateValidGraph();

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            ValidTimes(),
            profile,
            Utc(2026, 8, 10),
            Utc(2026, 8, 11));

        Assert.Single(result);
    }

    private static (Medicine Medicine, PersonProfile Profile, MedicineSchedule Schedule) CreateValidGraph()
    {
        var profile = new PersonProfile
        {
            Id = "profile",
            Name = "Local profile"
        };

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

    private static IReadOnlyCollection<ScheduleTime> ValidTimes() =>
        new[] { new ScheduleTime { Hour = 9, Minute = 0 } };

    private static DateTime Utc(int year, int month, int day) =>
        DateTime.SpecifyKind(new DateTime(year, month, day), DateTimeKind.Utc);
}
