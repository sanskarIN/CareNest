namespace CareNest.UiTests;

public sealed class AndroidReceiverLifecycleContractTests
{
    [Fact]
    public void SystemEventReceiver_UsesGoAsyncAndAlwaysFinishesPendingResult()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "Platforms",
            "Android",
            "PlatformNotificationService.Android.cs");
        var receiver = source[source.IndexOf(
            "public sealed class CareNestSystemEventReceiver",
            StringComparison.Ordinal)..];

        Assert.Contains("var pendingResult = GoAsync()", receiver, StringComparison.Ordinal);
        Assert.Contains("try", receiver, StringComparison.Ordinal);
        Assert.Contains("finally", receiver, StringComparison.Ordinal);
        Assert.Contains("pendingResult?.Finish()", receiver, StringComparison.Ordinal);
        Assert.Contains("CancellationToken.None", receiver, StringComparison.Ordinal);
    }

    [Fact]
    public void SystemEventReceiver_ContainsBackgroundRebuildFailures()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "Platforms",
            "Android",
            "PlatformNotificationService.Android.cs");
        var receiver = source[source.IndexOf(
            "public sealed class CareNestSystemEventReceiver",
            StringComparison.Ordinal)..];

        Assert.Contains("catch", receiver, StringComparison.Ordinal);
        Assert.Contains("coordinator.RebuildAsync", receiver, StringComparison.Ordinal);
        Assert.Contains("appointments.RebuildRemindersAsync", receiver, StringComparison.Ordinal);
        Assert.Contains("backups.SyncAsync", receiver, StringComparison.Ordinal);
    }
}
