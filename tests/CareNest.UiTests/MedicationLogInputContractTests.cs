namespace CareNest.UiTests;

public sealed class MedicationLogInputContractTests
{
    [Fact]
    public void ManualMedicationLogEdit_RejectsUndefinedStatusBeforeRepositoryRead()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "MedicationLogViewModel.cs");
        var start = source.IndexOf(
            "public Task EditEntryAsync(",
            StringComparison.Ordinal);
        var end = source.IndexOf(
            "private Task ChangeReminderAsync(",
            start,
            StringComparison.Ordinal);
        Assert.True(start >= 0);
        Assert.True(end > start);
        var method = source[start..end];

        var validationIndex = method.IndexOf("if (!Enum.IsDefined(status))", StringComparison.Ordinal);
        var repositoryIndex = method.IndexOf("_repository.GetMedicationLogAsync", StringComparison.Ordinal);

        Assert.True(validationIndex >= 0);
        Assert.True(repositoryIndex > validationIndex);
        Assert.Contains("ArgumentOutOfRangeException", method, StringComparison.Ordinal);
    }
}
