using CareNest.App.ViewModels;

namespace CareNest.App.Views;

public partial class DocumentsPage : ContentPage
{
    private readonly DocumentsViewModel _viewModel;

    public DocumentsPage(DocumentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }

    private async void DeleteClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not DocumentRow row)
        {
            return;
        }

        var confirmed = await DisplayAlertAsync(
            "Delete encrypted document?",
            "This removes the CareNest document record and its encrypted local file. Exported copies and backups elsewhere are not removed.",
            "Delete",
            "Cancel");

        if (confirmed)
        {
            await _viewModel.DeleteAsync(row);
        }
    }

    private async void TagsClicked(object? sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.CommandParameter is not DocumentRow row)
        {
            return;
        }

        var tags = await DisplayPromptAsync(
            "Document tags",
            "Enter comma-separated local tags.",
            initialValue: row.Tags,
            maxLength: 500);

        if (tags is not null)
        {
            await _viewModel.SetTagsAsync(
                row,
                tags);
        }
    }
}
