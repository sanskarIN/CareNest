using CareNest.Domain.Common;
using CareNest.Domain.Enums;

namespace CareNest.Domain.Entities;

public sealed class ReminderOccurrence : EntityBase
{
    public string ScheduleId { get; set; } = string.Empty;
    public string MedicineId { get; set; } = string.Empty;
    public string ProfileId { get; set; } = string.Empty;
    public string OccurrenceKey { get; set; } = string.Empty;
    public DateTime ScheduledUtc { get; set; }
    public DateTime LocalScheduledTime { get; set; }
    public string TimeZoneId { get; set; } = string.Empty;
    public ReminderState State { get; set; } = ReminderState.Scheduled;
    public DateTime? StateChangedUtc { get; set; }
    public DateTime? SnoozedUntilUtc { get; set; }
    public string? PlatformNotificationId { get; set; }
    public bool FollowUp { get; set; }
}
