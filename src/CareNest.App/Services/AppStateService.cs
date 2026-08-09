using CareNest.Application.Contracts;
using CareNest.Domain.Enums;
using CareNest.Shared;

namespace CareNest.App.Services;

public sealed class AppStateService(ICareNestRepository repository)
{
    public async Task<bool> IsOnboardingCompleteAsync(
        CancellationToken cancellationToken = default) =>
        string.Equals(
            await repository.GetSettingAsync(SettingKeys.OnboardingComplete, cancellationToken),
            "1",
            StringComparison.Ordinal);

    public Task SetOnboardingCompleteAsync(CancellationToken cancellationToken = default) =>
        repository.SetSettingAsync(SettingKeys.OnboardingComplete, "1", cancellationToken);

    public async Task<ThemePreference> GetThemeAsync(CancellationToken cancellationToken = default)
    {
        var value = await repository.GetSettingAsync(SettingKeys.Theme, cancellationToken);
        return Enum.TryParse<ThemePreference>(value, true, out var parsed)
            ? parsed
            : ThemePreference.System;
    }

    public async Task SetThemeAsync(ThemePreference theme, CancellationToken cancellationToken = default)
    {
        await repository.SetSettingAsync(SettingKeys.Theme, theme.ToString(), cancellationToken);
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            if (Microsoft.Maui.Controls.Application.Current is { } app)
            {
                app.UserAppTheme = theme switch
                {
                    ThemePreference.Light => AppTheme.Light,
                    ThemePreference.Dark => AppTheme.Dark,
                    _ => AppTheme.Unspecified
                };
            }
        });
    }

    public async Task<bool> GetBoolAsync(string key, bool defaultValue, CancellationToken cancellationToken = default)
    {
        var value = await repository.GetSettingAsync(key, cancellationToken);
        return value is null ? defaultValue : string.Equals(value, "1", StringComparison.Ordinal);
    }

    public Task SetBoolAsync(string key, bool value, CancellationToken cancellationToken = default) =>
        repository.SetSettingAsync(key, value ? "1" : "0", cancellationToken);

    public async Task SetLargeInterfaceAsync(bool enabled, CancellationToken cancellationToken = default)
    {
        await SetBoolAsync(SettingKeys.LargeInterface, enabled, cancellationToken);
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            if (Microsoft.Maui.Controls.Application.Current?.Resources is not { } resources)
            {
                return;
            }

            resources["BodyFontSize"] = enabled ? 19d : 16d;
            resources["PageTitleFontSize"] = enabled ? 32d : 28d;
            resources["SectionTitleFontSize"] = enabled ? 23d : 20d;
            resources["MutedFontSize"] = enabled ? 15d : 13d;
        });
    }

}
