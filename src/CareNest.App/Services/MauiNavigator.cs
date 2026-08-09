using CareNest.Application.Contracts;
using CareNest.Shared;

namespace CareNest.App.Services;

public sealed class MauiNavigator(
    IServiceProvider services,
    ICareNestRepository repository) : IAppNavigator
{
    public async Task GoToAsync(
        string route,
        IDictionary<string, object>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var animate = !string.Equals(
            await repository.GetSettingAsync(SettingKeys.ReducedMotion, cancellationToken),
            "1",
            StringComparison.Ordinal);

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (Shell.Current is null) return;
            if (parameters is null)
            {
                await Shell.Current.GoToAsync(route, animate);
            }
            else
            {
                await Shell.Current.GoToAsync(route, animate, parameters);
            }
        });
    }

    public async Task GoBackAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var animate = !string.Equals(
            await repository.GetSettingAsync(SettingKeys.ReducedMotion, cancellationToken),
            "1",
            StringComparison.Ordinal);

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (Shell.Current is not null)
            {
                await Shell.Current.GoToAsync("..", animate);
            }
        });
    }

    public Task ResetToShellAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return MainThread.InvokeOnMainThreadAsync(() =>
        {
            var window = Microsoft.Maui.Controls.Application.Current?.Windows.FirstOrDefault();
            if (window is not null)
            {
                window.Page = services.GetRequiredService<Views.AppShell>();
            }
        });
    }

    public Task ResetToOnboardingAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return MainThread.InvokeOnMainThreadAsync(() =>
        {
            var window = Microsoft.Maui.Controls.Application.Current?.Windows.FirstOrDefault();
            if (window is not null)
            {
                window.Page = new NavigationPage(services.GetRequiredService<Views.OnboardingPage>());
            }
        });
    }
}
