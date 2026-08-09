using System.Windows.Input;
using CareNest.Application.Contracts;
using CareNest.App.Services;

namespace CareNest.App.ViewModels;

public sealed class LockViewModel : ObservableViewModel
{
    private readonly IAppLockService _lock;
    private readonly IAppNavigator _navigator;
    private string _pin = string.Empty;
    private int _failedAttempts;

    public LockViewModel(
        IAppLockService appLock,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _lock = appLock;
        _navigator = navigator;
        UnlockCommand = new AsyncCommand(UnlockAsync);
    }

    public string Pin
    {
        get => _pin;
        set => SetProperty(ref _pin, value);
    }

    public ICommand UnlockCommand { get; }

    private Task UnlockAsync() =>
        RunAsync(async ct =>
        {
            if (_failedAttempts >= 5)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
                _failedAttempts = 0;
            }

            if (await _lock.VerifyPinAsync(Pin, ct))
            {
                Pin = string.Empty;
                _failedAttempts = 0;
                await _navigator.ResetToShellAsync(ct);
                return;
            }

            _failedAttempts++;
            Pin = string.Empty;
            StatusMessage = "PIN was not accepted. Try again.";
        },
        "CareNest could not verify the app lock.");
}
