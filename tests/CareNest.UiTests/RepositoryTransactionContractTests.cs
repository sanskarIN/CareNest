namespace CareNest.UiTests;

public sealed class RepositoryTransactionContractTests
{
    [Fact]
    public void MultiStepRepositoryWrites_UseSharedTransactionBoundary()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Persistence",
            "CareNestRepository.cs");

        Assert.Contains("RunAtomicAsync(connection =>", source, StringComparison.Ordinal);
        Assert.Contains("Db.RunInTransactionAsync(connection =>", source, StringComparison.Ordinal);
        Assert.Contains("SaveProfileAsync(PersonProfile profile", source, StringComparison.Ordinal);
        Assert.Contains("SaveScheduleAsync(MedicineSchedule schedule", source, StringComparison.Ordinal);
        Assert.Contains("DeleteProfileCascadeAsync(string profileId", source, StringComparison.Ordinal);
        Assert.Contains("DeleteMedicineCascadeAsync(string medicineId", source, StringComparison.Ordinal);
        Assert.Contains("DeleteDocumentAsync(string id", source, StringComparison.Ordinal);
        Assert.Contains("SetDocumentTagsAsync(string documentId", source, StringComparison.Ordinal);
        Assert.Contains("DeleteEmergencyContactAsync(string id", source, StringComparison.Ordinal);
        Assert.Contains("Action<SQLiteConnection> action,\n        CancellationToken cancellationToken", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ClearAll_UsesAtomicDeleteOnlyAndDoesNotDependOnVacuum()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Persistence",
            "CareNestRepository.cs");
        var start = source.IndexOf(
            "public Task ClearAllAsync(",
            StringComparison.Ordinal);
        var end = source.IndexOf(
            "private async Task RunAtomicAsync(",
            start,
            StringComparison.Ordinal);
        Assert.True(start >= 0);
        Assert.True(end > start);
        var method = source[start..end];

        Assert.Contains("RunAtomicAsync(connection =>", method, StringComparison.Ordinal);
        Assert.Contains("connection.Execute($\"DELETE FROM {table};\")", method, StringComparison.Ordinal);
        Assert.DoesNotContain("VACUUM", method, StringComparison.OrdinalIgnoreCase);
    }
}
