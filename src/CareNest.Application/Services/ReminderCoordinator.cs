using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Shared;
using Microsoft.Extensions.Logging;

namespace CareNest.Application.Services;

public sealed class ReminderCoordinator(
    ICareNestRepository repository,
    INotificationService notificationService,
    ReminderPlanner planner,
    TimeProvider timeProvider,
    ILogger<ReminderCoordinator> logger) : IReminderCoordinator
{
    public async Task RebuildAsync(DateTime? fromUtc = null, CancellationToken cancellationToken = default)
    {
        var now = fromUtc ?? timeProvider.GetUtcNow().UtcDateTime;
        var horizon = now.AddDays(AppConstants.ReminderHorizonDays);

        var schedules = await repository.GetEnabledSchedulesAsync(cancellationToken);
        foreach (var schedule in schedules)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var medicine = await repository.GetMedicineAsync(schedule.MedicineId, cancellationToken);
            if (medicine is null || medicine.State != MedicineState.Active)
            {
                continue;
            }

            var profile = await repository.GetProfileAsync(medicine.ProfileId, cancellationToken);
            if (profile is null || profile.IsArchived)
            {
                continue;
            }

            var times = await repository.GetScheduleTimesAsync(schedule.Id, cancellationToken);
            var occurrences = planner.BuildOccurrences(medicine, schedule, times, profile, now, horizon);
            await repository.UpsertOccurrencesAsync(occurrences, cancellationToken);
        }

        var future = await repository.GetOccurrencesAsync(now, horizon, cancellationToken: cancellationToken);
        var policy = await LoadNotificationPolicyAsync(cancellationToken);
        foreach (var occurrence in future.Where(x => x.State is ReminderState.Scheduled or ReminderState.Snoozed))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var dueUtc = occurrence.SnoozedUntilUtc ?? occurrence.ScheduledUtc;
            if (dueUtc <= now || IsInsideQuietHours(dueUtc, policy))
            {
                continue;
            }

            try
            {
                await notificationService.ScheduleAsync(new NotificationRequest(
                    occurrence.Id,
                    dueUtc,
                    "CareNest reminder",
                    policy.GenericLabels
                        ? "Open CareNest to review your scheduled reminder."
                        : "A medicine reminder is due. Open CareNest for the user-entered details.",
                    policy.Persistent,
                    occurrence.FollowUp ? "follow-up" : "medicine",
                    policy.PlaySound,
                    policy.Vibrate), cancellationToken);

                occurrence.PlatformNotificationId = occurrence.Id;
                await repository.SaveOccurrenceAsync(occurrence, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Reminder scheduling failed for occurrence {OccurrenceId}. No health details were logged.", occurrence.Id);
            }
        }
    }

    public async Task HandleOccurrenceAsync(
        string occurrenceId,
        ReminderState newState,
        DateTime? snoozedUntilUtc = null,
        string? note = null,
        CancellationToken cancellationToken = default)
    {
        var occurrence = await repository.GetOccurrenceAsync(occurrenceId, cancellationToken)
            ?? throw new InvalidOperationException("Reminder occurrence was not found.");

        if (newState == ReminderState.Snoozed && snoozedUntilUtc is null)
        {
            throw new ArgumentException("Snooze requires an explicit future time.", nameof(snoozedUntilUtc));
        }

        occurrence.State = newState;
        occurrence.StateChangedUtc = timeProvider.GetUtcNow().UtcDateTime;
        occurrence.SnoozedUntilUtc = newState == ReminderState.Snoozed ? snoozedUntilUtc : null;
        await repository.SaveOccurrenceAsync(occurrence, cancellationToken);

        await notificationService.CancelAsync(occurrenceId, cancellationToken);
        if (newState == ReminderState.Snoozed)
        {
            var policy = await LoadNotificationPolicyAsync(cancellationToken);
            if (!IsInsideQuietHours(snoozedUntilUtc!.Value, policy))
            {
                await notificationService.ScheduleAsync(new NotificationRequest(
                    occurrence.Id,
                    snoozedUntilUtc.Value,
                    "CareNest reminder",
                    policy.GenericLabels
                        ? "Open CareNest to review your snoozed reminder."
                        : "A snoozed medicine reminder is due. Open CareNest for details.",
                    policy.Persistent,
                    "medicine",
                    policy.PlaySound,
                    policy.Vibrate), cancellationToken);
            }
        }

        if (newState is ReminderState.Taken or ReminderState.Skipped or ReminderState.Delayed or ReminderState.Missed)
        {
            var status = newState switch
            {
                ReminderState.Taken => MedicationLogStatus.Taken,
                ReminderState.Skipped => MedicationLogStatus.Skipped,
                ReminderState.Delayed => MedicationLogStatus.Delayed,
                _ => MedicationLogStatus.Missed
            };

            var logEntry = new MedicationLogEntry
            {
                ProfileId = occurrence.ProfileId,
                MedicineId = occurrence.MedicineId,
                ReminderOccurrenceId = occurrence.Id,
                Status = status,
                EventUtc = timeProvider.GetUtcNow().UtcDateTime,
                Note = note
            };
            await repository.SaveMedicationLogEntryAsync(logEntry, cancellationToken);

            if (newState == ReminderState.Taken)
            {
                await ApplyUserConfiguredStockChangeAsync(occurrence.MedicineId, logEntry.Id, cancellationToken);
            }
        }

        await repository.AddAuditEntryAsync(new AuditEntry
        {
            EntityType = nameof(ReminderOccurrence),
            EntityId = occurrence.Id,
            Action = AuditAction.ReminderStateChanged,
            EventUtc = timeProvider.GetUtcNow().UtcDateTime,
            ChangedFieldsCsv = "State",
            SafeSummary = $"Reminder state changed to {newState}"
        }, cancellationToken);
    }

    public async Task MarkOverdueAsMissedAsync(CancellationToken cancellationToken = default)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var overdue = await repository.GetOccurrencesAsync(now.AddDays(-7), now.AddMinutes(-5), cancellationToken: cancellationToken);
        foreach (var occurrence in overdue.Where(x => x.State == ReminderState.Scheduled))
        {
            await HandleOccurrenceAsync(
                occurrence.Id,
                ReminderState.Missed,
                note: null,
                cancellationToken: cancellationToken);
        }
    }

    public async Task<IReadOnlyList<ReminderPreview>> GetUpcomingAsync(string? profileId, int take = 20, CancellationToken cancellationToken = default)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var occurrences = await repository.GetOccurrencesAsync(now, now.AddDays(AppConstants.ReminderHorizonDays), profileId, cancellationToken);
        var medicines = (await repository.GetMedicinesAsync(profileId, true, cancellationToken)).ToDictionary(x => x.Id);
        var profiles = (await repository.GetProfilesAsync(true, cancellationToken)).ToDictionary(x => x.Id);

        return occurrences
            .Where(x => x.State is ReminderState.Scheduled or ReminderState.Snoozed)
            .OrderBy(x => x.SnoozedUntilUtc ?? x.ScheduledUtc)
            .Take(Math.Clamp(take, 1, 100))
            .Select(x => new ReminderPreview(
                x.Id,
                x.MedicineId,
                medicines.TryGetValue(x.MedicineId, out var medicine) ? medicine.Name : "Medicine",
                x.ProfileId,
                profiles.TryGetValue(x.ProfileId, out var profile) ? profile.Name : "Profile",
                x.SnoozedUntilUtc ?? x.ScheduledUtc,
                x.LocalScheduledTime,
                x.TimeZoneId,
                x.State))
            .ToArray();
    }


    private async Task ApplyUserConfiguredStockChangeAsync(
        string medicineId,
        string logEntryId,
        CancellationToken cancellationToken)
    {
        var medicine = await repository.GetMedicineAsync(medicineId, cancellationToken);
        if (medicine?.StockCount is null ||
            medicine.StockChangePerTakenEvent is null ||
            medicine.StockChangePerTakenEvent <= 0)
        {
            return;
        }

        var before = await repository.CalculateCurrentStockAsync(medicineId, cancellationToken);
        if (before is null || before.Value < medicine.StockChangePerTakenEvent.Value)
        {
            await repository.AddAuditEntryAsync(new AuditEntry
            {
                EntityType = nameof(Medicine),
                EntityId = medicineId,
                Action = AuditAction.Updated,
                EventUtc = timeProvider.GetUtcNow().UtcDateTime,
                ChangedFieldsCsv = "StockAdjustment",
                SafeSummary = "Automatic stock adjustment skipped because it would make the estimate negative"
            }, cancellationToken);
            return;
        }

        await repository.SaveStockAdjustmentAsync(new StockAdjustment
        {
            MedicineId = medicineId,
            QuantityDelta = -medicine.StockChangePerTakenEvent.Value,
            EventUtc = timeProvider.GetUtcNow().UtcDateTime,
            Reason = "User-configured quantity change for Taken event",
            MedicationLogEntryId = logEntryId
        }, cancellationToken);

        var after = await repository.CalculateCurrentStockAsync(medicineId, cancellationToken);
        if (after is not null &&
            medicine.RefillThreshold is not null &&
            after <= medicine.RefillThreshold &&
            before > medicine.RefillThreshold)
        {
            try
            {
                var policy = await LoadNotificationPolicyAsync(cancellationToken);
                await notificationService.ScheduleAsync(new NotificationRequest(
                    $"stock-{medicineId}",
                    timeProvider.GetUtcNow().UtcDateTime.AddSeconds(3),
                    "CareNest stock reminder",
                    policy.GenericLabels
                        ? "Open CareNest to review a stock reminder."
                        : "A user-configured low-stock threshold was reached. Check the actual supply.",
                    false,
                    "stock",
                    policy.PlaySound,
                    policy.Vibrate), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Low-stock reminder scheduling failed for medicine record {MedicineId}.", medicineId);
            }
        }
    }

    private async Task<NotificationPolicy> LoadNotificationPolicyAsync(CancellationToken cancellationToken)
    {
        var quietEnabled = string.Equals(
            await repository.GetSettingAsync(SettingKeys.QuietHoursEnabled, cancellationToken),
            "1",
            StringComparison.Ordinal);
        var generic = !string.Equals(
            await repository.GetSettingAsync(SettingKeys.GenericNotificationLabels, cancellationToken),
            "0",
            StringComparison.Ordinal);
        var persistent = string.Equals(
            await repository.GetSettingAsync(SettingKeys.PersistentNotifications, cancellationToken),
            "1",
            StringComparison.Ordinal);
        var playSound = !string.Equals(
            await repository.GetSettingAsync(SettingKeys.SoundEnabled, cancellationToken),
            "0",
            StringComparison.Ordinal);
        var vibrate = !string.Equals(
            await repository.GetSettingAsync(SettingKeys.VibrationEnabled, cancellationToken),
            "0",
            StringComparison.Ordinal);

        var startText = await repository.GetSettingAsync(SettingKeys.QuietHoursStart, cancellationToken);
        var endText = await repository.GetSettingAsync(SettingKeys.QuietHoursEnd, cancellationToken);
        _ = TimeOnly.TryParse(startText, out var start);
        _ = TimeOnly.TryParse(endText, out var end);

        return new NotificationPolicy(quietEnabled, start, end, generic, persistent, playSound, vibrate);
    }

    private static bool IsInsideQuietHours(DateTime dueUtc, NotificationPolicy policy)
    {
        if (!policy.QuietHoursEnabled || policy.Start == policy.End)
        {
            return false;
        }

        var local = TimeOnly.FromDateTime(dueUtc.ToLocalTime());
        return policy.Start < policy.End
            ? local >= policy.Start && local < policy.End
            : local >= policy.Start || local < policy.End;
    }

    private sealed record NotificationPolicy(
        bool QuietHoursEnabled,
        TimeOnly Start,
        TimeOnly End,
        bool GenericLabels,
        bool Persistent,
        bool PlaySound,
        bool Vibrate);

}
