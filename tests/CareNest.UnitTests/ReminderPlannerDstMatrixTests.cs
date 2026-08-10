using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests;

public sealed class ReminderPlannerDstMatrixTests
{
    private readonly ReminderPlanner _planner = new();

    [Theory]
    [InlineData("America/New_York")]
    [InlineData("Europe/Berlin")]
    [InlineData("Australia/Sydney")]
    public void InvalidLocalTime_DoesNotInventReplacementAcrossDstZones(string zoneId)
    {
        var zone = TryFindZone(zoneId);
        if (zone is null)
        {
            return;
        }

        var invalidLocal = FindTransitionTime(zone, 2026, invalid: true);
        if (invalidLocal is null)
        {
            return;
        }

        var (medicine, profile, schedule) = CreateGraph(zone.Id, invalidLocal.Value.Date);
        var from = Utc(invalidLocal.Value.Date.AddDays(-1));
        var to = Utc(invalidLocal.Value.Date.AddDays(2));

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[]
            {
                new ScheduleTime
                {
                    Hour = invalidLocal.Value.Hour,
                    Minute = invalidLocal.Value.Minute
                }
            },
            profile,
            from,
            to);

        Assert.DoesNotContain(
            result,
            occurrence => occurrence.LocalScheduledTime == invalidLocal.Value);
    }

    [Theory]
    [InlineData("America/New_York")]
    [InlineData("Europe/Berlin")]
    [InlineData("Australia/Sydney")]
    public void AmbiguousLocalTime_ProducesOneDeterministicOccurrenceAcrossDstZones(string zoneId)
    {
        var zone = TryFindZone(zoneId);
        if (zone is null)
        {
            return;
        }

        var ambiguousLocal = FindTransitionTime(zone, 2026, invalid: false);
        if (ambiguousLocal is null)
        {
            return;
        }

        var (medicine, profile, schedule) = CreateGraph(zone.Id, ambiguousLocal.Value.Date);
        var times = new[]
        {
            new ScheduleTime
            {
                Hour = ambiguousLocal.Value.Hour,
                Minute = ambiguousLocal.Value.Minute
            }
        };
        var from = Utc(ambiguousLocal.Value.Date.AddDays(-1));
        var to = Utc(ambiguousLocal.Value.Date.AddDays(2));

        var first = _planner.BuildOccurrences(medicine, schedule, times, profile, from, to)
            .Where(x => x.LocalScheduledTime == ambiguousLocal.Value)
            .ToArray();
        var second = _planner.BuildOccurrences(medicine, schedule, times, profile, from, to)
            .Where(x => x.LocalScheduledTime == ambiguousLocal.Value)
            .ToArray();

        var firstOccurrence = Assert.Single(first);
        var secondOccurrence = Assert.Single(second);
        Assert.Equal(firstOccurrence.ScheduledUtc, secondOccurrence.ScheduledUtc);
        Assert.Equal(firstOccurrence.OccurrenceKey, secondOccurrence.OccurrenceKey);
    }

    private static DateTime? FindTransitionTime(TimeZoneInfo zone, int year, bool invalid)
    {
        var date = new DateTime(year, 1, 1);
        var end = date.AddYears(1);

        while (date < end)
        {
            for (var hour = 0; hour < 24; hour++)
            {
                foreach (var minute in new[] { 0, 30 })
                {
                    var local = DateTime.SpecifyKind(
                        date.AddHours(hour).AddMinutes(minute),
                        DateTimeKind.Unspecified);
                    var matches = invalid
                        ? zone.IsInvalidTime(local)
                        : zone.IsAmbiguousTime(local);
                    if (matches)
                    {
                        return local;
                    }
                }
            }

            date = date.AddDays(1);
        }

        return null;
    }

    private static (Medicine Medicine, PersonProfile Profile, MedicineSchedule Schedule) CreateGraph(
        string zoneId,
        DateTime startDate)
    {
        var profile = new PersonProfile { Id = "profile", Name = "Local profile" };
        var medicine = new Medicine
        {
            Id = "medicine",
            ProfileId = profile.Id,
            Name = "Record",
            Form = "Custom",
            StartDate = startDate,
            State = MedicineState.Active
        };
        var schedule = new MedicineSchedule
        {
            Id = "schedule",
            MedicineId = medicine.Id,
            Kind = ScheduleKind.Daily,
            StartDate = startDate,
            EndDate = startDate,
            TimeZoneId = zoneId,
            Enabled = true
        };
        return (medicine, profile, schedule);
    }

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
        catch (InvalidTimeZoneException)
        {
            return null;
        }
    }

    private static DateTime Utc(DateTime date) =>
        DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
}
