using CareNest.App.ViewModels;

namespace CareNest.App.Views;

public partial class AppointmentsPage : ContentPage
{
    private readonly AppointmentsViewModel _viewModel;

    public AppointmentsPage(AppointmentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }
}
