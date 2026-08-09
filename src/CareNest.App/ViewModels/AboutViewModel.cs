using System.Windows.Input;
using CareNest.App.Services;

namespace CareNest.App.ViewModels;

public sealed class AboutViewModel : ObservableViewModel
{
    private readonly CareNest.Application.Contracts.IAppFileGateway _files;

    public AboutViewModel(
        CareNest.Application.Contracts.IAppFileGateway files,
        SafeUiErrorService errors) : base(errors)
    {
        _files = files;
        OpenRepositoryCommand = new AsyncCommand(() => OpenAsync("https://github.com/sanskarIN/CareNest"));
        OpenCreatorCommand = new AsyncCommand(() => OpenAsync("https://www.github.com/sanskarIN"));
        BusinessEmailCommand = new AsyncCommand(() => OpenAsync("mailto:sanskarin@outlook.in"));
        SupportEmailCommand = new AsyncCommand(() => OpenAsync("mailto:supportramsandesh@gmail.com"));
        PrivacyCommand = new AsyncCommand(() => OpenAsync("https://github.com/sanskarIN/CareNest/blob/main/PRIVACY.md"));
        TermsCommand = new AsyncCommand(() => OpenAsync("https://github.com/sanskarIN/CareNest/blob/main/TERMS.md"));
        SecurityCommand = new AsyncCommand(() => OpenAsync("https://github.com/sanskarIN/CareNest/blob/main/SECURITY.md"));
        ThirdPartyNoticesCommand = new AsyncCommand(ShowThirdPartyNoticesAsync);
    }

    public string Version => AppInfo.Current.VersionString;
    public string Build => AppInfo.Current.BuildString;
    public string Platform => $"{DeviceInfo.Current.Platform} {DeviceInfo.Current.VersionString}";

    public ICommand OpenRepositoryCommand { get; }
    public ICommand OpenCreatorCommand { get; }
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
