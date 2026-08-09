using CareNest.App.ViewModels;

namespace CareNest.App.Views;

public partial class AppointmentEditorPage : ContentPage, IQueryAttributable
{
    private readonly AppointmentEditorViewModel _viewModel;

    public AppointmentEditorPage(AppointmentEditorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = query.TryGetValue("AppointmentId", out var value)
            ? value?.ToString()
            : null;

        _ = _viewModel.LoadAsync(id);
    }

    private async void DeleteClicked(object? sender, EventArgs e)
    {
        var confirmed = await DisplayAlertAsync(
            "Delete appointment?",
            "This removes the appointment from CareNest on this device and cancels its CareNest reminder.",
            "Delete",
            "Cancel");

        if (confirmed)
        {
            await _viewModel.DeleteAsync();
        }
    }
}
