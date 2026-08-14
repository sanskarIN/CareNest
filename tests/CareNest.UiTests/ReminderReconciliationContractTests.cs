namespace CareNest.UiTests;

public sealed class ReminderReconciliationContractTests
{
    [Fact]
    public void Coordinator_UsesEffectiveDueTimeForScheduledAndSnoozedRows()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Application",
            "Services",
            "ReminderCoordinator.cs");

        Assert.Contains("private static DateTime EffectiveDueUtc", source, StringComparison.Ordinal);
        Assert.Contains("ReminderState.Snoozed && occurrence.SnoozedUntilUtc", source, StringComparison.Ordinal);
        Assert.Contains("GetActionableOccurrencesAsync", source, StringComparison.Ordinal);
        Assert.Contains("MarkOverdueAsMissedAsync", source, StringComparison.Ordinal);
        Assert.Contains("GetUpcomingAsync", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Rebuild_CancelsExistingPlatformRequestBeforeReplacingOrInvalidatingIt()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Application",
            "Services",
            "ReminderCoordinator.cs");
        var rebuild = source[..source.IndexOf(
            "public async Task HandleOccurrenceAsync(",
            StringComparison.Ordinal)];

        var cancelIndex = rebuild.IndexOf("TryCancelPlatformRequestAsync", StringComparison.Ordinal);
        var invalidIndex = rebuild.IndexOf("if (!valid)", StringComparison.Ordinal);
        var scheduleIndex = rebuild.IndexOf("notificationService.ScheduleAsync", StringComparison.Ordinal);

        Assert.True(cancelIndex >= 0);
        Assert.True(invalidIndex > cancelIndex);
        Assert.True(scheduleIndex > invalidIndex);
        Assert.Contains("occurrence.State = ReminderState.Cancelled", rebuild, StringComparison.Ordinal);
        Assert.Contains("IsInsideQuietHours", rebuild, StringComparison.Ordinal);
    }

    [Fact]
    public void MedicineScheduleSave_DoesNotDeleteRowsBeforeRebuildCanCancelTheirPlatformRequests()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Application",
            "Services",
            "MedicineService.cs");
        var start = source.IndexOf("public async Task SaveScheduleAsync(", StringComparison.Ordinal);
        var end = source.IndexOf("public async Task ApplyStockAdjustmentAsync(", start, StringComparison.Ordinal);
        Assert.True(start >= 0);
        Assert.True(end > start);
        var method = source[start..end];

        Assert.DoesNotContain("DeleteFutureOccurrencesForScheduleAsync", method, StringComparison.Ordinal);
        Assert.Contains("await reminders.RebuildAsync", method, StringComparison.Ordinal);
        Assert.True(
            method.IndexOf("await reminders.RebuildAsync", StringComparison.Ordinal) <
            method.IndexOf("AddAuditEntryAsync", StringComparison.Ordinal));
    }

    [Fact]
    public void MedicineDelete_CancelsPlatformRequestsBeforeCascadeAndCompensatesOnCascadeFailure()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Application",
            "Services",
            "MedicineService.cs");
        var method = source[source.IndexOf("public async Task DeleteAsync(", StringComparison.Ordinal)..];

        var cancelIndex = method.IndexOf("CancelFutureForMedicineAsync", StringComparison.Ordinal);
        var cascadeIndex = method.IndexOf("DeleteMedicineCascadeAsync", StringComparison.Ordinal);
        Assert.True(cancelIndex >= 0);
        Assert.True(cascadeIndex > cancelIndex);
        Assert.Contains("TryRestoreReminderRequestsAsync", method, StringComparison.Ordinal);
        Assert.Contains("CancellationToken.None", method, StringComparison.Ordinal);
    }

    [Fact]
    public void ProfileDelete_CancelsPlatformRequestsBeforeCascadeAndCompensatesOnCascadeFailure()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.Application",
            "Services",
            "ProfileService.cs");
        var method = source[source.IndexOf("public async Task DeleteAsync(", StringComparison.Ordinal)..];

        var cancelIndex = method.IndexOf("CancelFutureForProfileAsync", StringComparison.Ordinal);
        var cascadeIndex = method.IndexOf("DeleteProfileCascadeAsync", StringComparison.Ordinal);
        Assert.True(cancelIndex >= 0);
        Assert.True(cascadeIndex > cancelIndex);
        Assert.Contains("TryRestoreReminderRequestsAsync", method, StringComparison.Ordinal);
        Assert.Contains("CancellationToken.None", method, StringComparison.Ordinal);
    }
}
