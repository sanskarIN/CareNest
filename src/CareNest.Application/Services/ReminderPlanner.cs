using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Domain.Rules;

namespace CareNest.Application.Services;

public sealed class ReminderPlanner
{
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "The planner is intentionally registered as an injectable application service so callers are not coupled to static behavior.")]
    public IReadOnlyList<ReminderOccurrence> BuildOccurrences(
        Medicine medicine,
        MedicineSchedule schedule,
        IReadOnlyCollection<ScheduleTime> times,
        PersonProfile profile,
        DateTime fromUtc,
        DateTime toUtc)
    {
        ArgumentNullException.ThrowIfNull(medicine);
        ArgumentNullException.ThrowIfNull(schedule);
        ArgumentNullException.ThrowIfNull(times);
        ArgumentNullException.ThrowIfNull(profile);

        ValidateUtcWindow(fromUtc, toUtc);
        ValidateOwnership(medicine, schedule, times, profile);
        MedicineRules.ValidateSchedule(schedule, times);

        if (!schedule.Enabled ||
            medicine.State != MedicineState.Active ||
            schedule.Kind == ScheduleKind.AsNeeded ||
            toUtc <= fromUtc)
        {
            return [];
        }

        var zone = TimeZoneInfo.FindSystemTimeZoneById(schedule.TimeZoneId);
        var fromLocal = TimeZoneInfo.ConvertTimeFromUtc(fromUtc, zone);
        var toLocal = TimeZoneInfo.ConvertTimeFromUtc(toUtc, zone);

        var startDate = MaxDate(schedule.StartDate.Date, medicine.StartDate.Date, fromLocal.Date.AddDays(-1));
        var endDate = MinDate(
            schedule.EndDate?.Date ?? DateTime.MaxValue.Date,
            medicine.EndDate?.Date ?? DateTime.MaxValue.Date,
            toLocal.Date.AddDays(1));

        if (endDate < startDate)
        {
            return [];
        }

        var result = new List<ReminderOccurrence>();
        if (schedule.Kind == ScheduleKind.EveryNHours)
        {
            BuildIntervalOccurrences(result, medicine, schedule, times.Single(), profile, zone, fromUtc, toUtc);
        }
        else
        {
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (!IsDateActive(schedule, date))
                {
                    continue;
                }

                foreach (var time in times)
                {
                    var local = DateTime.SpecifyKind(date.Add(time.AsTimeOnly().ToTimeSpan()), DateTimeKind.Unspecified);
                    AddOccurrence(result, medicine, schedule, profile, zone, local, fromUtc, toUtc, false);

                    if (schedule.FollowUpMinutes is { } followUp)
                    {
                        AddOccurrence(result, medicine, schedule, profile, zone, local.AddMinutes(followUp), fromUtc, toUtc, true);
                    }
                }
            }
        }

        return result
            .GroupBy(x => x.OccurrenceKey, StringComparer.Ordinal)
            .Select(x => x.First())
            .OrderBy(x => x.ScheduledUtc)
            .ToArray();
    }

    private static void ValidateUtcWindow(DateTime fromUtc, DateTime toUtc)
    {
        if (fromUtc.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("Reminder planning window start must be UTC.", nameof(fromUtc));
        }

        if (toUtc.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("Reminder planning window end must be UTC.", nameof(toUtc));
        }
    }

    private static void ValidateOwnership(
        Medicine medicine,
        MedicineSchedule schedule,
        IReadOnlyCollection<ScheduleTime> times,
        PersonProfile profile)
    {
        if (!string.Equals(schedule.MedicineId, medicine.Id, StringComparison.Ordinal))
        {
            throw new ArgumentException(
                "The reminder schedule does not belong to the supplied medicine record.",
                nameof(schedule));
        }

        if (!string.Equals(medicine.ProfileId, profile.Id, StringComparison.Ordinal))
        {
            throw new ArgumentException(
                "The medicine record does not belong to the supplied local profile.",
                nameof(profile));
        }

        foreach (var time in times)
        {
            if (!string.IsNullOrWhiteSpace(time.MedicineScheduleId) &&
                !string.Equals(time.MedicineScheduleId, schedule.Id, StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    "A reminder time belongs to a different schedule.",
                    nameof(times));
            }
        }
    }

    private static void BuildIntervalOccurrences(
        List<ReminderOccurrence> result,
        Medicine medicine,
        MedicineSchedule schedule,
        ScheduleTime startTime,
        PersonProfile profile,
        TimeZoneInfo zone,
        DateTime fromUtc,
        DateTime toUtc)
    {
        var interval = TimeSpan.FromHours(schedule.IntervalHours!.Value);
        var localStart = DateTime.SpecifyKind(schedule.StartDate.Date.Add(startTime.AsTimeOnly().ToTimeSpan()), DateTimeKind.Unspecified);
        var utcStart = SafeLocalToUtc(localStart, zone);
        if (utcStart is null)
        {
            utcStart = SafeLocalToUtc(localStart.AddHours(1), zone);
        }

        if (utcStart is null)
        {
            return;
        }

        var cursor = utcStart.Value;
        if (cursor < fromUtc)
        {
            var intervals = Math.Floor((fromUtc - cursor).TotalHours / interval.TotalHours);
            cursor = cursor.AddHours(interval.TotalHours * Math.Max(0, intervals));
            while (cursor < fromUtc)
            {
                cursor = cursor.Add(interval);
            }
        }

        var scheduleEndLocal = schedule.EndDate?.Date.AddDays(1).AddTicks(-1);
        var medicineEndLocal = medicine.EndDate?.Date.AddDays(1).AddTicks(-1);

        while (cursor < toUtc)
        {
            var local = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(cursor, DateTimeKind.Utc), zone);
            if ((scheduleEndLocal is null || local <= scheduleEndLocal) &&
                (medicineEndLocal is null || local <= medicineEndLocal))
            {
                AddOccurrence(result, medicine, schedule, profile, zone, DateTime.SpecifyKind(local, DateTimeKind.Unspecified), fromUtc, toUtc, false);

                if (schedule.FollowUpMinutes is { } followUp)
                {
                    var followUpLocal = DateTime.SpecifyKind(local.AddMinutes(followUp), DateTimeKind.Unspecified);
                    AddOccurrence(result, medicine, schedule, profile, zone, followUpLocal, fromUtc, toUtc, true);
                }
            }

            cursor = cursor.Add(interval);
        }
    }

    private static bool IsDateActive(MedicineSchedule schedule, DateTime date)
    {
        if (date.Date < schedule.StartDate.Date || schedule.EndDate is { } end && date.Date > end.Date)
        {
            return false;
        }

        return schedule.Kind switch
        {
            ScheduleKind.Daily or ScheduleKind.CustomDateRange => true,
            ScheduleKind.SelectedWeekdays => (schedule.WeekdayMask & (1 << (int)date.DayOfWeek)) != 0,
            ScheduleKind.Cycle => IsCycleActive(schedule, date),
            _ => false
        };
    }

    private static bool IsCycleActive(MedicineSchedule schedule, DateTime date)
    {
        var onDays = schedule.CycleOnDays!.Value;
        var offDays = schedule.CycleOffDays!.Value;
        var cycleLength = onDays + offDays;
        var dayIndex = (date.Date - schedule.StartDate.Date).Days;
        return dayIndex >= 0 && dayIndex % cycleLength < onDays;
    }

    private static void AddOccurrence(
        List<ReminderOccurrence> result,
        Medicine medicine,
        MedicineSchedule schedule,
        PersonProfile profile,
        TimeZoneInfo zone,
        DateTime local,
        DateTime fromUtc,
        DateTime toUtc,
        bool followUp)
    {
        var utc = SafeLocalToUtc(local, zone);
        if (utc is null || utc < fromUtc || utc >= toUtc)
        {
            return;
        }

        var keyRaw = $"{schedule.Id}|{local:yyyyMMddHHmm}|{zone.Id}|{followUp}";
        var key = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(keyRaw))).ToLowerInvariant();

        result.Add(new ReminderOccurrence
        {
            ScheduleId = schedule.Id,
            MedicineId = medicine.Id,
            ProfileId = profile.Id,
            OccurrenceKey = key,
            ScheduledUtc = utc.Value,
            LocalScheduledTime = local,
            TimeZoneId = zone.Id,
            State = ReminderState.Scheduled,
            FollowUp = followUp
        });
    }

    private static DateTime? SafeLocalToUtc(DateTime local, TimeZoneInfo zone)
    {
        if (zone.IsInvalidTime(local))
        {
            return null;
        }

        if (zone.IsAmbiguousTime(local))
        {
            var offsets = zone.GetAmbiguousTimeOffsets(local);
            var selected = offsets.Max();
            return new DateTimeOffset(local, selected).UtcDateTime;
        }

        return TimeZoneInfo.ConvertTimeToUtc(local, zone);
    }

    private static DateTime MaxDate(params DateTime[] dates) => dates.Max();
    private static DateTime MinDate(params DateTime[] dates) => dates.Min();
}
