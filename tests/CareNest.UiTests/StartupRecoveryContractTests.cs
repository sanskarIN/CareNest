namespace CareNest.UiTests;

public sealed class StartupRecoveryContractTests
{
    [Fact]
    public void StartupRecovery_RunsEachRecoveryOperationThroughIndependentBoundary()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "Services",
            "StartupCoordinator.cs");

        Assert.Contains("\"overdue-reminder-reconciliation\"", source, StringComparison.Ordinal);
        Assert.Contains("\"medicine-reminder-rebuild\"", source, StringComparison.Ordinal);
        Assert.Contains("\"appointment-reminder-rebuild\"", source, StringComparison.Ordinal);
        Assert.Contains("\"backup-reminder-sync\"", source, StringComparison.Ordinal);
        Assert.Contains("private async Task RunRecoveryStepAsync(", source, StringComparison.Ordinal);
    }

    [Fact]
    public void StartupRecovery_PropagatesCancellationButContainsOtherStepFailures()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "Services",
            "StartupCoordinator.cs");
        var helper = source[source.IndexOf(
            "private async Task RunRecoveryStepAsync(",
            StringComparison.Ordinal)..];

        Assert.Contains("catch (OperationCanceledException)", helper, StringComparison.Ordinal);
        Assert.Contains("throw;", helper, StringComparison.Ordinal);
        Assert.Contains("catch (Exception ex)", helper, StringComparison.Ordinal);
        Assert.Contains("logger.IsEnabled(LogLevel.Warning)", helper, StringComparison.Ordinal);
        Assert.Contains("ex.GetType().FullName", helper, StringComparison.Ordinal);
        Assert.DoesNotContain("ex.Message", helper, StringComparison.Ordinal);
        Assert.DoesNotContain("ex.StackTrace", helper, StringComparison.Ordinal);
        Assert.DoesNotContain("logger.LogWarning(ex", helper, StringComparison.Ordinal);
    }
}
