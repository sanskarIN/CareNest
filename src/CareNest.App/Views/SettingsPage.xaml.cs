using CareNest.App.ViewModels;

namespace CareNest.App.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.LoadAsync();
    }

    private async void EnableLockClicked(object? sender, EventArgs e)
    {
        var pin = await DisplayPromptAsync(
            "Enable app lock",
            "Enter a numeric PIN containing 6–32 digits.",
            "Enable",
            "Cancel",
            maxLength: 32,
            keyboard: Keyboard.Numeric);
        if (!string.IsNullOrEmpty(pin))
        {
            await _viewModel.EnableAppLockAsync(pin);
        }
    }

    private async void DisableLockClicked(object? sender, EventArgs e)
    {
        var pin = await DisplayPromptAsync(
            "Disable app lock",
            "Enter the current PIN.",
            "Disable",
            "Cancel",
            maxLength: 32,
            keyboard: Keyboard.Numeric);
        if (!string.IsNullOrEmpty(pin))
        {
            await _viewModel.DisableAppLockAsync(pin);
        }
    }

    private async void CreateBackupClicked(object? sender, EventArgs e)
    {
        var password = await DisplayPromptAsync(
            "Encrypt backup",
            "Choose a password of at least 10 characters. CareNest cannot recover it.",
            "Create",
            "Cancel",
            maxLength: 128);
        if (!string.IsNullOrEmpty(password))
        {
            await _viewModel.CreateBackupAsync(password);
        }
    }

    private async void RestoreBackupClicked(object? sender, EventArgs e)
    {
        var confirmed = await DisplayAlertAsync(
            "Restore backup?",
            "A validated restore replaces current local CareNest data. Consider creating a backup first.",
            "Continue",
            "Cancel");
        if (!confirmed) return;

        var password = await DisplayPromptAsync(
            "Backup password",
            "Enter the password used to create this backup.",
            "Restore",
            "Cancel",
            maxLength: 128);
        if (!string.IsNullOrEmpty(password))
        {
            await _viewModel.RestoreBackupAsync(password);
        }
    }

    private async void ResetDataClicked(object? sender, EventArgs e)
    {
        var first = await DisplayAlertAsync(
            "Delete all local CareNest data?",
            "This permanently deletes profiles, medicines, schedules, logs, appointments, documents, settings, and app-lock data from this device. Exported files and backups elsewhere are not deleted.",
            "Continue",
            "Cancel");
        if (!first) return;

        var second = await DisplayAlertAsync(
            "Final confirmation",
            "This action cannot be undone from CareNest unless you have a valid backup.",
            "Delete everything",
            "Cancel");
        if (second)
        {
            await _viewModel.ResetAllDataAsync();
        }
    }
}
