namespace CareNest.UiTests;

public sealed class DatabaseMigrationSafetyContractTests
{
    [Fact]
    public void SqliteMigrations_ApplyStatementsAndVersionRecordInOneTransaction()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Persistence",
            "SqliteDatabase.cs");

        Assert.Contains("ApplyMigrationAsync(", source, StringComparison.Ordinal);
        Assert.Contains("RunInTransactionAsync(connection =>", source, StringComparison.Ordinal);
        Assert.Contains("foreach (var statement in statements)", source, StringComparison.Ordinal);
        Assert.Contains("connection.Execute(statement)", source, StringComparison.Ordinal);
        Assert.Contains("INSERT OR REPLACE INTO SchemaInfo", source, StringComparison.Ordinal);
    }

    [Fact]
    public void SqliteMigrations_CheckCancellationBeforeStartingEachAtomicMigration()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Infrastructure",
            "Persistence",
            "SqliteDatabase.cs");
        var migrationMethod = source[source.IndexOf(
            "private async Task ApplyMigrationAsync(",
            StringComparison.Ordinal)..];

        var cancellationIndex = migrationMethod.IndexOf(
            "cancellationToken.ThrowIfCancellationRequested()",
            StringComparison.Ordinal);
        var transactionIndex = migrationMethod.IndexOf(
            "RunInTransactionAsync(connection =>",
            StringComparison.Ordinal);

        Assert.True(cancellationIndex >= 0);
        Assert.True(transactionIndex > cancellationIndex);
    }
}
