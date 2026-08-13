using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.Domain.Rules;
using CareNest.Shared;

namespace CareNest.Application.Services;

public sealed class AppointmentService(
    ICareNestRepository repository,
    INotificationService notifications,
    TimeProvider timeProvider) : IAppointmentService
{
    public Task<IReadOnlyList<Appointment>> ListAsync(
        string? profileId = null,
        CancellationToken cancellationToken = default) =>
        repository.GetAppointmentsAsync(
            profileId,
            false,
            cancellationToken);

    public async Task SaveAsync(
        Appointment appointment,
        CancellationToken cancellationToken = default)
    {
        AppointmentRules.Validate(appointment);
        var exists = await repository.GetAppointmentAsync(
            appointment.Id,
            cancellationToken) is not null;

        appointment.Touch(
            timeProvider.GetUtcNow().UtcDateTime);

        await repository.SaveAppointmentAsync(
            appointment,
            cancellationToken);

        await repository.AddAuditEntryAsync(new AuditEntry
        {
            EntityType = nameof(Appointment),
            EntityId = appointment.Id,
            Action = exists
                ? AuditAction.Updated
                : AuditAction.Created,
            EventUtc = timeProvider.GetUtcNow().UtcDateTime,
            SafeSummary = exists
                ? "Appointment updated"
                : "Appointment created"
        }, cancellationToken);

        await ScheduleReminderIfNeededAsync(
            appointment,
            requestPermission: true,
            cancellationToken);
    }

    public async Task DeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        await notifications.CancelAsync(
            NotificationId(id),
            cancellationToken);

        await repository.DeleteAppointmentAsync(
            id,
            cancellationToken);
    }

    public async Task RebuildRemindersAsync(
        CancellationToken cancellationToken = default)
    {
        var appointments = await repository.GetAppointmentsAsync(
            null,
            false,
            cancellationToken);

        foreach (var appointment in appointments)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await ScheduleReminderIfNeededAsync(
                appointment,
                requestPermission: false,
                cancellationToken);
        }
    }

    private async Task ScheduleReminderIfNeededAsync(
        Appointment appointment,
        bool requestPermission,
        CancellationToken cancellationToken)
    {
        await notifications.CancelAsync(
            NotificationId(appointment.Id),
            cancellationToken);

        if (appointment.ReminderMinutesBefore is null ||
            appointment.Archived)
        {
            return;
        }

        if (appointment.StartsUtc.Kind != DateTimeKind.Utc)
        {
            throw new InvalidOperationException("Stored appointment start time must be UTC before reminder scheduling.");
        }

        var due = appointment.StartsUtc
            .AddMinutes(-appointment.ReminderMinutesBefore.Value);

        if (due <= timeProvider.GetUtcNow().UtcDateTime)
        {
            return;
        }

        var diagnostics = await notifications.GetDiagnosticsAsync(
            cancellationToken);

        if (!diagnostics.PermissionGranted &&
            requestPermission)
        {
            _ = await notifications.RequestPermissionAsync(
                cancellationToken);
        }

        var policy = await LoadPolicyAsync(cancellationToken);
        if (IsInsideQuietHours(due, policy))
        {
            return;
        }

        await notifications.ScheduleAsync(
            new NotificationRequest(
                NotificationId(appointment.Id),
                due,
                "CareNest appointment reminder",
                policy.GenericLabels
                    ? "Open CareNest to review your upcoming appointment."
                    : "An appointment reminder is due. Open CareNest for user-entered details.",
                policy.Persistent,
                "appointment",
                policy.PlaySound,
                policy.Vibrate),
            cancellationToken);
    }

    private async Task<NotificationPolicy> LoadPolicyAsync(CancellationToken cancellationToken)
    {
        var quiet = string.Equals(
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
        _ = TimeOnly.TryParse(
            await repository.GetSettingAsync(SettingKeys.QuietHoursStart, cancellationToken),
            out var start);
        _ = TimeOnly.TryParse(
            await repository.GetSettingAsync(SettingKeys.QuietHoursEnd, cancellationToken),
            out var end);
        return new NotificationPolicy(quiet, start, end, generic, persistent, playSound, vibrate);
    }

    private static bool IsInsideQuietHours(DateTime dueUtc, NotificationPolicy policy)
    {
        if (!policy.Quiet || policy.Start == policy.End)
        {
            return false;
        }

        var local = TimeOnly.FromDateTime(dueUtc.ToLocalTime());
        return policy.Start < policy.End
            ? local >= policy.Start && local < policy.End
            : local >= policy.Start || local < policy.End;
    }

    private sealed record NotificationPolicy(
        bool Quiet,
        TimeOnly Start,
        TimeOnly End,
        bool GenericLabels,
        bool Persistent,
        bool PlaySound,
        bool Vibrate);

    private static string NotificationId(string appointmentId) =>
        $"appointment-{appointmentId}";
}
