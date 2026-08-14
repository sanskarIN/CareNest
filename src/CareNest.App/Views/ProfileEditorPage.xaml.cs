using CareNest.App.ViewModels;

namespace CareNest.App.Views;

public partial class ProfileEditorPage : ContentPage, IQueryAttributable
{
    private readonly ProfileEditorViewModel _viewModel;

    public ProfileEditorPage(ProfileEditorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = query.TryGetValue("ProfileId", out var value) ? value?.ToString() : null;
        _ = _viewModel.LoadAsync(id);
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        try
        {
            await _viewModel.DiscardPendingPhotoAsync();
        }
        catch
        {
            // Best-effort editor cleanup must not escape an async page lifecycle callback.
        }
    }

    private async void AddContactClicked(object? sender, EventArgs e)
    {
        var name = await DisplayPromptAsync("Emergency contact", "Name", maxLength: 120);
        if (string.IsNullOrWhiteSpace(name)) return;
        var relationship = await DisplayPromptAsync("Emergency contact", "Relationship (optional)", maxLength: 80);
        var phone = await DisplayPromptAsync("Emergency contact", "Phone number (optional)", maxLength: 80, keyboard: Keyboard.Telephone);
        var notes = await DisplayPromptAsync("Emergency contact", "Notes (optional)", maxLength: 500);
        await _viewModel.AddEmergencyContactAsync(name, relationship, phone, notes);
    }

    private async void DeleteClicked(object? sender, EventArgs e)
    {
        var confirmed = await DisplayAlertAsync(
            "Delete profile?",
            "This removes this profile and its associated CareNest records and encrypted document files from this device. Exported copies and backups elsewhere are not removed.",
            "Delete",
            "Cancel");

        if (confirmed) await _viewModel.DeleteAsync();
    }
}
