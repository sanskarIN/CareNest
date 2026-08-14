using System.Windows.Input;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Shared;

namespace CareNest.App.ViewModels;

public sealed class OnboardingViewModel : ObservableViewModel
{
    private readonly IProfileService _profiles;
    private readonly IAppLockService _appLock;
    private readonly AppStateService _appState;
    private readonly IAppNavigator _navigator;

    private string _profileName = string.Empty;
    private bool _disclaimerAccepted;
    private bool _enableLock;
    private string _pin = string.Empty;

    public OnboardingViewModel(
        IProfileService profiles,
        IAppLockService appLock,
        AppStateService appState,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _profiles = profiles;
        _appLock = appLock;
        _appState = appState;
        _navigator = navigator;
        FinishCommand = new AsyncCommand(FinishAsync);
    }

    public string ProfileName
    {
        get => _profileName;
        set => SetProperty(ref _profileName, value);
    }

    public bool DisclaimerAccepted
    {
        get => _disclaimerAccepted;
        set => SetProperty(ref _disclaimerAccepted, value);
    }

    public bool EnableLock
    {
        get => _enableLock;
        set => SetProperty(ref _enableLock, value);
    }

    public string Pin
    {
        get => _pin;
        set => SetProperty(ref _pin, value);
    }

    public ICommand FinishCommand { get; }

    private Task FinishAsync() =>
        RunAsync(async ct =>
        {
            if (!DisclaimerAccepted)
            {
                StatusMessage = "Please confirm that you understand CareNest's medical limitations.";
                return;
            }

            if (string.IsNullOrWhiteSpace(ProfileName))
            {
                StatusMessage = "Enter a name or nickname for the primary local profile.";
                return;
            }

            if (EnableLock && !IsValidPin(Pin))
            {
                StatusMessage = "Use a numeric app-lock PIN containing 6 to 32 digits.";
                return;
            }

            var profile = new PersonProfile
            {
                Name = ProfileName.Trim(),
                IsPrimary = true
            };
            var profileAttempted = false;
            var lockEnabled = false;

            try
            {
                profileAttempted = true;
                await _profiles.SaveAsync(profile, ct);

                if (EnableLock)
                {
                    await _appLock.SetPinAsync(Pin, ct);
                    lockEnabled = true;
                }

                await _appState.SetBoolAsync(
                    SettingKeys.GenericNotificationLabels,
                    true,
                    ct);
                await _appState.SetOnboardingCompleteAsync(ct);
            }
            catch (Exception onboardingFailure)
            {
                var rollbackFailures = new List<Exception>();

                if (lockEnabled)
                {
                    try
                    {
                        await _appLock.DisableAsync(CancellationToken.None);
                    }
                    catch (Exception cleanupFailure)
                    {
                        rollbackFailures.Add(cleanupFailure);
                    }
                }

                if (profileAttempted)
                {
                    try
                    {
                        await _profiles.DeleteAsync(profile.Id, CancellationToken.None);
                    }
                    catch (Exception cleanupFailure)
                    {
                        rollbackFailures.Add(cleanupFailure);
                    }
                }

                try
                {
                    await _appState.SetBoolAsync(
                        SettingKeys.OnboardingComplete,
                        false,
                        CancellationToken.None);
                }
                catch (Exception cleanupFailure)
                {
                    rollbackFailures.Add(cleanupFailure);
                }

                if (rollbackFailures.Count > 0)
                {
                    rollbackFailures.Insert(0, onboardingFailure);
                    throw new AggregateException(
                        "Onboarding failed and the incomplete local setup could not be fully rolled back.",
                        rollbackFailures);
                }

                throw;
            }

            Pin = string.Empty;
            await _navigator.ResetToShellAsync(ct);
        },
        "CareNest could not complete onboarding.");

    private static bool IsValidPin(string pin) =>
        pin.Length is >= 6 and <= 32 && pin.All(char.IsDigit);
}
