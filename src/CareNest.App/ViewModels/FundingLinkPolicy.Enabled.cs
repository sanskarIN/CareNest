using System.Windows.Input;

namespace CareNest.App.ViewModels;

internal static class FundingLinkPolicy
{
    private const string FundingUrl = "https://buymeacoffee.com/sanskarIN";

    public static bool IsVisible => true;

    public static ICommand CreateCommand(Func<string, Task> openAsync)
    {
        ArgumentNullException.ThrowIfNull(openAsync);
        return new AsyncCommand(() => openAsync(FundingUrl));
    }
}
