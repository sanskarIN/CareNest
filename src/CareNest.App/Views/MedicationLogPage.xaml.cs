using CareNest.App.ViewModels;
using CareNest.Domain.Enums;

namespace CareNest.App.Views;

public partial class MedicationLogPage : ContentPage
{
    private readonly MedicationLogViewModel _viewModel;

    public MedicationLogPage(MedicationLogViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }

    private async void EditLogClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not MedicationLogRow row)
        {
            return;
        }

        var options = Enum
            .GetNames<MedicationLogStatus>();

        var selected = await DisplayActionSheetAsync(
            "Set organizational status",
            "Cancel",
            null,
            options);

        if (string.IsNullOrWhiteSpace(selected) ||
            selected == "Cancel" ||
            !Enum.TryParse<MedicationLogStatus>(selected, out var status))
        {
            return;
        }

        var note = await DisplayPromptAsync(
            "Optional note",
            "Enter a user note. Leave blank for no note.",
            initialValue: row.Note ?? string.Empty,
            maxLength: 1000);

        await _viewModel.EditEntryAsync(
            row.Id,
            status,
            note);
    }

    private async void HistoryClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not MedicationLogRow row)
        {
            return;
        }

        var history = await _viewModel.GetEditHistoryAsync(row.Id);
        await DisplayAlertAsync("Edit history", history, "Close");
    }

}
