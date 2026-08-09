using CareNest.Domain.Common;
using CareNest.Domain.Enums;

namespace CareNest.Domain.Entities;

public sealed class AuditEntry : EntityBase
{
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public AuditAction Action { get; set; }
    public DateTime EventUtc { get; set; }
    public string? ChangedFieldsCsv { get; set; }
    public string? SafeSummary { get; set; }
}
