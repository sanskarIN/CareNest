using CareNest.Application.Contracts;
using CareNest.Domain.Entities;

namespace CareNest.UnitTests.TestDoubles;

internal class RepositoryStub : ICareNestRepository
{
    public virtual Task InitializeAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<int> GetSchemaVersionAsync(CancellationToken cancellationToken = default) => Task.FromResult(0);

    public virtual Task<IReadOnlyList<PersonProfile>> GetProfilesAsync(bool includeArchived = false, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<PersonProfile>>(Array.Empty<PersonProfile>());

    public virtual Task<PersonProfile?> GetProfileAsync(string id, CancellationToken cancellationToken = default) => Task.FromResult<PersonProfile?>(null);

    public virtual Task SaveProfileAsync(PersonProfile profile, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task DeleteProfileCascadeAsync(string profileId, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<Medicine>> GetMedicinesAsync(string? profileId = null, bool includeArchived = false, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Medicine>>(Array.Empty<Medicine>());

    public virtual Task<Medicine?> GetMedicineAsync(string id, CancellationToken cancellationToken = default) => Task.FromResult<Medicine?>(null);

    public virtual Task SaveMedicineAsync(Medicine medicine, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task DeleteMedicineCascadeAsync(string medicineId, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<MedicineSchedule>> GetSchedulesForMedicineAsync(string medicineId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<MedicineSchedule>>(Array.Empty<MedicineSchedule>());

    public virtual Task<IReadOnlyList<MedicineSchedule>> GetEnabledSchedulesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<MedicineSchedule>>(Array.Empty<MedicineSchedule>());

    public virtual Task SaveScheduleAsync(MedicineSchedule schedule, IReadOnlyCollection<ScheduleTime> times, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<ScheduleTime>> GetScheduleTimesAsync(string scheduleId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<ScheduleTime>>(Array.Empty<ScheduleTime>());

    public virtual Task DeleteScheduleAsync(string scheduleId, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task UpsertOccurrencesAsync(IEnumerable<ReminderOccurrence> occurrences, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<ReminderOccurrence>> GetOccurrencesAsync(DateTime fromUtc, DateTime toUtc, string? profileId = null, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<ReminderOccurrence>>(Array.Empty<ReminderOccurrence>());

    public virtual Task<ReminderOccurrence?> GetOccurrenceAsync(string id, CancellationToken cancellationToken = default) => Task.FromResult<ReminderOccurrence?>(null);

    public virtual Task<ReminderOccurrence?> GetOccurrenceByKeyAsync(string key, CancellationToken cancellationToken = default) => Task.FromResult<ReminderOccurrence?>(null);

    public virtual Task SaveOccurrenceAsync(ReminderOccurrence occurrence, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task DeleteFutureOccurrencesForScheduleAsync(string scheduleId, DateTime fromUtc, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<MedicationLogEntry>> GetMedicationLogAsync(string? profileId = null, string? medicineId = null, DateTime? fromUtc = null, DateTime? toUtc = null, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<MedicationLogEntry>>(Array.Empty<MedicationLogEntry>());

    public virtual Task SaveMedicationLogEntryAsync(MedicationLogEntry entry, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<Appointment>> GetAppointmentsAsync(string? profileId = null, bool includeArchived = false, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Appointment>>(Array.Empty<Appointment>());

    public virtual Task<Appointment?> GetAppointmentAsync(string id, CancellationToken cancellationToken = default) => Task.FromResult<Appointment?>(null);

    public virtual Task SaveAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task DeleteAppointmentAsync(string id, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<CareDocument>> GetDocumentsAsync(string? profileId = null, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<CareDocument>>(Array.Empty<CareDocument>());

    public virtual Task<CareDocument?> GetDocumentAsync(string id, CancellationToken cancellationToken = default) => Task.FromResult<CareDocument?>(null);

    public virtual Task SaveDocumentAsync(CareDocument document, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task DeleteDocumentAsync(string id, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<Tag>> GetTagsAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Tag>>(Array.Empty<Tag>());

    public virtual Task SaveTagAsync(Tag tag, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task SetDocumentTagsAsync(string documentId, IEnumerable<string> tagIds, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<Tag>> GetDocumentTagsAsync(string documentId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Tag>>(Array.Empty<Tag>());

    public virtual Task<IReadOnlyList<StockAdjustment>> GetStockAdjustmentsAsync(string medicineId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<StockAdjustment>>(Array.Empty<StockAdjustment>());

    public virtual Task SaveStockAdjustmentAsync(StockAdjustment adjustment, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<decimal?> CalculateCurrentStockAsync(string medicineId, CancellationToken cancellationToken = default) => Task.FromResult<decimal?>(null);

    public virtual Task<IReadOnlyList<EmergencyContact>> GetEmergencyContactsAsync(string profileId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<EmergencyContact>>(Array.Empty<EmergencyContact>());

    public virtual Task SaveEmergencyContactAsync(EmergencyContact contact, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task DeleteEmergencyContactAsync(string id, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<string?> GetSettingAsync(string key, CancellationToken cancellationToken = default) => Task.FromResult<string?>(null);

    public virtual Task SetSettingAsync(string key, string value, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task AddAuditEntryAsync(AuditEntry entry, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task<IReadOnlyList<AuditEntry>> GetAuditEntriesAsync(string entityType, string entityId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<AuditEntry>>(Array.Empty<AuditEntry>());

    public virtual Task CreateBackupMetadataAsync(BackupMetadata metadata, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task VacuumAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public virtual Task ClearAllAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
