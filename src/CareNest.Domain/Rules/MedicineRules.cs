using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Shared;

namespace CareNest.Domain.Rules;

public static class MedicineRules
{
    private const int ValidWeekdayMask = 0b1111111;

    public static void Validate(Medicine medicine)
    {
        Guard.NotBlank(medicine.ProfileId, nameof(medicine.ProfileId), 64);
        medicine.Name = Guard.NotBlank(medicine.Name, nameof(medicine.Name), 120);
        medicine.Form = Guard.NotBlank(medicine.Form, nameof(medicine.Form), 80);

        if (medicine.EndDate is { } end && end.Date < medicine.StartDate.Date)
        {
            throw new ArgumentException("End date cannot be before start date.", nameof(medicine));
        }

        if (medicine.StockCount is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(medicine), "Stock count cannot be negative.");
        }

        if (medicine.RefillThreshold is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(medicine), "Refill threshold cannot be negative.");
        }

        if (medicine.StockChangePerTakenEvent is < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(medicine),
                "The user-entered stock change per Taken event cannot be negative.");
        }
    }

    public static void ValidateSchedule(MedicineSchedule schedule, IReadOnlyCollection<ScheduleTime> times)
    {
        Guard.NotBlank(schedule.MedicineId, nameof(schedule.MedicineId), 64);

        if (!Enum.IsDefined(schedule.Kind))
        {
            throw new ArgumentOutOfRangeException(nameof(schedule), "Schedule type is not recognized.");
        }

        if (schedule.EndDate is { } end && end.Date < schedule.StartDate.Date)
        {
            throw new ArgumentException("Schedule end date cannot be before start date.", nameof(schedule));
        }

        if (schedule.Kind == ScheduleKind.EveryNHours)
        {
            if (schedule.IntervalHours is null or < 1 or > 168)
            {
                throw new ArgumentOutOfRangeException(nameof(schedule), "Every-N-hours schedules require an explicit interval from 1 to 168 hours.");
            }

            if (times.Count != 1)
            {
                throw new ArgumentException("Every-N-hours schedules require exactly one user-selected starting time.", nameof(times));
            }
        }
        else if (schedule.Kind != ScheduleKind.AsNeeded && times.Count == 0)
        {
            throw new ArgumentException("At least one user-selected reminder time is required.", nameof(times));
        }

        if (schedule.Kind == ScheduleKind.SelectedWeekdays)
        {
            if (schedule.WeekdayMask == 0)
            {
                throw new ArgumentException("Select at least one weekday.", nameof(schedule));
            }

            if ((schedule.WeekdayMask & ~ValidWeekdayMask) != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(schedule), "Weekday selection contains unsupported bits.");
            }
        }

        if (schedule.Kind == ScheduleKind.Cycle &&
            (schedule.CycleOnDays is null or < 1 || schedule.CycleOffDays is null or < 1))
        {
            throw new ArgumentException("Cycle schedules require explicit on-days and off-days.", nameof(schedule));
        }

        foreach (var time in times)
        {
            Guard.Range(time.Hour, 0, 23, nameof(time.Hour));
            Guard.Range(time.Minute, 0, 59, nameof(time.Minute));
        }

        if (schedule.FollowUpMinutes is < 1 or > 1440)
        {
            throw new ArgumentOutOfRangeException(nameof(schedule), "Follow-up must be between 1 minute and 24 hours.");
        }

        schedule.TimeZoneId = Guard.NotBlank(schedule.TimeZoneId, nameof(schedule.TimeZoneId), 128);
        _ = TimeZoneInfo.FindSystemTimeZoneById(schedule.TimeZoneId);
    }
}
