using CareNest.Domain.Common;

namespace CareNest.Domain.Entities;

public sealed class BackupMetadata : EntityBase
{
    public int FormatVersion { get; set; }
    public int SchemaVersion { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public string AppVersion { get; set; } = string.Empty;
    public string? DestinationHint { get; set; }
}
