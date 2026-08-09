using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using SQLite;

namespace CareNest.Infrastructure.Persistence;

public sealed class CareNestRepository(SqliteDatabase database) : ICareNestRepository
{
    private SQLiteAsyncConnection Db => database.Connection;

    public Task InitializeAsync(CancellationToken cancellationToken = default) => database.InitializeAsync(cancellationToken);
    public Task<int> GetSchemaVersionAsync(CancellationToken cancellationToken = default) => database.GetSchemaVersionAsync(cancellationToken);

    public async Task<IReadOnlyList<PersonProfile>> GetProfilesAsync(bool includeArchived = false, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        var sql = includeArchived
            ? "SELECT * FROM PersonProfile ORDER BY IsPrimary DESC, Name COLLATE NOCASE"
            : "SELECT * FROM PersonProfile WHERE IsArchived = 0 ORDER BY IsPrimary DESC, Name COLLATE NOCASE";
        return await Db.QueryAsync<PersonProfile>(sql);
    }

    public async Task<PersonProfile?> GetProfileAsync(string id, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return (await Db.QueryAsync<PersonProfile>("SELECT * FROM PersonProfile WHERE Id = ? LIMIT 1", id)).FirstOrDefault();
    }

    public async Task SaveProfileAsync(PersonProfile profile, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        if (profile.IsPrimary)
        {
            await Db.ExecuteAsync("UPDATE PersonProfile SET IsPrimary = 0 WHERE Id <> ?", profile.Id);
        }
        await Db.InsertOrReplaceAsync(profile);
    }

    public async Task DeleteProfileCascadeAsync(string profileId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        var medicines = await GetMedicinesAsync(profileId, true, cancellationToken);
        foreach (var medicine in medicines)
        {
            await DeleteMedicineCascadeAsync(medicine.Id, cancellationToken);
        }

        var documents = await GetDocumentsAsync(profileId, cancellationToken);
        foreach (var document in documents)
        {
            await Db.ExecuteAsync("DELETE FROM DocumentTag WHERE DocumentId = ?", document.Id);
            await Db.ExecuteAsync("DELETE FROM CareDocument WHERE Id = ?", document.Id);
        }

        await Db.ExecuteAsync("DELETE FROM Appointment WHERE ProfileId = ?", profileId);
        await Db.ExecuteAsync("DELETE FROM EmergencyContact WHERE ProfileId = ?", profileId);
        await Db.ExecuteAsync("DELETE FROM MedicationLogEntry WHERE ProfileId = ?", profileId);
        await Db.ExecuteAsync("DELETE FROM ReminderOccurrence WHERE ProfileId = ?", profileId);
        await Db.ExecuteAsync("DELETE FROM PersonProfile WHERE Id = ?", profileId);
    }

    public async Task<IReadOnlyList<Medicine>> GetMedicinesAsync(string? profileId = null, bool includeArchived = false, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        var conditions = new List<string>();
        var args = new List<object>();
        if (!string.IsNullOrWhiteSpace(profileId))
        {
            conditions.Add("ProfileId = ?");
            args.Add(profileId);
        }
        if (!includeArchived)
        {
            conditions.Add("State <> ?");
            args.Add((int)MedicineState.Archived);
        }

        var where = conditions.Count == 0 ? string.Empty : " WHERE " + string.Join(" AND ", conditions);
        return await Db.QueryAsync<Medicine>($"SELECT * FROM Medicine{where} ORDER BY Name COLLATE NOCASE", args.ToArray());
    }

    public async Task<Medicine?> GetMedicineAsync(string id, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return (await Db.QueryAsync<Medicine>("SELECT * FROM Medicine WHERE Id = ? LIMIT 1", id)).FirstOrDefault();
    }

    public async Task SaveMedicineAsync(Medicine medicine, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertOrReplaceAsync(medicine);
    }

    public async Task DeleteMedicineCascadeAsync(string medicineId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        var schedules = await Db.QueryAsync<MedicineSchedule>("SELECT * FROM MedicineSchedule WHERE MedicineId = ?", medicineId);
        foreach (var schedule in schedules)
        {
            await DeleteScheduleAsync(schedule.Id, cancellationToken);
        }

        await Db.ExecuteAsync("DELETE FROM MedicationLogEntry WHERE MedicineId = ?", medicineId);
        await Db.ExecuteAsync("DELETE FROM StockAdjustment WHERE MedicineId = ?", medicineId);
        await Db.ExecuteAsync("DELETE FROM ReminderOccurrence WHERE MedicineId = ?", medicineId);
        await Db.ExecuteAsync("DELETE FROM Medicine WHERE Id = ?", medicineId);
    }

    public async Task<IReadOnlyList<MedicineSchedule>> GetSchedulesForMedicineAsync(string medicineId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return await Db.QueryAsync<MedicineSchedule>("SELECT * FROM MedicineSchedule WHERE MedicineId = ? ORDER BY CreatedUtc", medicineId);
    }

    public async Task<IReadOnlyList<MedicineSchedule>> GetEnabledSchedulesAsync(CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return await Db.QueryAsync<MedicineSchedule>("SELECT * FROM MedicineSchedule WHERE Enabled = 1");
    }

    public async Task SaveScheduleAsync(MedicineSchedule schedule, IReadOnlyCollection<ScheduleTime> times, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertOrReplaceAsync(schedule);
        await Db.ExecuteAsync("DELETE FROM ScheduleTime WHERE MedicineScheduleId = ?", schedule.Id);
        foreach (var time in times)
        {
            cancellationToken.ThrowIfCancellationRequested();
            time.MedicineScheduleId = schedule.Id;
            await Db.InsertAsync(time);
        }
    }

    public async Task<IReadOnlyList<ScheduleTime>> GetScheduleTimesAsync(string scheduleId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return await Db.QueryAsync<ScheduleTime>("SELECT * FROM ScheduleTime WHERE MedicineScheduleId = ? ORDER BY Hour, Minute", scheduleId);
    }

    public async Task DeleteScheduleAsync(string scheduleId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.ExecuteAsync("DELETE FROM ReminderOccurrence WHERE ScheduleId = ?", scheduleId);
        await Db.ExecuteAsync("DELETE FROM ScheduleTime WHERE MedicineScheduleId = ?", scheduleId);
        await Db.ExecuteAsync("DELETE FROM MedicineSchedule WHERE Id = ?", scheduleId);
    }

    public async Task UpsertOccurrencesAsync(IEnumerable<ReminderOccurrence> occurrences, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        foreach (var occurrence in occurrences)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var existing = await GetOccurrenceByKeyAsync(occurrence.OccurrenceKey, cancellationToken);
            if (existing is not null)
            {
                occurrence.Id = existing.Id;
                occurrence.State = existing.State;
                occurrence.StateChangedUtc = existing.StateChangedUtc;
                occurrence.SnoozedUntilUtc = existing.SnoozedUntilUtc;
                occurrence.PlatformNotificationId = existing.PlatformNotificationId;
                occurrence.CreatedUtc = existing.CreatedUtc;
            }
            await Db.InsertOrReplaceAsync(occurrence);
        }
    }

    public async Task<IReadOnlyList<ReminderOccurrence>> GetOccurrencesAsync(DateTime fromUtc, DateTime toUtc, string? profileId = null, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        if (string.IsNullOrWhiteSpace(profileId))
        {
            return await Db.QueryAsync<ReminderOccurrence>(
                "SELECT * FROM ReminderOccurrence WHERE ScheduledUtc >= ? AND ScheduledUtc < ? ORDER BY ScheduledUtc",
                fromUtc, toUtc);
        }

        return await Db.QueryAsync<ReminderOccurrence>(
            "SELECT * FROM ReminderOccurrence WHERE ScheduledUtc >= ? AND ScheduledUtc < ? AND ProfileId = ? ORDER BY ScheduledUtc",
            fromUtc, toUtc, profileId);
    }

    public async Task<ReminderOccurrence?> GetOccurrenceAsync(string id, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return (await Db.QueryAsync<ReminderOccurrence>("SELECT * FROM ReminderOccurrence WHERE Id = ? LIMIT 1", id)).FirstOrDefault();
    }

    public async Task<ReminderOccurrence?> GetOccurrenceByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return (await Db.QueryAsync<ReminderOccurrence>("SELECT * FROM ReminderOccurrence WHERE OccurrenceKey = ? LIMIT 1", key)).FirstOrDefault();
    }

    public async Task SaveOccurrenceAsync(ReminderOccurrence occurrence, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        occurrence.UpdatedUtc = DateTime.UtcNow;
        await Db.InsertOrReplaceAsync(occurrence);
    }

    public async Task DeleteFutureOccurrencesForScheduleAsync(string scheduleId, DateTime fromUtc, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.ExecuteAsync("DELETE FROM ReminderOccurrence WHERE ScheduleId = ? AND ScheduledUtc >= ? AND State IN (?, ?)",
            scheduleId, fromUtc, (int)ReminderState.Scheduled, (int)ReminderState.Snoozed);
    }

    public async Task<IReadOnlyList<MedicationLogEntry>> GetMedicationLogAsync(
        string? profileId = null,
        string? medicineId = null,
        DateTime? fromUtc = null,
        DateTime? toUtc = null,
        CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        var conditions = new List<string>();
        var args = new List<object>();

        AddCondition(profileId, "ProfileId", conditions, args);
        AddCondition(medicineId, "MedicineId", conditions, args);
        if (fromUtc is not null)
        {
            conditions.Add("EventUtc >= ?");
            args.Add(fromUtc.Value);
        }
        if (toUtc is not null)
        {
            conditions.Add("EventUtc < ?");
            args.Add(toUtc.Value);
        }

        var where = conditions.Count == 0 ? string.Empty : " WHERE " + string.Join(" AND ", conditions);
        return await Db.QueryAsync<MedicationLogEntry>($"SELECT * FROM MedicationLogEntry{where} ORDER BY EventUtc DESC", args.ToArray());
    }

    public async Task SaveMedicationLogEntryAsync(MedicationLogEntry entry, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        entry.UpdatedUtc = DateTime.UtcNow;
        await Db.InsertOrReplaceAsync(entry);
    }

    public async Task<IReadOnlyList<Appointment>> GetAppointmentsAsync(string? profileId = null, bool includeArchived = false, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        var conditions = new List<string>();
        var args = new List<object>();
        AddCondition(profileId, "ProfileId", conditions, args);
        if (!includeArchived)
        {
            conditions.Add("Archived = 0");
        }
        var where = conditions.Count == 0 ? string.Empty : " WHERE " + string.Join(" AND ", conditions);
        return await Db.QueryAsync<Appointment>($"SELECT * FROM Appointment{where} ORDER BY StartsUtc", args.ToArray());
    }

    public async Task<Appointment?> GetAppointmentAsync(string id, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return (await Db.QueryAsync<Appointment>("SELECT * FROM Appointment WHERE Id = ? LIMIT 1", id)).FirstOrDefault();
    }

    public async Task SaveAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertOrReplaceAsync(appointment);
    }

    public async Task DeleteAppointmentAsync(string id, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.ExecuteAsync("DELETE FROM Appointment WHERE Id = ?", id);
    }

    public async Task<IReadOnlyList<CareDocument>> GetDocumentsAsync(string? profileId = null, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return string.IsNullOrWhiteSpace(profileId)
            ? await Db.QueryAsync<CareDocument>("SELECT * FROM CareDocument ORDER BY UpdatedUtc DESC")
            : await Db.QueryAsync<CareDocument>("SELECT * FROM CareDocument WHERE ProfileId = ? ORDER BY UpdatedUtc DESC", profileId);
    }

    public async Task<CareDocument?> GetDocumentAsync(string id, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return (await Db.QueryAsync<CareDocument>("SELECT * FROM CareDocument WHERE Id = ? LIMIT 1", id)).FirstOrDefault();
    }

    public async Task SaveDocumentAsync(CareDocument document, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertOrReplaceAsync(document);
    }

    public async Task DeleteDocumentAsync(string id, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.ExecuteAsync("DELETE FROM DocumentTag WHERE DocumentId = ?", id);
        await Db.ExecuteAsync("DELETE FROM CareDocument WHERE Id = ?", id);
    }

    public async Task<IReadOnlyList<Tag>> GetTagsAsync(CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return await Db.QueryAsync<Tag>("SELECT * FROM Tag ORDER BY Name COLLATE NOCASE");
    }

    public async Task SaveTagAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertOrReplaceAsync(tag);
    }

    public async Task SetDocumentTagsAsync(string documentId, IEnumerable<string> tagIds, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.ExecuteAsync("DELETE FROM DocumentTag WHERE DocumentId = ?", documentId);
        foreach (var tagId in tagIds.Distinct(StringComparer.Ordinal))
        {
            await Db.ExecuteAsync("INSERT OR IGNORE INTO DocumentTag (DocumentId, TagId) VALUES (?, ?)", documentId, tagId);
        }
    }

    public async Task<IReadOnlyList<Tag>> GetDocumentTagsAsync(string documentId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return await Db.QueryAsync<Tag>(
            "SELECT t.* FROM Tag t INNER JOIN DocumentTag dt ON dt.TagId = t.Id WHERE dt.DocumentId = ? ORDER BY t.Name COLLATE NOCASE",
            documentId);
    }

    public async Task<IReadOnlyList<StockAdjustment>> GetStockAdjustmentsAsync(string medicineId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return await Db.QueryAsync<StockAdjustment>("SELECT * FROM StockAdjustment WHERE MedicineId = ? ORDER BY EventUtc", medicineId);
    }

    public async Task SaveStockAdjustmentAsync(StockAdjustment adjustment, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertOrReplaceAsync(adjustment);
    }

    public async Task<decimal?> CalculateCurrentStockAsync(string medicineId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        var medicine = await GetMedicineAsync(medicineId, cancellationToken);
        if (medicine?.StockCount is null)
        {
            return null;
        }

        var rows = await Db.QueryAsync<StockSum>("SELECT COALESCE(SUM(QuantityDelta), 0) AS Total FROM StockAdjustment WHERE MedicineId = ?", medicineId);
        return medicine.StockCount.Value + (rows.FirstOrDefault()?.Total ?? 0m);
    }

    public async Task<IReadOnlyList<EmergencyContact>> GetEmergencyContactsAsync(string profileId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return await Db.QueryAsync<EmergencyContact>("SELECT * FROM EmergencyContact WHERE ProfileId = ? ORDER BY Name COLLATE NOCASE", profileId);
    }

    public async Task SaveEmergencyContactAsync(EmergencyContact contact, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertOrReplaceAsync(contact);
    }

    public async Task DeleteEmergencyContactAsync(string id, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.ExecuteAsync("DELETE FROM EmergencyContact WHERE Id = ?", id);
        await Db.ExecuteAsync("UPDATE PersonProfile SET EmergencyContactId = NULL WHERE EmergencyContactId = ?", id);
    }

    public async Task<string?> GetSettingAsync(string key, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return (await Db.QueryAsync<AppSetting>("SELECT * FROM AppSetting WHERE Key = ? LIMIT 1", key)).FirstOrDefault()?.Value;
    }

    public async Task SetSettingAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertOrReplaceAsync(new AppSetting { Key = key, Value = value, UpdatedUtc = DateTime.UtcNow });
    }

    public async Task AddAuditEntryAsync(AuditEntry entry, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertAsync(entry);
    }

    public async Task<IReadOnlyList<AuditEntry>> GetAuditEntriesAsync(string entityType, string entityId, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        return await Db.QueryAsync<AuditEntry>(
            "SELECT * FROM AuditEntry WHERE EntityType = ? AND EntityId = ? ORDER BY EventUtc DESC",
            entityType, entityId);
    }

    public async Task CreateBackupMetadataAsync(BackupMetadata metadata, CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        await Db.InsertAsync(metadata);
    }

    public async Task VacuumAsync(CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        await Db.ExecuteAsync("VACUUM;");
    }

    public async Task ClearAllAsync(CancellationToken cancellationToken = default)
    {
        await Ready(cancellationToken);
        var tables = new[]
        {
            "DocumentTag", "ScheduleTime", "ReminderOccurrence", "MedicationLogEntry",
            "StockAdjustment", "MedicineSchedule", "Medicine", "Appointment",
            "CareDocument", "Tag", "EmergencyContact", "BackupMetadata",
            "AuditEntry", "PersonProfile", "AppSetting"
        };

        foreach (var table in tables)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Db.ExecuteAsync($"DELETE FROM {table};");
        }

        await Db.ExecuteAsync("VACUUM;");
    }

    private async Task Ready(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await database.InitializeAsync(cancellationToken);
    }

    private static void AddCondition(string? value, string column, ICollection<string> conditions, ICollection<object> args)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        conditions.Add($"{column} = ?");
        args.Add(value);
    }

    private sealed class StockSum
    {
        public decimal Total { get; set; }
    }
}
