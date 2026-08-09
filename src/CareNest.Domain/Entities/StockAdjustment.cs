using CareNest.Domain.Common;

namespace CareNest.Domain.Entities;

public sealed class StockAdjustment : EntityBase
{
    public string MedicineId { get; set; } = string.Empty;
    public decimal QuantityDelta { get; set; }
    public DateTime EventUtc { get; set; }
    public string? Reason { get; set; }
    public string? MedicationLogEntryId { get; set; }
}
