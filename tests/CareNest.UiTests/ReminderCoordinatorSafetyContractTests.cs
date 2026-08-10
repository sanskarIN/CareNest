namespace CareNest.UiTests;

public sealed class ReminderCoordinatorSafetyContractTests
{
    [Fact]
    public void Rebuild_RequiresUtcStartBeforePlanning()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Application",
            "Services",
            "ReminderCoordinator.cs");

        Assert.Contains("now.Kind != DateTimeKind.Utc", source, StringComparison.Ordinal);
        Assert.Contains("Reminder rebuild start must be UTC.", source, StringComparison.Ordinal);
        Assert.Contains("nameof(fromUtc)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Snooze_RequiresExplicitFutureUtcTimestamp()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Application",
            "Services",
            "ReminderCoordinator.cs");

        Assert.Contains("snoozedUntilUtc is null", source, StringComparison.Ordinal);
        Assert.Contains("snoozedUntilUtc.Value.Kind != DateTimeKind.Utc", source, StringComparison.Ordinal);
        Assert.Contains("snoozedUntilUtc.Value <= now", source, StringComparison.Ordinal);
        Assert.Contains("Snooze time must be UTC.", source, StringComparison.Ordinal);
        Assert.Contains("Snooze time must be in the future.", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Snooze_UsesValidatedTimestampForOccurrenceAndNotification()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Application",
            "Services",
            "ReminderCoordinator.cs");

        Assert.Contains("occurrence.SnoozedUntilUtc = newState == ReminderState.Snoozed ? snoozedUntilUtc : null", source, StringComparison.Ordinal);
        Assert.Contains("snoozedUntilUtc.Value,", source, StringComparison.Ordinal);
    }
}
