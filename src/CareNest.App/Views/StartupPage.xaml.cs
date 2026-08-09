using CareNest.App.Services;

namespace CareNest.App.Views;

public partial class StartupPage : ContentPage
{
    private readonly StartupCoordinator _startup;
    private readonly IServiceProvider _services;
    private readonly SafeUiErrorService _errors;
    private bool _started;

    public StartupPage(
        StartupCoordinator startup,
        IServiceProvider services,
        SafeUiErrorService errors)
    {
        InitializeComponent();
        _startup = startup;
        _services = services;
        _errors = errors;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_started)
        {
            return;
        }

        _started = true;

        try
        {
            var destination = await _startup.InitializeAsync();
            Page page = destination switch
            {
                StartupDestination.Onboarding => _services.GetRequiredService<OnboardingPage>(),
                StartupDestination.Lock => _services.GetRequiredService<LockPage>(),
                _ => _services.GetRequiredService<AppShell>()
            };

            if (Window is not null)
            {
                Window.Page = page;
            }
        }
        catch (Exception ex)
        {
            await _errors.ShowAsync(
                "CareNest could not initialize local storage. Review the troubleshooting guide and try again.",
                ex);
        }
    }
}
