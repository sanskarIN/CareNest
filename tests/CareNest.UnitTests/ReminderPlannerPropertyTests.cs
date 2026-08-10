using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests;

public sealed class ReminderPlannerPropertyTests
{
    private readonly ReminderPlanner _planner = new();

    [Fact]
    public void DailySchedule_DeterministicRandomWindowsStayInsideHalfOpenBounds()
    {
        var random = new Random(20260810);
        var (medicine, profile, schedule) = CreateGraph(ScheduleKind.Daily);
        var times = new[]
        {
            new ScheduleTime { Hour = 0, Minute = 15 },
            new ScheduleTime { Hour = 8, Minute = 30 },
            new ScheduleTime { Hour = 23, Minute = 45 }
        };

        for (var iteration = 0; iteration < 64; iteration++)
        {
            var from = Utc(2026, 8, 1)
                .AddDays(random.Next(0, 20))
                .AddMinutes(random.Next(0, 24 * 60));
            var to = from.AddMinutes(random.Next(1, 7 * 24 * 60));

            var result = _planner.BuildOccurrences(medicine, schedule, times, profile, from, to);

            Assert.All(result, occurrence =>
            {
                Assert.True(occurrence.ScheduledUtc >= from);
                Assert.True(occurrence.ScheduledUtc < to);
            });
            Assert.Equal(result.Count, result.Select(x => x.OccurrenceKey).Distinct(StringComparer.Ordinal).Count());
            Assert.Equal(result.OrderBy(x => x.ScheduledUtc).Select(x => x.OccurrenceKey), result.Select(x => x.OccurrenceKey));
        }
    }

    [Fact]
    public void CycleSchedule_DeterministicPatternMatrixMatchesExplicitOnOffRule()
    {
        for (var onDays = 1; onDays <= 5; onDays++)
        {
            for (var offDays = 1; offDays <= 5; offDays++)
            {
                var (medicine, profile, schedule) = CreateGraph(ScheduleKind.Cycle);
                schedule.CycleOnDays = onDays;
                schedule.CycleOffDays = offDays;
                schedule.StartDate = new DateTime(2026, 8, 10);
                medicine.StartDate = schedule.StartDate;
                var from = Utc(2026, 8, 10);
                var to = Utc(2026, 9, 10);

                var result = _planner.BuildOccurrences(
                    medicine,
                    schedule,
                    new[] { new ScheduleTime { Hour = 12, Minute = 0 } },
                    profile,
                    from,
                    to);

                var expectedDates = Enumerable
                    .Range(0, 31)
                    .Where(dayIndex => dayIndex % (onDays + offDays) < onDays)
                    .Select(dayIndex => schedule.StartDate.AddDays(dayIndex).Date)
                    .ToArray();

                Assert.Equal(expectedDates, result.Select(x => x.LocalScheduledTime.Date));
            }
        }
    }

    [Fact]
    public void SelectedWeekdays_AllValidMasksEmitOnlySelectedDays()
    {
        for (var mask = 1; mask <= 0b1111111; mask++)
        {
            var (medicine, profile, schedule) = CreateGraph(ScheduleKind.SelectedWeekdays);
            schedule.WeekdayMask = mask;
            var from = Utc(2026, 8, 2);
            var to = from.AddDays(14);

            var result = _planner.BuildOccurrences(
                medicine,
                schedule,
                new[] { new ScheduleTime { Hour = 9, Minute = 0 } },
                profile,
                from,
                to);

            Assert.All(result, occurrence =>
            {
                var bit = 1 << (int)occurrence.LocalScheduledTime.DayOfWeek;
                Assert.NotEqual(0, mask & bit);
            });
        }
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(12)]
    [InlineData(24)]
    [InlineData(48)]
    [InlineData(168)]
    public void EveryNHours_AllowedIntervalsPreserveElapsedUtcSpacing(int intervalHours)
    {
        var (medicine, profile, schedule) = CreateGraph(ScheduleKind.EveryNHours);
        schedule.IntervalHours = intervalHours;
        schedule.StartDate = new DateTime(2026, 8, 1);
        medicine.StartDate = schedule.StartDate;
        var from = Utc(2026, 8, 1);
        var to = from.AddDays(30);

        var result = _planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 0, Minute = 0 } },
            profile,
            from,
            to);

        Assert.NotEmpty(result);
        for (var index = 1; index < result.Count; index++)
        {
            Assert.Equal(TimeSpan.FromHours(intervalHours), result[index].ScheduledUtc - result[index - 1].ScheduledUtc);
        }
    }

    private static (Medicine Medicine, PersonProfile Profile, MedicineSchedule Schedule) CreateGraph(ScheduleKind kind)
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
            Kind = kind,
            StartDate = new DateTime(2026, 8, 1),
            TimeZoneId = TimeZoneInfo.Utc.Id,
            Enabled = true
        };
        return (medicine, profile, schedule);
    }

    private static DateTime Utc(int year, int month, int day) =>
        DateTime.SpecifyKind(new DateTime(year, month, day), DateTimeKind.Utc);
}
