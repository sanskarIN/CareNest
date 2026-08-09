using CareNest.Domain.Common;
using CareNest.Domain.Enums;

namespace CareNest.Domain.Entities;

public sealed class MedicationLogEntry : EntityBase
{
    public string ProfileId { get; set; } = string.Empty;
    public string MedicineId { get; set; } = string.Empty;
    public string? ReminderOccurrenceId { get; set; }
    public MedicationLogStatus Status { get; set; }
    public DateTime EventUtc { get; set; }
    public string? Note { get; set; }
    public bool ManuallyEdited { get; set; }
}
