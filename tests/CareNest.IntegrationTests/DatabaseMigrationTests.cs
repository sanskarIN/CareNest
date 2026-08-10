using CareNest.Domain.Entities;
using SQLite;

namespace CareNest.IntegrationTests;

public sealed class DatabaseMigrationTests
{
    [Fact]
    public async Task FreshDatabase_ReachesCurrentSchema_AndPassesIntegrity()
    {
        await using var store = await TestStore.CreateAsync();

        var version = await store.Repository.GetSchemaVersionAsync();

        Assert.Equal(5, version);
        var integrity = await store.Database.Connection.ExecuteScalarAsync<string>("PRAGMA integrity_check;");
        Assert.Equal("ok", integrity, ignoreCase: true);

        var journalMode = await store.Database.Connection.ExecuteScalarAsync<string>("PRAGMA journal_mode;");
        Assert.Equal("wal", journalMode, ignoreCase: true);

        var busyTimeout = await store.Database.Connection.ExecuteScalarAsync<int>("PRAGMA busy_timeout;");
        Assert.True(busyTimeout >= 5000);
    }

    [Fact]
    public async Task Snapshot_FromWalDatabase_CreatesNonEmptyDatabaseFile()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile { Name = "Snapshot profile", IsPrimary = true };
        await store.Repository.SaveProfileAsync(profile);

        var snapshotPath = Path.Combine(
            Path.GetTempPath(),
            $"carenest-snapshot-{Guid.NewGuid():N}.db");

        try
        {
            await store.Database.CreateSnapshotAsync(snapshotPath);

            Assert.True(File.Exists(snapshotPath));
            Assert.True(new FileInfo(snapshotPath).Length > 0);
        }
        finally
        {
            if (File.Exists(snapshotPath))
            {
                File.Delete(snapshotPath);
            }
        }
    }

    [Fact]
    public async Task Snapshot_FromWalDatabase_PreservesCommittedProfileData()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile { Name = "Snapshot contents", IsPrimary = true };
        await store.Repository.SaveProfileAsync(profile);

        var snapshotPath = Path.Combine(
            Path.GetTempPath(),
            $"carenest-snapshot-content-{Guid.NewGuid():N}.db");

        SQLiteAsyncConnection? snapshot = null;
        try
        {
            await store.Database.CreateSnapshotAsync(snapshotPath);
            snapshot = new SQLiteAsyncConnection(
                snapshotPath,
                SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);

            var count = await snapshot.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM PersonProfile WHERE Id = ? AND Name = ?;",
                profile.Id,
                profile.Name);
            var integrity = await snapshot.ExecuteScalarAsync<string>("PRAGMA integrity_check;");

            Assert.Equal(1, count);
            Assert.Equal("ok", integrity, ignoreCase: true);
        }
        finally
        {
            if (snapshot is not null)
            {
                await snapshot.CloseAsync();
            }

            if (File.Exists(snapshotPath))
            {
                File.Delete(snapshotPath);
            }
        }
    }

    [Fact]
    public async Task Repository_RoundTripsCoreEntities()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile { Name = "Test profile", IsPrimary = true };
        await store.Repository.SaveProfileAsync(profile);

        var medicine = new Medicine
        {
            ProfileId = profile.Id,
            Name = "Test medicine record",
            Form = "Custom",
            StartDate = DateTime.Today,
            StockCount = 10,
            RefillThreshold = 2,
            StockChangePerTakenEvent = 1,
            RefillDate = DateTime.Today.AddDays(7)
        };
        await store.Repository.SaveMedicineAsync(medicine);

        var loaded = await store.Repository.GetMedicineAsync(medicine.Id);

        Assert.NotNull(loaded);
        Assert.Equal(1m, loaded.StockChangePerTakenEvent);
        Assert.Equal(medicine.RefillDate, loaded.RefillDate);
    }

    [Fact]
    public async Task CascadeDelete_RemovesProfileOwnedRows()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile { Name = "Delete me" };
        await store.Repository.SaveProfileAsync(profile);
        var medicine = new Medicine
        {
            ProfileId = profile.Id,
            Name = "Record",
            Form = "Custom",
            StartDate = DateTime.Today
        };
        await store.Repository.SaveMedicineAsync(medicine);

        await store.Repository.DeleteProfileCascadeAsync(profile.Id);

        Assert.Null(await store.Repository.GetProfileAsync(profile.Id));
        Assert.Null(await store.Repository.GetMedicineAsync(medicine.Id));
    }
}
