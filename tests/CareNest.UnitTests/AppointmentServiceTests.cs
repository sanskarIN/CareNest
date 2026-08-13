using CareNest.Application.Services;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;
using CareNest.UnitTests.TestDoubles;

namespace CareNest.UnitTests;

public sealed class AppointmentServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 13, 9, 30, 0, TimeSpan.Zero);

    [Fact]
    public async Task SaveAsync_FutureAppointment_SchedulesExplicitUtcReminderAndCreatedAudit()
    {
        var repository = new RecordingRepository();
        var notifications = new NotificationServiceSpy();
        var service = new AppointmentService(repository, notifications, new FixedTimeProvider(Now));
        var appointment = ValidAppointment();

        await service.SaveAsync(appointment);

        Assert.Same(appointment, repository.SavedAppointment);
        Assert.Equal(Now.UtcDateTime, appointment.UpdatedUtc);
        var audit = Assert.Single(repository.AuditEntries);
        Assert.Equal(AuditAction.Created, audit.Action);
        Assert.Equal(appointment.Id, audit.EntityId);
        var request = Assert.Single(notifications.Scheduled);
        Assert.Equal($"appointment-{appointment.Id}", request.OccurrenceId);
        Assert.Equal(new DateTime(2026, 8, 14, 9, 0, 0, DateTimeKind.Utc), request.ScheduledUtc);
        Assert.Equal("appointment", request.Category);
        Assert.Contains("Open CareNest", request.Body, StringComparison.Ordinal);
    }

    [Fact]
    public async Task SaveAsync_DeniedPermissionAndRejectedRequest_DoesNotSchedule()
    {
        var repository = new RecordingRepository();
        var notifications = new NotificationServiceSpy
        {
            Diagnostics = new(false, true, true, true, "test", Array.Empty<string>()),
            PermissionRequestResult = false
        };
        var service = new AppointmentService(repository, notifications, new FixedTimeProvider(Now));

        await service.SaveAsync(ValidAppointment());

        Assert.Equal(1, notifications.PermissionRequestCount);
        Assert.Empty(notifications.Scheduled);
    }

    [Fact]
    public async Task SaveAsync_DeniedPermissionThenGranted_SchedulesReminder()
    {
        var repository = new RecordingRepository();
        var notifications = new NotificationServiceSpy
        {
            Diagnostics = new(false, true, true, true, "test", Array.Empty<string>()),
            PermissionRequestResult = true
        };
        var service = new AppointmentService(repository, notifications, new FixedTimeProvider(Now));

        await service.SaveAsync(ValidAppointment());

        Assert.Equal(1, notifications.PermissionRequestCount);
        Assert.Single(notifications.Scheduled);
    }

    [Fact]
    public async Task RebuildRemindersAsync_DeniedPermission_DoesNotPromptOrSchedule()
    {
        var repository = new RecordingRepository { Appointments = [ValidAppointment()] };
        var notifications = new NotificationServiceSpy
        {
            Diagnostics = new(false, true, true, true, "test", Array.Empty<string>())
        };
        var service = new AppointmentService(repository, notifications, new FixedTimeProvider(Now));

        await service.RebuildRemindersAsync();

        Assert.Equal(0, notifications.PermissionRequestCount);
        Assert.Empty(notifications.Scheduled);
    }

    [Fact]
    public async Task RebuildRemindersAsync_StoredNonUtcAppointment_FailsClosed()
    {
        var appointment = ValidAppointment();
        appointment.StartsUtc = DateTime.SpecifyKind(appointment.StartsUtc, DateTimeKind.Unspecified);
        var repository = new RecordingRepository { Appointments = [appointment] };
        var service = new AppointmentService(repository, new NotificationServiceSpy(), new FixedTimeProvider(Now));

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RebuildRemindersAsync());
    }

    [Fact]
    public async Task DeleteAsync_CancelsPlatformReminderAndDeletesRecord()
    {
        var repository = new RecordingRepository();
        var notifications = new NotificationServiceSpy();
        var service = new AppointmentService(repository, notifications, new FixedTimeProvider(Now));

        await service.DeleteAsync("appointment-1");

        Assert.Contains("appointment-appointment-1", notifications.CancelledOccurrenceIds);
        Assert.Equal("appointment-1", repository.DeletedAppointmentId);
    }

    private static Appointment ValidAppointment() => new()
    {
        Id = "appointment-1",
        ProfileId = "profile-1",
        Title = "Routine visit",
        StartsUtc = new DateTime(2026, 8, 14, 10, 0, 0, DateTimeKind.Utc),
        TimeZoneId = TimeZoneInfo.Utc.Id,
        ReminderMinutesBefore = 60
    };

    private sealed class RecordingRepository : RepositoryStub
    {
        public Appointment? ExistingAppointment { get; init; }

        public IReadOnlyList<Appointment> Appointments { get; init; } = Array.Empty<Appointment>();

        public Appointment? SavedAppointment { get; private set; }

        public string? DeletedAppointmentId { get; private set; }

        public List<AuditEntry> AuditEntries { get; } = [];

        public override Task<Appointment?> GetAppointmentAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ExistingAppointment?.Id == id ? ExistingAppointment : null);
        }

        public override Task<IReadOnlyList<Appointment>> GetAppointmentsAsync(
            string? profileId = null,
            bool includeArchived = false,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(Appointments);
        }

        public override Task SaveAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            SavedAppointment = appointment;
            return Task.CompletedTask;
        }

        public override Task DeleteAppointmentAsync(string id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DeletedAppointmentId = id;
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
