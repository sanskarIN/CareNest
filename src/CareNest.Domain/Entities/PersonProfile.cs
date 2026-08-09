using CareNest.Domain.Common;

namespace CareNest.Domain.Entities;

public sealed class PersonProfile : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? PhotoPath { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? BloodGroup { get; set; }
    public string? AllergiesAndSensitivities { get; set; }
    public string? EmergencyContactId { get; set; }
    public string? Notes { get; set; }
    public string ProfileColor { get; set; } = "#5B7C6F";
    public string ProfileIcon { get; set; } = "person";
    public bool IsPrimary { get; set; }
    public bool IsArchived { get; set; }
}
