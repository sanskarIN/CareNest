namespace CareNest.UiTests;

public sealed class WindowsNotificationTimerContractTests
{
    [Fact]
    public void WindowsReminderTimers_AreNotLinkedToCallerCancellationLifetime()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "Platforms",
            "Windows",
            "PlatformNotificationService.Windows.cs");

        Assert.DoesNotContain("CreateLinkedTokenSource", source, StringComparison.Ordinal);
        Assert.Contains("var cts = new CancellationTokenSource()", source, StringComparison.Ordinal);
        Assert.Contains("var timerToken = cts.Token", source, StringComparison.Ordinal);
    }

    [Fact]
    public void WindowsReminderCancellation_LeavesTimerDisposalToBackgroundOwner()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "Platforms",
            "Windows",
            "PlatformNotificationService.Windows.cs");
        var cancelStart = source.IndexOf(
            "private partial Task CancelCoreAsync(",
            StringComparison.Ordinal);
        var cancelEnd = source.IndexOf(
            "private partial Task CancelAllCoreAsync(",
            cancelStart,
            StringComparison.Ordinal);
        Assert.True(cancelStart >= 0);
        Assert.True(cancelEnd > cancelStart);
        var cancel = source[cancelStart..cancelEnd];

        Assert.Contains("cts.Cancel()", cancel, StringComparison.Ordinal);
        Assert.DoesNotContain("cts.Dispose()", cancel, StringComparison.Ordinal);
        Assert.Contains("cts.Dispose()", source, StringComparison.Ordinal);
    }

    [Fact]
    public void WindowsOldTimer_CannotRemoveNewerReplacementWithSameId()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "Platforms",
            "Windows",
            "PlatformNotificationService.Windows.cs");

        Assert.Contains("RemoveOnlyIfCurrent(request.OccurrenceId, cts)", source, StringComparison.Ordinal);
        Assert.Contains("ICollection<KeyValuePair<string, CancellationTokenSource>> entries = Scheduled", source, StringComparison.Ordinal);
        Assert.Contains("entries.Remove(new KeyValuePair<string, CancellationTokenSource>(occurrenceId, owner))", source, StringComparison.Ordinal);
    }

    [Fact]
    public void WindowsBackgroundTimer_ContainsNotificationDisplayFailures()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "Platforms",
            "Windows",
            "PlatformNotificationService.Windows.cs");

        Assert.Contains("catch (OperationCanceledException) when (timerToken.IsCancellationRequested)", source, StringComparison.Ordinal);
        Assert.Contains("catch\n            {\n                // Notification display failures", source, StringComparison.Ordinal);
        Assert.Contains("finally", source, StringComparison.Ordinal);
    }
}
