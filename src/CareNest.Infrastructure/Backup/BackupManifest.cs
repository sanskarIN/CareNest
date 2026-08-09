namespace CareNest.Infrastructure.Backup;

internal sealed record BackupManifest(
    int FormatVersion,
    int SchemaVersion,
    DateTime CreatedUtc,
    string AppVersion,
    int DocumentCount);
