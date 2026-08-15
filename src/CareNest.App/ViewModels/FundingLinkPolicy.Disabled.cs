using System.Windows.Input;

namespace CareNest.App.ViewModels;

internal static class FundingLinkPolicy
{
    public static bool IsVisible => false;

    public static ICommand CreateCommand(Func<string, Task> openAsync)
    {
        ArgumentNullException.ThrowIfNull(openAsync);
        return new AsyncCommand(() => Task.CompletedTask, static () => false);
    }
}
