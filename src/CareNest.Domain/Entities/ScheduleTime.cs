using CareNest.Domain.Common;

namespace CareNest.Domain.Entities;

public sealed class ScheduleTime : EntityBase
{
    public string MedicineScheduleId { get; set; } = string.Empty;
    public int Hour { get; set; }
    public int Minute { get; set; }

    public TimeOnly AsTimeOnly() => new(Hour, Minute);
}
