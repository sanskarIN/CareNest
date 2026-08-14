using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.UnitTests.TestDoubles;

namespace CareNest.UnitTests;

public sealed class MedicineServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 9, 30, 0, TimeSpan.Zero);

    [Fact]
    public async Task SaveAsync_NewMedicine_PersistsCreatedAuditAndRebuildsReminders()
    {
        var repository = new RecordingRepository();
        var reminders = new ReminderCoordinatorSpy();
        var service = new MedicineService(repository, reminders, new FixedTimeProvider(Now));
        var medicine = ValidMedicine();

        await service.SaveAsync(medicine);

        Assert.Same(medicine, repository.SavedMedicine);
        Assert.Equal(Now.UtcDateTime, medicine.UpdatedUtc);
        var audit = Assert.Single(repository.AuditEntries);
        Assert.Equal(AuditAction.Created, audit.Action);
        Assert.Equal(medicine.Id, audit.EntityId);
        Assert.Equal(1, reminders.RebuildCount);
    }

    [Fact]
    public async Task SaveAsync_ExistingMedicine_PersistsUpdatedAudit()
    {
        var medicine = ValidMedicine();
        var repository = new RecordingRepository { ExistingMedicine = medicine };
        var service = new MedicineService(repository, new ReminderCoordinatorSpy(), new FixedTimeProvider(Now));

        await service.SaveAsync(medicine);

        var audit = Assert.Single(repository.AuditEntries);
        Assert.Equal(AuditAction.Updated, audit.Action);
        Assert.Equal("Medicine record updated", audit.SafeSummary);
    }

    [Fact]
    public async Task SaveScheduleAsync_PreservesRowsForPlatformReconciliationAndRebuilds()
    {
        var repository = new RecordingRepository();
        var reminders = new ReminderCoordinatorSpy();
        var service = new MedicineService(repository, reminders, new FixedTimeProvider(Now));
        var schedule = new MedicineSchedule
        {
            Id = "schedule-1",
            MedicineId = "medicine-1",
            Kind = ScheduleKind.Daily,
            StartDate = new DateTime(2026, 8, 1),
            TimeZoneId = TimeZoneInfo.Utc.Id
        };
        IReadOnlyCollection<ScheduleTime> times =
        [
            new ScheduleTime { MedicineScheduleId = schedule.Id, Hour = 8, Minute = 15 }
        ];

        await service.SaveScheduleAsync(schedule, times);

        Assert.Same(schedule, repository.SavedSchedule);
        Assert.Same(times, repository.SavedScheduleTimes);
        Assert.Equal(Now.UtcDateTime, schedule.UpdatedUtc);
        Assert.Null(repository.DeletedFutureScheduleId);
        Assert.Equal(1, reminders.RebuildCount);
        var audit = Assert.Single(repository.AuditEntries);
        Assert.Equal(nameof(MedicineSchedule), audit.EntityType);
        Assert.Equal(schedule.Id, audit.EntityId);
    }

    [Fact]
    public async Task ApplyStockAdjustmentAsync_UsesCalculatedStockAndPersistsUserDelta()
    {
        var medicine = ValidMedicine();
        var repository = new RecordingRepository
        {
            ExistingMedicine = medicine,
            CalculatedStock = 5m
        };
        var service = new MedicineService(repository, new ReminderCoordinatorSpy(), new FixedTimeProvider(Now));

        await service.ApplyStockAdjustmentAsync(medicine.Id, -2m, "Manual correction", "log-1");

        var adjustment = Assert.IsType<StockAdjustment>(repository.SavedStockAdjustment);
        Assert.Equal(medicine.Id, adjustment.MedicineId);
        Assert.Equal(-2m, adjustment.QuantityDelta);
        Assert.Equal("Manual correction", adjustment.Reason);
        Assert.Equal("log-1", adjustment.MedicationLogEntryId);
        Assert.Equal(Now.UtcDateTime, adjustment.EventUtc);
    }

    [Fact]
    public async Task ApplyStockAdjustmentAsync_FallsBackToUserEnteredMedicineStock()
    {
        var medicine = ValidMedicine();
        medicine.StockCount = 4m;
        var repository = new RecordingRepository
        {
            ExistingMedicine = medicine,
            CalculatedStock = null
        };
        var service = new MedicineService(repository, new ReminderCoordinatorSpy(), new FixedTimeProvider(Now));

        await service.ApplyStockAdjustmentAsync(medicine.Id, -4m, null);

        Assert.NotNull(repository.SavedStockAdjustment);
    }

    [Fact]
    public async Task ApplyStockAdjustmentAsync_RejectsNegativeEstimatedStockWithoutPersisting()
    {
        var medicine = ValidMedicine();
        var repository = new RecordingRepository
        {
            ExistingMedicine = medicine,
            CalculatedStock = 1m
        };
        var service = new MedicineService(repository, new ReminderCoordinatorSpy(), new FixedTimeProvider(Now));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ApplyStockAdjustmentAsync(medicine.Id, -2m, null));

        Assert.Null(repository.SavedStockAdjustment);
    }

    [Fact]
    public async Task DeleteAsync_CancelsFutureRequestsBeforeCascadeAndAudits()
    {
        var repository = new RecordingRepository();
        var reminders = new ReminderCoordinatorSpy();
        var service = new MedicineService(repository, reminders, new FixedTimeProvider(Now));

        await service.DeleteAsync("medicine-1");

        Assert.Equal(new[] { "medicine-1" }, reminders.CancelledMedicineIds);
        Assert.Equal("medicine-1", repository.DeletedMedicineId);
        var audit = Assert.Single(repository.AuditEntries);
        Assert.Equal(AuditAction.Deleted, audit.Action);
        Assert.Equal("medicine-1", audit.EntityId);
        Assert.Equal(0, reminders.RebuildCount);
    }

    [Fact]
    public async Task DeleteAsync_CascadeFails_RebuildsReminderRequestsForCompensation()
    {
        var repository = new RecordingRepository
        {
            DeleteMedicineFailure = new InvalidOperationException("test failure")
        };
        var reminders = new ReminderCoordinatorSpy();
        var service = new MedicineService(repository, reminders, new FixedTimeProvider(Now));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync("medicine-1"));

        Assert.Equal(new[] { "medicine-1" }, reminders.CancelledMedicineIds);
        Assert.Equal(1, reminders.RebuildCount);
    }

    private static Medicine ValidMedicine() => new()
    {
        Id = "medicine-1",
        ProfileId = "profile-1",
        Name = "User medicine",
        Form = "User-entered form",
        StrengthText = "opaque strength text",
        InstructionText = "opaque instruction text",
        StartDate = new DateTime(2026, 8, 1),
        State = MedicineState.Active
    };

    private sealed class RecordingRepository : RepositoryStub
    {
        public Medicine? ExistingMedicine { get; init; }

        public decimal? CalculatedStock { get; init; }

        public Medicine? SavedMedicine { get; private set; }

        public MedicineSchedule? SavedSchedule { get; private set; }

        public IReadOnlyCollection<ScheduleTime>? SavedScheduleTimes { get; private set; }

        public string? DeletedFutureScheduleId { get; private set; }

        public DateTime? DeletedFutureFromUtc { get; private set; }

        public StockAdjustment? SavedStockAdjustment { get; private set; }

        public string? DeletedMedicineId { get; private set; }

        public Exception? DeleteMedicineFailure { get; init; }

        public List<AuditEntry> AuditEntries { get; } = [];

        public override Task<Medicine?> GetMedicineAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ExistingMedicine?.Id == id ? ExistingMedicine : null);
        }

        public override Task SaveMedicineAsync(Medicine medicine, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SavedMedicine = medicine;
            return Task.CompletedTask;
        }

        public override Task SaveScheduleAsync(
            MedicineSchedule schedule,
            IReadOnlyCollection<ScheduleTime> times,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SavedSchedule = schedule;
            SavedScheduleTimes = times;
            return Task.CompletedTask;
        }

        public override Task DeleteFutureOccurrencesForScheduleAsync(
            string scheduleId,
            DateTime fromUtc,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DeletedFutureScheduleId = scheduleId;
            DeletedFutureFromUtc = fromUtc;
            return Task.CompletedTask;
        }

        public override Task<decimal?> CalculateCurrentStockAsync(string medicineId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(CalculatedStock);
        }

        public override Task SaveStockAdjustmentAsync(StockAdjustment adjustment, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SavedStockAdjustment = adjustment;
            return Task.CompletedTask;
        }

        public override Task DeleteMedicineCascadeAsync(string medicineId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (DeleteMedicineFailure is not null)
            {
                return Task.FromException(DeleteMedicineFailure);
            }

            DeletedMedicineId = medicineId;
            return Task.CompletedTask;
        }

        public override Task AddAuditEntryAsync(AuditEntry entry, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            AuditEntries.Add(entry);
            return Task.CompletedTask;
        }
    }
}
