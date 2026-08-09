using CareNest.Domain.Common;

namespace CareNest.Domain.Entities;

public sealed class Appointment : EntityBase
{
    public string ProfileId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? ClinicianOrFacility { get; set; }
    public DateTime StartsUtc { get; set; }
    public string TimeZoneId { get; set; } = TimeZoneInfo.Local.Id;
    public string? Location { get; set; }
    public string? PreparationNotes { get; set; }
    public string? QuestionsToAsk { get; set; }
    public string? AttachmentDocumentId { get; set; }
    public DateTime? FollowUpDate { get; set; }
    public int? ReminderMinutesBefore { get; set; }
    public bool Archived { get; set; }
}
