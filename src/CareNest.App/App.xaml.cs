namespace CareNest.App;

public partial class App : Microsoft.Maui.Controls.Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;
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
