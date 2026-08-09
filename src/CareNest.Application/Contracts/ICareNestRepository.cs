using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.Application.Contracts;

public interface ICareNestRepository
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
    Task<int> GetSchemaVersionAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PersonProfile>> GetProfilesAsync(bool includeArchived = false, CancellationToken cancellationToken = default);
    Task<PersonProfile?> GetProfileAsync(string id, CancellationToken cancellationToken = default);
    Task SaveProfileAsync(PersonProfile profile, CancellationToken cancellationToken = default);
    Task DeleteProfileCascadeAsync(string profileId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Medicine>> GetMedicinesAsync(string? profileId = null, bool includeArchived = false, CancellationToken cancellationToken = default);
    Task<Medicine?> GetMedicineAsync(string id, CancellationToken cancellationToken = default);
    Task SaveMedicineAsync(Medicine medicine, CancellationToken cancellationToken = default);
    Task DeleteMedicineCascadeAsync(string medicineId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MedicineSchedule>> GetSchedulesForMedicineAsync(string medicineId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MedicineSchedule>> GetEnabledSchedulesAsync(CancellationToken cancellationToken = default);
    Task SaveScheduleAsync(MedicineSchedule schedule, IReadOnlyCollection<ScheduleTime> times, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ScheduleTime>> GetScheduleTimesAsync(string scheduleId, CancellationToken cancellationToken = default);
    Task DeleteScheduleAsync(string scheduleId, CancellationToken cancellationToken = default);

    Task UpsertOccurrencesAsync(IEnumerable<ReminderOccurrence> occurrences, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReminderOccurrence>> GetOccurrencesAsync(DateTime fromUtc, DateTime toUtc, string? profileId = null, CancellationToken cancellationToken = default);
    Task<ReminderOccurrence?> GetOccurrenceAsync(string id, CancellationToken cancellationToken = default);
    Task<ReminderOccurrence?> GetOccurrenceByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task SaveOccurrenceAsync(ReminderOccurrence occurrence, CancellationToken cancellationToken = default);
    Task DeleteFutureOccurrencesForScheduleAsync(string scheduleId, DateTime fromUtc, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MedicationLogEntry>> GetMedicationLogAsync(string? profileId = null, string? medicineId = null, DateTime? fromUtc = null, DateTime? toUtc = null, CancellationToken cancellationToken = default);
    Task SaveMedicationLogEntryAsync(MedicationLogEntry entry, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Appointment>> GetAppointmentsAsync(string? profileId = null, bool includeArchived = false, CancellationToken cancellationToken = default);
    Task<Appointment?> GetAppointmentAsync(string id, CancellationToken cancellationToken = default);
    Task SaveAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task DeleteAppointmentAsync(string id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CareDocument>> GetDocumentsAsync(string? profileId = null, CancellationToken cancellationToken = default);
    Task<CareDocument?> GetDocumentAsync(string id, CancellationToken cancellationToken = default);
    Task SaveDocumentAsync(CareDocument document, CancellationToken cancellationToken = default);
    Task DeleteDocumentAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Tag>> GetTagsAsync(CancellationToken cancellationToken = default);
    Task SaveTagAsync(Tag tag, CancellationToken cancellationToken = default);
    Task SetDocumentTagsAsync(string documentId, IEnumerable<string> tagIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Tag>> GetDocumentTagsAsync(string documentId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StockAdjustment>> GetStockAdjustmentsAsync(string medicineId, CancellationToken cancellationToken = default);
    Task SaveStockAdjustmentAsync(StockAdjustment adjustment, CancellationToken cancellationToken = default);
    Task<decimal?> CalculateCurrentStockAsync(string medicineId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EmergencyContact>> GetEmergencyContactsAsync(string profileId, CancellationToken cancellationToken = default);
    Task SaveEmergencyContactAsync(EmergencyContact contact, CancellationToken cancellationToken = default);
    Task DeleteEmergencyContactAsync(string id, CancellationToken cancellationToken = default);

    Task<string?> GetSettingAsync(string key, CancellationToken cancellationToken = default);
    Task SetSettingAsync(string key, string value, CancellationToken cancellationToken = default);

    Task AddAuditEntryAsync(AuditEntry entry, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditEntry>> GetAuditEntriesAsync(string entityType, string entityId, CancellationToken cancellationToken = default);

    Task CreateBackupMetadataAsync(BackupMetadata metadata, CancellationToken cancellationToken = default);
    Task VacuumAsync(CancellationToken cancellationToken = default);
    Task ClearAllAsync(CancellationToken cancellationToken = default);
}
