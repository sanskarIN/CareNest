using CareNest.Domain.Common;
using CareNest.Domain.Enums;

namespace CareNest.Domain.Entities;

public sealed class Medicine : EntityBase
{
    public string ProfileId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Form { get; set; } = string.Empty;
    public string? StrengthText { get; set; }
    public string? InstructionText { get; set; }
    public string? PrescriberNotes { get; set; }
    public string? PharmacyNotes { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime? EndDate { get; set; }
    public decimal? StockCount { get; set; }
    public decimal? RefillThreshold { get; set; }
    public decimal? StockChangePerTakenEvent { get; set; }
    public DateTime? RefillDate { get; set; }
    public string? PrescriptionDocumentId { get; set; }
    public MedicineState State { get; set; } = MedicineState.Active;
}
