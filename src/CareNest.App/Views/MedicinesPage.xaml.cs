using CareNest.App.ViewModels;

namespace CareNest.App.Views;

public partial class MedicinesPage : ContentPage
{
    private readonly MedicinesViewModel _viewModel;

    public MedicinesPage(MedicinesViewModel viewModel)
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
