namespace CareNest.Domain.Enums;

public enum MedicineState
{
    Active = 0,
    Paused = 1,
    Completed = 2,
    Archived = 3
}

public enum ScheduleKind
{
    Daily = 0,
    SelectedWeekdays = 1,
    EveryNHours = 2,
    Cycle = 3,
    CustomDateRange = 4,
    AsNeeded = 5
}

public enum ReminderState
{
    Scheduled = 0,
    Snoozed = 1,
    Taken = 2,
    Skipped = 3,
    Delayed = 4,
    Missed = 5,
    Cancelled = 6
}

public enum MedicationLogStatus
{
    Taken = 0,
    Skipped = 1,
    Delayed = 2,
    Missed = 3,
    Custom = 4
}

public enum DocumentCategory
{
    Prescription = 0,
    LabReport = 1,
    ImagingReport = 2,
    DischargeSummary = 3,
    VaccinationRecord = 4,
    Insurance = 5,
    Custom = 6
}

public enum ThemePreference
{
    System = 0,
    Light = 1,
    Dark = 2
}

public enum AuditAction
{
    Created = 0,
    Updated = 1,
    Deleted = 2,
    Exported = 3,
    Restored = 4,
    ReminderStateChanged = 5
}
