using CareNest.Domain.Common;
using CareNest.Domain.Enums;

namespace CareNest.Domain.Entities;

public sealed class CareDocument : EntityBase
{
    public string ProfileId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DocumentCategory Category { get; set; }
    public string? FolderName { get; set; }
    public string EncryptedFileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public long OriginalSizeBytes { get; set; }
    public string Sha256 { get; set; } = string.Empty;
    public int EncryptionVersion { get; set; } = 1;
    public string? Notes { get; set; }
}
