using CareNest.Domain.Entities;
using CareNest.Shared;

namespace CareNest.Domain.Rules;

public static class AppointmentRules
{
    public static void Validate(Appointment appointment)
    {
        Guard.NotBlank(appointment.ProfileId, nameof(appointment.ProfileId), 64);
        appointment.Title = Guard.NotBlank(appointment.Title, nameof(appointment.Title), 180);
        _ = TimeZoneInfo.FindSystemTimeZoneById(appointment.TimeZoneId);

        if (appointment.ReminderMinutesBefore is < 0 or > 60 * 24 * 30)
        {
            throw new ArgumentOutOfRangeException(nameof(appointment.ReminderMinutesBefore));
        }
    }
}
