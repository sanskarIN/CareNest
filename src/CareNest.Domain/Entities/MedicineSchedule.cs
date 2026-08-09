using CareNest.Domain.Common;
using CareNest.Domain.Enums;

namespace CareNest.Domain.Entities;

public sealed class MedicineSchedule : EntityBase
{
    public string MedicineId { get; set; } = string.Empty;
    public ScheduleKind Kind { get; set; } = ScheduleKind.Daily;
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime? EndDate { get; set; }
    public int? IntervalHours { get; set; }
    public int? CycleOnDays { get; set; }
    public int? CycleOffDays { get; set; }
    public int WeekdayMask { get; set; } = 0b1111111;
    public string TimeZoneId { get; set; } = TimeZoneInfo.Local.Id;
    public int? FollowUpMinutes { get; set; }
    public bool Enabled { get; set; } = true;
}
