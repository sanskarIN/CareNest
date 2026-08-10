namespace CareNest.UiTests;

public sealed class ViewModelContractTests
{
    [Fact]
    public void ViewModels_DoNotUseAsyncVoidOrTaskRun()
    {
        foreach (var path in EnumerateViewModels())
        {
            var source = File.ReadAllText(path);
            Assert.DoesNotContain("async void", source, StringComparison.Ordinal);
            Assert.DoesNotContain("Task.Run(", source, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void ViewModels_DoNotReachIntoSqliteInfrastructureDirectly()
    {
        foreach (var path in EnumerateViewModels())
        {
            var source = File.ReadAllText(path);
            Assert.DoesNotContain("SQLiteAsyncConnection", source, StringComparison.Ordinal);
            Assert.DoesNotContain("SqliteDatabase", source, StringComparison.Ordinal);
            Assert.DoesNotContain("CareNest.Infrastructure.Persistence", source, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void ViewModels_DoNotCreateNetworkClients()
    {
        foreach (var path in EnumerateViewModels())
        {
            var source = File.ReadAllText(path);
            Assert.DoesNotContain("HttpClient", source, StringComparison.Ordinal);
            Assert.DoesNotContain("WebClient", source, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void AboutViewModel_UsesCentralizedPublicDestinations()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "AboutViewModel.cs");

        Assert.Contains("AppConstants.RepositoryUrl", source, StringComparison.Ordinal);
        Assert.Contains("AppConstants.CreatorUrl", source, StringComparison.Ordinal);
        Assert.Contains("AppConstants.FundingUrl", source, StringComparison.Ordinal);
        Assert.Contains("AppConstants.BusinessEmail", source, StringComparison.Ordinal);
        Assert.Contains("AppConstants.SupportEmail", source, StringComparison.Ordinal);
        Assert.DoesNotContain("buymeacoffee.com/sanskarIN", source, StringComparison.Ordinal);
    }

    [Fact]
    public void OnboardingViewModel_DoesNotRequestNotificationPermission()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "OnboardingViewModel.cs");

        Assert.DoesNotContain("RequestPermissionAsync", source, StringComparison.Ordinal);
        Assert.DoesNotContain("INotificationService", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ScheduleEditorViewModel_PreservesAsNeededNoReminderBehavior()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "ScheduleEditorViewModel.cs");

        Assert.Contains("Kind != ScheduleKind.AsNeeded", source, StringComparison.Ordinal);
        Assert.Contains("RequestPermissionAsync", source, StringComparison.Ordinal);
    }

    private static string[] EnumerateViewModels()
    {
        var directory = Path.Combine(RepositoryLocator.Root, "src", "CareNest.App", "ViewModels");
        var files = Directory.EnumerateFiles(directory, "*ViewModel.cs", SearchOption.TopDirectoryOnly).ToArray();
        Assert.NotEmpty(files);
        return files;
    }
}
