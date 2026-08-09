using CareNest.Domain.Entities;
using CareNest.Domain.Rules;

namespace CareNest.UnitTests;

public sealed class AppointmentAndProfileRulesTests
{
    [Fact]
    public void Profile_OnlyRequiresName()
    {
        var profile = new PersonProfile { Name = "Nickname" };
        ProfileRules.Validate(profile);
        Assert.Null(profile.BloodGroup);
        Assert.Null(profile.AllergiesAndSensitivities);
    }

    [Fact]
    public void Appointment_RejectsImpossibleReminderLead()
    {
        var appointment = new Appointment
        {
            ProfileId = "p",
            Title = "Visit",
            StartsUtc = DateTime.UtcNow.AddDays(1),
            TimeZoneId = TimeZoneInfo.Utc.Id,
            ReminderMinutesBefore = 50_000
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => AppointmentRules.Validate(appointment));
    }
}
