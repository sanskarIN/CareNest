using System.Windows.Input;
using CareNest.App.Services;
using CareNest.Shared;

namespace CareNest.App.ViewModels;

public sealed class AboutViewModel : ObservableViewModel
{
    private readonly CareNest.Application.Contracts.IAppFileGateway _files;

    public AboutViewModel(
        CareNest.Application.Contracts.IAppFileGateway files,
        SafeUiErrorService errors) : base(errors)
    {
        _files = files;
        OpenRepositoryCommand = new AsyncCommand(() => OpenAsync(AppConstants.RepositoryUrl));
        OpenCreatorCommand = new AsyncCommand(() => OpenAsync(AppConstants.CreatorUrl));
        SupportProjectCommand = new AsyncCommand(() => OpenAsync(AppConstants.FundingUrl));
        BusinessEmailCommand = new AsyncCommand(() => OpenAsync($"mailto:{AppConstants.BusinessEmail}"));
        SupportEmailCommand = new AsyncCommand(() => OpenAsync($"mailto:{AppConstants.SupportEmail}"));
        PrivacyCommand = new AsyncCommand(() => OpenAsync($"{AppConstants.RepositoryUrl}/blob/main/PRIVACY.md"));
        TermsCommand = new AsyncCommand(() => OpenAsync($"{AppConstants.RepositoryUrl}/blob/main/TERMS.md"));
        SecurityCommand = new AsyncCommand(() => OpenAsync($"{AppConstants.RepositoryUrl}/blob/main/SECURITY.md"));
        ThirdPartyNoticesCommand = new AsyncCommand(ShowThirdPartyNoticesAsync);
    }

    public string Version => AppInfo.Current.VersionString;
    public string Build => AppInfo.Current.BuildString;
    public string Platform => $"{DeviceInfo.Current.Platform} {DeviceInfo.Current.VersionString}";

    public ICommand OpenRepositoryCommand { get; }
    public ICommand OpenCreatorCommand { get; }
    public ICommand SupportProjectCommand { get; }
    public ICommand BusinessEmailCommand { get; }
    public ICommand SupportEmailCommand { get; }
    public ICommand PrivacyCommand { get; }
    public ICommand TermsCommand { get; }
    public ICommand SecurityCommand { get; }
    public ICommand ThirdPartyNoticesCommand { get; }

    private Task ShowThirdPartyNoticesAsync() =>
        RunAsync(async ct =>
        {
            await using var stream = await FileSystem.Current.OpenAppPackageFileAsync("third_party_notices.txt");
            using var reader = new StreamReader(stream);
            var text = await reader.ReadToEndAsync(ct);
            await _files.ShareTextAsync(text, "CareNest third-party notices", ct);
        }, "CareNest could not open the bundled third-party notices.");

    private Task OpenAsync(string uri) =>
        RunAsync(
            async ct =>
            {
                ct.ThrowIfCancellationRequested();
                if (!await Launcher.Default.TryOpenAsync(new Uri(uri)))
                {
                    throw new InvalidOperationException("No application is available to open this link.");
                }
            },
            "CareNest could not open the requested link.");
}
