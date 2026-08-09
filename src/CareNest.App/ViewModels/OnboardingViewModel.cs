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

            await _profiles.SaveAsync(new PersonProfile
            {
                Name = ProfileName.Trim(),
                IsPrimary = true
            }, ct);

            if (EnableLock)
            {
                await _appLock.SetPinAsync(Pin, ct);
            }

            await _appState.SetOnboardingCompleteAsync(ct);
            await _appState.SetBoolAsync(
                SettingKeys.GenericNotificationLabels,
                true,
                ct);

            Pin = string.Empty;
            await _navigator.ResetToShellAsync(ct);
        },
        "CareNest could not complete onboarding.");
}
