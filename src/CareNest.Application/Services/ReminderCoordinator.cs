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
        if (now.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("Reminder rebuild start must be UTC.", nameof(fromUtc));
        }

        var horizon = now.AddDays(AppConstants.ReminderHorizonDays);
        var plannedKeys = new HashSet<string>(StringComparer.Ordinal);

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
            foreach (var occurrence in occurrences)
            {
                plannedKeys.Add(occurrence.OccurrenceKey);
            }

            await repository.UpsertOccurrencesAsync(occurrences, cancellationToken);
        }

        var actionable = await GetActionableOccurrencesAsync(now, horizon, null, cancellationToken);
        var policy = await LoadNotificationPolicyAsync(cancellationToken);
        foreach (var occurrence in actionable)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var dueUtc = EffectiveDueUtc(occurrence);
            var historicalSnooze =
                occurrence.State == ReminderState.Snoozed && occurrence.SnoozedUntilUtc is not null &&
                occurrence.ScheduledUtc < now;
            var valid = historicalSnooze || plannedKeys.Contains(occurrence.OccurrenceKey);
            var hadPlatformRequest = !string.IsNullOrWhiteSpace(occurrence.PlatformNotificationId);

            if (!await TryCancelPlatformRequestAsync(occurrence, cancellationToken))
            {
                continue;
            }

            if (hadPlatformRequest)
            {
                await repository.SaveOccurrenceAsync(occurrence, cancellationToken);
            }

            if (!valid)
            {
                occurrence.State = ReminderState.Cancelled;
                occurrence.StateChangedUtc = now;
                occurrence.SnoozedUntilUtc = null;
                occurrence.PlatformNotificationId = null;
                await repository.SaveOccurrenceAsync(occurrence, cancellationToken);
                continue;
            }

            if (IsInsideQuietHours(dueUtc, policy))
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
                LogOperationalWarning("Reminder scheduling failed", ex);
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
        if (newState is not (
            ReminderState.Snoozed or
            ReminderState.Taken or
            ReminderState.Skipped or
            ReminderState.Delayed or
            ReminderState.Missed or
            ReminderState.Cancelled))
        {
            throw new ArgumentOutOfRangeException(
                nameof(newState),
                newState,
                "Reminder actions must be snoozed, taken, skipped, delayed, missed, or cancelled.");
        }

        var occurrence = await repository.GetOccurrenceAsync(occurrenceId, cancellationToken)
            ?? throw new InvalidOperationException("Reminder occurrence was not found.");
        var now = timeProvider.GetUtcNow().UtcDateTime;

        if (newState == ReminderState.Snoozed)
        {
            if (snoozedUntilUtc is null)
            {
                throw new ArgumentException("Snooze requires an explicit future time.", nameof(snoozedUntilUtc));
            }

            if (snoozedUntilUtc.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Snooze time must be UTC.", nameof(snoozedUntilUtc));
            }

            if (snoozedUntilUtc.Value <= now)
            {
                throw new ArgumentOutOfRangeException(nameof(snoozedUntilUtc), "Snooze time must be in the future.");
            }
        }

        var previousState = occurrence.State;
        var previousStateChangedUtc = occurrence.StateChangedUtc;
        var previousSnoozedUntilUtc = occurrence.SnoozedUntilUtc;

        // Do not persist a handled state while the old OS request can still fire.
        await notificationService.CancelAsync(occurrenceId, cancellationToken);

        try
        {
            occurrence.State = newState;
            occurrence.StateChangedUtc = now;
            occurrence.SnoozedUntilUtc = newState == ReminderState.Snoozed ? snoozedUntilUtc : null;
            occurrence.PlatformNotificationId = null;
            await repository.SaveOccurrenceAsync(occurrence, cancellationToken);

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
                    occurrence.PlatformNotificationId = occurrence.Id;
                    await repository.SaveOccurrenceAsync(occurrence, cancellationToken);
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
                    try
                    {
                        await ApplyUserConfiguredStockChangeAsync(
                            occurrence.MedicineId,
                            logEntry.Id,
                            cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        LogOperationalWarning("Taken-event stock adjustment failed", ex);
                    }
                }
            }
        }
        catch (Exception primaryFailure)
        {
            try
            {
                occurrence.State = previousState;
                occurrence.StateChangedUtc = previousStateChangedUtc;
                occurrence.SnoozedUntilUtc = previousSnoozedUntilUtc;
                occurrence.PlatformNotificationId = null;
                await repository.SaveOccurrenceAsync(occurrence, CancellationToken.None);
                await RebuildAsync(cancellationToken: CancellationToken.None);
            }
            catch (Exception recoveryFailure)
            {
                throw new AggregateException(
                    "Reminder action failed and the previous reminder request could not be fully restored.",
                    primaryFailure,
                    recoveryFailure);
            }

            throw;
        }

        try
        {
            await repository.AddAuditEntryAsync(new AuditEntry
            {
                EntityType = nameof(ReminderOccurrence),
                EntityId = occurrence.Id,
                Action = AuditAction.ReminderStateChanged,
                EventUtc = timeProvider.GetUtcNow().UtcDateTime,
                ChangedFieldsCsv = "State",
                SafeSummary = $"Reminder state changed to {newState}"
            }, CancellationToken.None);
        }
        catch (Exception ex)
        {
            LogOperationalWarning("Reminder action audit write failed", ex);
        }
    }

    public async Task MarkOverdueAsMissedAsync(CancellationToken cancellationToken = default)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var cutoff = now.AddMinutes(-5);
        var overdue = await GetActionableOccurrencesAsync(
            now.AddDays(-AppConstants.ReminderHorizonDays),
            cutoff,
            null,
            cancellationToken);

        foreach (var occurrence in overdue)
        {
            await HandleOccurrenceAsync(
                occurrence.Id,
                ReminderState.Missed,
                note: null,
                cancellationToken: cancellationToken);
        }
    }

    public async Task<IReadOnlyList<ReminderPreview>> GetUpcomingAsync(
        string? profileId,
        int take = 20,
        CancellationToken cancellationToken = default)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var horizon = now.AddDays(AppConstants.ReminderHorizonDays);
        var occurrences = await GetActionableOccurrencesAsync(now, horizon, profileId, cancellationToken);
        var medicines = (await repository.GetMedicinesAsync(profileId, true, cancellationToken)).ToDictionary(x => x.Id);
        var profiles = (await repository.GetProfilesAsync(true, cancellationToken)).ToDictionary(x => x.Id);

        return occurrences
            .OrderBy(EffectiveDueUtc)
            .Take(Math.Clamp(take, 1, 100))
            .Select(x => new ReminderPreview(
                x.Id,
                x.MedicineId,
                medicines.TryGetValue(x.MedicineId, out var medicine) ? medicine.Name : "Medicine",
                x.ProfileId,
                profiles.TryGetValue(x.ProfileId, out var profile) ? profile.Name : "Profile",
                EffectiveDueUtc(x),
                x.LocalScheduledTime,
                x.TimeZoneId,
                x.State))
            .ToArray();
    }

    public Task CancelFutureForMedicineAsync(
        string medicineId,
        CancellationToken cancellationToken = default) =>
        CancelFutureAsync(
            occurrence => string.Equals(occurrence.MedicineId, medicineId, StringComparison.Ordinal),
            cancellationToken);

    public Task CancelFutureForProfileAsync(
        string profileId,
        CancellationToken cancellationToken = default) =>
        CancelFutureAsync(
            occurrence => string.Equals(occurrence.ProfileId, profileId, StringComparison.Ordinal),
            cancellationToken);

    private async Task CancelFutureAsync(
        Func<ReminderOccurrence, bool> predicate,
        CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var horizon = now.AddDays(AppConstants.ReminderHorizonDays);
        var actionable = await GetActionableOccurrencesAsync(now, horizon, null, cancellationToken);

        foreach (var occurrence in actionable.Where(predicate))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var hadPlatformRequest = !string.IsNullOrWhiteSpace(occurrence.PlatformNotificationId);
            if (!await TryCancelPlatformRequestAsync(occurrence, cancellationToken))
            {
                throw new InvalidOperationException(
                    "CareNest could not reconcile one or more scheduled reminder requests.");
            }

            if (hadPlatformRequest)
            {
                await repository.SaveOccurrenceAsync(occurrence, cancellationToken);
            }
        }
    }

    private async Task<IReadOnlyList<ReminderOccurrence>> GetActionableOccurrencesAsync(
        DateTime fromUtc,
        DateTime toUtc,
        string? profileId,
        CancellationToken cancellationToken)
    {
        if (fromUtc.Kind != DateTimeKind.Utc || toUtc.Kind != DateTimeKind.Utc || toUtc <= fromUtc)
        {
            throw new ArgumentException("Reminder query bounds must be an increasing UTC range.");
        }

        var lookback = fromUtc.AddDays(-AppConstants.ReminderHorizonDays);
        var rows = await repository.GetOccurrencesAsync(lookback, toUtc, profileId, cancellationToken);
        return rows
            .Where(occurrence => occurrence.State is ReminderState.Scheduled or ReminderState.Snoozed)
            .Where(occurrence =>
            {
                var dueUtc = EffectiveDueUtc(occurrence);
                return dueUtc >= fromUtc && dueUtc < toUtc;
            })
            .ToArray();
    }

    private static DateTime EffectiveDueUtc(ReminderOccurrence occurrence) =>
        occurrence.State == ReminderState.Snoozed && occurrence.SnoozedUntilUtc is DateTime snoozedUntilUtc
            ? snoozedUntilUtc
            : occurrence.ScheduledUtc;

    private async Task<bool> TryCancelPlatformRequestAsync(
        ReminderOccurrence occurrence,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(occurrence.PlatformNotificationId))
        {
            return true;
        }

        try
        {
            await notificationService.CancelAsync(occurrence.Id, cancellationToken);
            occurrence.PlatformNotificationId = null;
            return true;
        }
        catch (Exception ex)
        {
            LogOperationalWarning("Reminder cancellation failed", ex);
            return false;
        }
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
                LogOperationalWarning("Low-stock reminder scheduling failed", ex);
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

    private void LogOperationalWarning(string operation, Exception ex)
    {
        if (!logger.IsEnabled(LogLevel.Warning))
        {
            return;
        }

        var exceptionType = ex.GetType().FullName ?? "Unknown";
        logger.LogWarning(
            "{Operation}. ExceptionType={ExceptionType}. Health record identifiers and exception details were not logged.",
            operation,
            exceptionType);
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
