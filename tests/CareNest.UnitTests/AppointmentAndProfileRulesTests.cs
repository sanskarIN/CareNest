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
        var appointment = ValidAppointment();
        appointment.ReminderMinutesBefore = 50_000;

        Assert.Throws<ArgumentOutOfRangeException>(() => AppointmentRules.Validate(appointment));
    }

    [Fact]
    public void Appointment_RejectsLocalStartTime()
    {
        var appointment = ValidAppointment();
        appointment.StartsUtc = DateTime.SpecifyKind(appointment.StartsUtc, DateTimeKind.Local);

        Assert.Throws<ArgumentException>(() => AppointmentRules.Validate(appointment));
    }

    [Fact]
    public void Appointment_RejectsUnspecifiedStartTime()
    {
        var appointment = ValidAppointment();
        appointment.StartsUtc = DateTime.SpecifyKind(appointment.StartsUtc, DateTimeKind.Unspecified);

        Assert.Throws<ArgumentException>(() => AppointmentRules.Validate(appointment));
    }

    [Fact]
    public void Appointment_TrimsValidTimeZoneIdentifier()
    {
        var appointment = ValidAppointment();
        appointment.TimeZoneId = $"  {TimeZoneInfo.Utc.Id}  ";

        AppointmentRules.Validate(appointment);

        Assert.Equal(TimeZoneInfo.Utc.Id, appointment.TimeZoneId);
    }

    [Fact]
    public void Appointment_AcceptsExplicitUtcStartTime()
    {
        var appointment = ValidAppointment();

        AppointmentRules.Validate(appointment);

        Assert.Equal(DateTimeKind.Utc, appointment.StartsUtc.Kind);
    }

    private static Appointment ValidAppointment() => new()
    {
        ProfileId = "p",
        Title = "Visit",
        StartsUtc = new DateTime(2026, 8, 20, 10, 0, 0, DateTimeKind.Utc),
        TimeZoneId = TimeZoneInfo.Utc.Id,
        ReminderMinutesBefore = 30
    };
}
