using CareNest.App.ViewModels;

namespace CareNest.App.Views;

public partial class ScheduleEditorPage : ContentPage, IQueryAttributable
{
    private readonly ScheduleEditorViewModel _viewModel;

    public ScheduleEditorPage(ScheduleEditorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("MedicineId", out var value) &&
            value is not null)
        {
            _ = _viewModel.LoadAsync(value.ToString()!);
        }
    }
}
