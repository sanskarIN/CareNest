namespace CareNest.UiTests;

public sealed class ReminderLoggingPrivacyContractTests
{
    [Fact]
    public void ReminderCoordinator_DoesNotLogExceptionObjectsOrRecordIdentifiers()
    {
        var source = RepositoryLocator.Read("src", "CareNest.Application", "Services", "ReminderCoordinator.cs");

        Assert.Contains("ex.GetType().FullName", source, StringComparison.Ordinal);
        Assert.DoesNotContain("logger.LogWarning(ex", source, StringComparison.Ordinal);
        Assert.DoesNotContain("{OccurrenceId}", source, StringComparison.Ordinal);
        Assert.DoesNotContain("{MedicineId}", source, StringComparison.Ordinal);
        Assert.DoesNotContain("ex.Message", source, StringComparison.Ordinal);
        Assert.DoesNotContain("ex.StackTrace", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ReminderCoordinator_LogMessagesDeclareRedactionBoundary()
    {
        var source = RepositoryLocator.Read("src", "CareNest.Application", "Services", "ReminderCoordinator.cs");

        Assert.Contains("Health record identifiers and exception details were not logged", source, StringComparison.Ordinal);
    }
}
