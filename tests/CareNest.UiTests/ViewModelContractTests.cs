namespace CareNest.UiTests;

public sealed class ViewModelContractTests
{
    [Fact]
    public void ConcreteViewModels_DoNotUseAsyncVoidOrTaskRun()
    {
        foreach (var path in EnumerateConcreteViewModels())
        {
            var source = File.ReadAllText(path);
            Assert.DoesNotContain("async void", source, StringComparison.Ordinal);
            Assert.DoesNotContain("Task.Run(", source, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void ViewModels_DoNotReachIntoSqliteInfrastructureDirectly()
    {
        foreach (var path in EnumerateConcreteViewModels())
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
        foreach (var path in EnumerateConcreteViewModels())
        {
            var source = File.ReadAllText(path);
            Assert.DoesNotContain("HttpClient", source, StringComparison.Ordinal);
            Assert.DoesNotContain("WebClient", source, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void AboutViewModel_UsesCentralizedPublicDestinationsAndPhysicalFundingPolicy()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "AboutViewModel.cs");
        var constants = RepositoryLocator.Read("src", "CareNest.Shared", "AppConstants.cs");
        var enabled = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "FundingLinkPolicy.Enabled.cs");
        var disabled = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "FundingLinkPolicy.Disabled.cs");

        Assert.Contains("AppConstants.RepositoryUrl", source, StringComparison.Ordinal);
        Assert.Contains("AppConstants.CreatorUrl", source, StringComparison.Ordinal);
        Assert.Contains("AppConstants.BusinessEmail", source, StringComparison.Ordinal);
        Assert.Contains("AppConstants.SupportEmail", source, StringComparison.Ordinal);
        Assert.DoesNotContain("buymeacoffee.com", source, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("CARENEST_FUNDING_LINK", source, StringComparison.Ordinal);
        Assert.Contains("FundingLinkPolicy.CreateCommand(OpenAsync)", source, StringComparison.Ordinal);
        Assert.Contains("FundingLinkPolicy.IsVisible", source, StringComparison.Ordinal);
        Assert.Contains("private const string FundingUrl = \"https://buymeacoffee.com/sanskarIN\";", enabled, StringComparison.Ordinal);
        Assert.Contains("openAsync(FundingUrl)", enabled, StringComparison.Ordinal);
        Assert.DoesNotContain("buymeacoffee.com", disabled, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Task.CompletedTask, static () => false", disabled, StringComparison.Ordinal);
        Assert.DoesNotContain("AppConstants.FundingUrl", source, StringComparison.Ordinal);
        Assert.DoesNotContain("buymeacoffee.com", constants, StringComparison.OrdinalIgnoreCase);
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

    private static string[] EnumerateConcreteViewModels()
    {
        var directory = Path.Combine(RepositoryLocator.Root, "src", "CareNest.App", "ViewModels");
        var files = Directory
            .EnumerateFiles(directory, "*ViewModel.cs", SearchOption.TopDirectoryOnly)
            .Where(path => !string.Equals(Path.GetFileName(path), "ObservableViewModel.cs", StringComparison.Ordinal))
            .ToArray();
        Assert.NotEmpty(files);
        return files;
    }
}
