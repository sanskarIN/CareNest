using CareNest.App.ViewModels;

namespace CareNest.App.Views;

public partial class LockPage : ContentPage
{
    public LockPage(LockViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
