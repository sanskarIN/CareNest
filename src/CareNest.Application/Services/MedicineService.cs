using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Domain.Rules;

namespace CareNest.Application.Services;

public sealed class MedicineService(
    ICareNestRepository repository,
    IReminderCoordinator reminders,
    TimeProvider timeProvider) : IMedicineService
{
    public Task<IReadOnlyList<Medicine>> ListAsync(string? profileId = null, CancellationToken cancellationToken = default) =>
        repository.GetMedicinesAsync(profileId, false, cancellationToken);

    public Task<Medicine?> GetAsync(string id, CancellationToken cancellationToken = default) =>
        repository.GetMedicineAsync(id, cancellationToken);

    public async Task SaveAsync(Medicine medicine, CancellationToken cancellationToken = default)
    {
        MedicineRules.Validate(medicine);
        var exists = await repository.GetMedicineAsync(medicine.Id, cancellationToken) is not null;
        medicine.Touch(timeProvider.GetUtcNow().UtcDateTime);
        await repository.SaveMedicineAsync(medicine, cancellationToken);
        await repository.AddAuditEntryAsync(new AuditEntry
        {
            EntityType = nameof(Medicine),
            EntityId = medicine.Id,
            Action = exists ? AuditAction.Updated : AuditAction.Created,
            EventUtc = timeProvider.GetUtcNow().UtcDateTime,
            SafeSummary = exists ? "Medicine record updated" : "Medicine record created"
        }, cancellationToken);
        await reminders.RebuildAsync(cancellationToken: cancellationToken);
    }

    public async Task SaveScheduleAsync(
        MedicineSchedule schedule,
        IReadOnlyCollection<ScheduleTime> times,
        CancellationToken cancellationToken = default)
    {
        MedicineRules.ValidateSchedule(schedule, times);
        schedule.Touch(timeProvider.GetUtcNow().UtcDateTime);
        await repository.SaveScheduleAsync(schedule, times, cancellationToken);

        // Rebuild owns platform-request reconciliation. Deleting future rows here would
        // remove the information required to cancel obsolete platform notifications.
        await reminders.RebuildAsync(cancellationToken: cancellationToken);

        await repository.AddAuditEntryAsync(new AuditEntry
        {
            EntityType = nameof(MedicineSchedule),
            EntityId = schedule.Id,
            Action = AuditAction.Updated,
            EventUtc = timeProvider.GetUtcNow().UtcDateTime,
            ChangedFieldsCsv = "Schedule",
            SafeSummary = "Medicine schedule updated"
        }, cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            await reminders.CancelFutureForMedicineAsync(id, cancellationToken);
            await repository.DeleteMedicineCascadeAsync(id, cancellationToken);
        }
        catch (Exception primaryFailure)
        {
            var recoveryFailure = await TryRestoreReminderRequestsAsync();
            if (recoveryFailure is not null)
            {
                throw new AggregateException(
                    "Medicine deletion failed and reminder requests could not be fully restored.",
                    primaryFailure,
                    recoveryFailure);
            }

            throw;
        }

        await repository.AddAuditEntryAsync(new AuditEntry
        {
            EntityType = nameof(Medicine),
            EntityId = id,
            Action = AuditAction.Deleted,
            EventUtc = timeProvider.GetUtcNow().UtcDateTime,
            SafeSummary = "Medicine record deleted"
        }, cancellationToken);
    }

    public async Task ApplyStockAdjustmentAsync(
        string medicineId,
        decimal delta,
        string? reason,
        string? logEntryId = null,
        CancellationToken cancellationToken = default)
    {
        var medicine = await repository.GetMedicineAsync(medicineId, cancellationToken)
            ?? throw new InvalidOperationException("Medicine was not found.");

        var current = await repository.CalculateCurrentStockAsync(medicineId, cancellationToken) ?? medicine.StockCount ?? 0m;
        if (current + delta < 0)
        {
            throw new InvalidOperationException("This entry would make the estimated stock negative. Check the actual supply and enter a manual correction.");
        }

        await repository.SaveStockAdjustmentAsync(new StockAdjustment
        {
            MedicineId = medicineId,
            QuantityDelta = delta,
            EventUtc = timeProvider.GetUtcNow().UtcDateTime,
            Reason = reason,
            MedicationLogEntryId = logEntryId
        }, cancellationToken);
    }

    private async Task<Exception?> TryRestoreReminderRequestsAsync()
    {
        try
        {
            await reminders.RebuildAsync(cancellationToken: CancellationToken.None);
            return null;
        }
        catch (Exception recoveryFailure)
        {
            return recoveryFailure;
        }
    }
}
