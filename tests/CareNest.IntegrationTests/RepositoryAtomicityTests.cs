using CareNest.Domain.Entities;

namespace CareNest.IntegrationTests;

public sealed class RepositoryAtomicityTests
{
    [Fact]
    public async Task SavePrimaryProfile_WhenReplacementInsertFails_PreservesExistingPrimary()
    {
        await using var store = await TestStore.CreateAsync();
        var existing = new PersonProfile
        {
            Id = "profile-existing",
            Name = "Existing",
            IsPrimary = true
        };
        await store.Repository.SaveProfileAsync(existing);

        var invalidReplacement = new PersonProfile
        {
            Id = "profile-invalid",
            Name = null!,
            IsPrimary = true
        };

        await Assert.ThrowsAnyAsync<Exception>(() =>
            store.Repository.SaveProfileAsync(invalidReplacement));

        var reloadedExisting = await store.Repository.GetProfileAsync(existing.Id);
        Assert.NotNull(reloadedExisting);
        Assert.True(reloadedExisting!.IsPrimary);
        Assert.Null(await store.Repository.GetProfileAsync(invalidReplacement.Id));
    }

    [Fact]
    public async Task SaveSchedule_WhenOneTimeInsertFails_PreservesPreviousScheduleAndTimes()
    {
        await using var store = await TestStore.CreateAsync();
        var profile = new PersonProfile { Name = "Schedule profile" };
        await store.Repository.SaveProfileAsync(profile);
        var medicine = new Medicine
        {
            ProfileId = profile.Id,
            Name = "Schedule medicine",
            Form = "Custom",
            StartDate = DateTime.Today
        };
        await store.Repository.SaveMedicineAsync(medicine);

        var schedule = new MedicineSchedule
        {
            Id = "schedule-1",
            MedicineId = medicine.Id,
            StartDate = DateTime.Today,
            TimeZoneId = TimeZoneInfo.Utc.Id,
            Enabled = true
        };
        var originalTimes = new[]
        {
            new ScheduleTime
            {
                Id = "time-original-1",
                MedicineScheduleId = schedule.Id,
                Hour = 8,
                Minute = 0
            },
            new ScheduleTime
            {
                Id = "time-original-2",
                MedicineScheduleId = schedule.Id,
                Hour = 20,
                Minute = 0
            }
        };
        await store.Repository.SaveScheduleAsync(schedule, originalTimes);

        schedule.Enabled = false;
        var duplicateIdTimes = new[]
        {
            new ScheduleTime
            {
                Id = "duplicate-time",
                MedicineScheduleId = schedule.Id,
                Hour = 9,
                Minute = 0
            },
            new ScheduleTime
            {
                Id = "duplicate-time",
                MedicineScheduleId = schedule.Id,
                Hour = 21,
                Minute = 0
            }
        };

        await Assert.ThrowsAnyAsync<Exception>(() =>
            store.Repository.SaveScheduleAsync(schedule, duplicateIdTimes));

        var reloadedSchedule = Assert.Single(
            await store.Repository.GetSchedulesForMedicineAsync(medicine.Id));
        var reloadedTimes = await store.Repository.GetScheduleTimesAsync(schedule.Id);

        Assert.True(reloadedSchedule.Enabled);
        Assert.Equal(2, reloadedTimes.Count);
        Assert.Contains(reloadedTimes, item => item.Id == "time-original-1");
        Assert.Contains(reloadedTimes, item => item.Id == "time-original-2");
        Assert.DoesNotContain(reloadedTimes, item => item.Id == "duplicate-time");
    }
}
