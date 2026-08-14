using CareNest.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace CareNest.App.Services;

public enum StartupDestination
{
    Onboarding,
    Lock,
    Shell
}

public sealed class StartupCoordinator(
    ICareNestRepository repository,
    IReminderCoordinator reminders,
    IAppointmentService appointments,
    CareNest.Application.Services.BackupReminderCoordinator backupReminder,
    IAppLockService appLock,
    AppStateService appState,
    ILogger<StartupCoordinator> logger)
{
    public async Task<StartupDestination> InitializeAsync(
        CancellationToken cancellationToken = default)
    {
        await repository.InitializeAsync(cancellationToken);

        var theme = await appState.GetThemeAsync(cancellationToken);
        await appState.SetThemeAsync(theme, cancellationToken);
        var largeInterface = await appState.GetBoolAsync(CareNest.Shared.SettingKeys.LargeInterface, false, cancellationToken);
        await appState.SetLargeInterfaceAsync(largeInterface, cancellationToken);

        await RunRecoveryStepAsync(
            "overdue-reminder-reconciliation",
            () => reminders.MarkOverdueAsMissedAsync(cancellationToken));
        await RunRecoveryStepAsync(
            "medicine-reminder-rebuild",
            () => reminders.RebuildAsync(cancellationToken: cancellationToken));
        await RunRecoveryStepAsync(
            "appointment-reminder-rebuild",
            () => appointments.RebuildRemindersAsync(cancellationToken));
        await RunRecoveryStepAsync(
            "backup-reminder-sync",
            () => backupReminder.SyncAsync(requestPermission: false, cancellationToken: cancellationToken));

        if (!await appState.IsOnboardingCompleteAsync(cancellationToken))
        {
            return StartupDestination.Onboarding;
        }

        return await appLock.IsEnabledAsync(cancellationToken)
            ? StartupDestination.Lock
            : StartupDestination.Shell;
    }

    private async Task RunRecoveryStepAsync(
        string step,
        Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                var exceptionType = ex.GetType().FullName ?? "Unknown";
                logger.LogWarning(
                    "Startup recovery step failed non-fatally. Step={Step}; ExceptionType={ExceptionType}. Health record identifiers and exception details were not logged.",
                    step,
                    exceptionType);
            }
        }
    }
}
