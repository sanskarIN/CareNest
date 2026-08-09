using CareNest.Domain.Common;

namespace CareNest.Domain.Entities;

public sealed class EmergencyContact : EntityBase
{
    public string ProfileId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Relationship { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Notes { get; set; }
}
