using CareNest.Application.Services;
using CareNest.Domain.Enums;
using CareNest.UnitTests.TestDoubles;
using Microsoft.Extensions.Logging.Abstractions;

namespace CareNest.UnitTests;

public sealed class ReminderCoordinatorActionValidationTests
{
    private static readonly DateTimeOffset Now =
        new(2026, 8, 14, 1, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task HandleOccurrenceAsync_ScheduledState_IsRejectedBeforeRepositoryMutation()
    {
        var coordinator = CreateCoordinator();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            coordinator.HandleOccurrenceAsync(
                "occurrence-1",
                ReminderState.Scheduled));
    }

    [Fact]
    public async Task HandleOccurrenceAsync_UndefinedState_IsRejectedBeforeRepositoryMutation()
    {
        var coordinator = CreateCoordinator();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            coordinator.HandleOccurrenceAsync(
                "occurrence-1",
                (ReminderState)999));
    }

    private static ReminderCoordinator CreateCoordinator() =>
        new(
            new RepositoryStub(),
            new NotificationServiceSpy(),
            new ReminderPlanner(),
            new FixedTimeProvider(Now),
            NullLogger<ReminderCoordinator>.Instance);
}
