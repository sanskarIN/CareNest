using CareNest.App.ViewModels;

namespace CareNest.App.Views;

public partial class MedicineEditorPage : ContentPage, IQueryAttributable
{
    private readonly MedicineEditorViewModel _viewModel;

    public MedicineEditorPage(MedicineEditorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = query.TryGetValue("MedicineId", out var value)
            ? value?.ToString()
            : null;

        _ = _viewModel.LoadAsync(id);
    }

    private async void DeleteClicked(object? sender, EventArgs e)
    {
        var confirmed = await DisplayAlertAsync(
            "Delete medicine record?",
            "This removes the medicine record, its schedules, future reminders, related CareNest medication logs, and stock adjustments on this device.",
            "Delete",
            "Cancel");

        if (confirmed)
        {
            await _viewModel.DeleteAsync();
        }
    }

    private async void StockCorrectionClicked(object? sender, EventArgs e)
    {
        var quantityText = await DisplayPromptAsync(
            "Stock correction",
            "Enter a positive or negative quantity adjustment. CareNest does not infer this value.",
            keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(quantityText))
        {
            return;
        }

        if (!decimal.TryParse(quantityText, out var delta))
        {
            await DisplayAlertAsync(
                "CareNest",
                "Enter a valid number.",
                "OK");
            return;
        }

        var reason = await DisplayPromptAsync(
            "Reason",
            "Optional note for this manual correction.");

        await _viewModel.AddStockCorrectionAsync(
            delta,
            reason);
    }
}
