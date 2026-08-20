using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CareNest.CrossPlatform.Views;

public sealed partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        PlatformText.Text = $"Running on {GetPlatformName()}";
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        PlatformText = this.FindControl<TextBlock>("PlatformText")
            ?? throw new InvalidOperationException("PlatformText control was not found.");
    }

    private TextBlock PlatformText { get; set; } = null!;

    private static string GetPlatformName()
    {
        if (OperatingSystem.IsBrowser())
        {
            return "WebAssembly browser";
        }

        if (OperatingSystem.IsWindows())
        {
            return "Windows";
        }

        if (OperatingSystem.IsLinux())
        {
            return "Linux";
        }

        if (OperatingSystem.IsMacOS())
        {
            return "macOS";
        }

        if (OperatingSystem.IsAndroid())
        {
            return "Android";
        }

        if (OperatingSystem.IsIOS())
        {
            return "iOS/iPadOS";
        }

        return "an available .NET platform";
    }
}
