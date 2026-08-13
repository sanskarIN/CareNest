using CareNest.Domain.Entities;
using CareNest.Shared;

namespace CareNest.Domain.Rules;

public static class AppointmentRules
{
    public static void Validate(Appointment appointment)
    {
        ArgumentNullException.ThrowIfNull(appointment);

        Guard.NotBlank(appointment.ProfileId, nameof(appointment.ProfileId), 64);
        appointment.Title = Guard.NotBlank(appointment.Title, nameof(appointment.Title), 180);
        appointment.TimeZoneId = Guard.NotBlank(appointment.TimeZoneId, nameof(appointment.TimeZoneId), 180).Trim();
        _ = TimeZoneInfo.FindSystemTimeZoneById(appointment.TimeZoneId);

        if (appointment.StartsUtc.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("Appointment start time must be an explicit UTC value.", nameof(appointment));
        }

        if (appointment.ReminderMinutesBefore is < 0 or > 60 * 24 * 30)
        {
            throw new ArgumentOutOfRangeException(nameof(appointment), "Appointment reminder must be between 0 minutes and 30 days before the appointment.");
        }
    }
}
