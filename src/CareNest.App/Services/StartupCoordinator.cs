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

        try
        {
            await reminders.MarkOverdueAsMissedAsync(cancellationToken);
            await reminders.RebuildAsync(cancellationToken: cancellationToken);
            await appointments.RebuildRemindersAsync(cancellationToken);
            await backupReminder.SyncAsync(requestPermission: false, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                var exceptionType = ex.GetType().FullName ?? "Unknown";
                logger.LogWarning(
                    "Reminder recovery encountered a non-fatal error. ExceptionType={ExceptionType}. Health record identifiers and exception details were not logged.",
                    exceptionType);
            }
        }

        if (!await appState.IsOnboardingCompleteAsync(cancellationToken))
        {
            return StartupDestination.Onboarding;
        }

        return await appLock.IsEnabledAsync(cancellationToken)
            ? StartupDestination.Lock
            : StartupDestination.Shell;
    }
}
