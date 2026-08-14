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

        Assert.Contains("RunAtomicAsync(cancellationToken", source, StringComparison.Ordinal);
        Assert.Contains("Db.RunInTransactionAsync(connection =>", source, StringComparison.Ordinal);
        Assert.Contains("SaveProfileAsync(PersonProfile profile", source, StringComparison.Ordinal);
        Assert.Contains("SaveScheduleAsync(MedicineSchedule schedule", source, StringComparison.Ordinal);
        Assert.Contains("DeleteProfileCascadeAsync(string profileId", source, StringComparison.Ordinal);
        Assert.Contains("DeleteMedicineCascadeAsync(string medicineId", source, StringComparison.Ordinal);
        Assert.Contains("DeleteDocumentAsync(string id", source, StringComparison.Ordinal);
        Assert.Contains("SetDocumentTagsAsync(string documentId", source, StringComparison.Ordinal);
        Assert.Contains("DeleteEmergencyContactAsync(string id", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ClearAll_DeletesStructuredTablesInsideTransactionBeforeVacuum()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Persistence",
            "CareNestRepository.cs");
        var method = source[source.IndexOf(
            "public async Task ClearAllAsync(",
            StringComparison.Ordinal)..];

        var transactionIndex = method.IndexOf("await RunAtomicAsync", StringComparison.Ordinal);
        var deleteIndex = method.IndexOf("connection.Execute($\"DELETE FROM {table};\")", StringComparison.Ordinal);
        var vacuumIndex = method.IndexOf("await Db.ExecuteAsync(\"VACUUM;\")", StringComparison.Ordinal);

        Assert.True(transactionIndex >= 0);
        Assert.True(deleteIndex > transactionIndex);
        Assert.True(vacuumIndex > deleteIndex);
    }
}
