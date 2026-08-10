using CareNest.App.Services;

namespace CareNest.App;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services, GlobalExceptionHandler globalExceptions)
    {
        InitializeComponent();
        _services = services;
        globalExceptions.Attach();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var startup = _services.GetRequiredService<Views.StartupPage>();
        return new Window(startup)
        {
            Title = Shared.AppConstants.ProductName
        };
    }
}
