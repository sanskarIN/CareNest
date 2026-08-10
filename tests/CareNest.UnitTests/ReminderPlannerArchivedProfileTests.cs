using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.UnitTests;

public sealed class ReminderPlannerArchivedProfileTests
{
    [Fact]
    public void ArchivedProfile_DoesNotCreateAutomaticOccurrences()
    {
        var planner = new ReminderPlanner();
        var profile = new PersonProfile
        {
            Id = "profile",
            Name = "Archived profile",
            IsArchived = true
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
        var from = DateTime.SpecifyKind(new DateTime(2026, 8, 10), DateTimeKind.Utc);
        var to = from.AddDays(1);

        var result = planner.BuildOccurrences(
            medicine,
            schedule,
            new[] { new ScheduleTime { Hour = 9, Minute = 0 } },
            profile,
            from,
            to);

        Assert.Empty(result);
    }
}
