using CareNest.Domain.Entities;

namespace CareNest.Application.Contracts;

public interface IProfileService
{
    Task<IReadOnlyList<PersonProfile>> ListAsync(CancellationToken cancellationToken = default);
    Task<PersonProfile?> GetAsync(string id, CancellationToken cancellationToken = default);
    Task SaveAsync(PersonProfile profile, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IMedicineService
{
    Task<IReadOnlyList<Medicine>> ListAsync(string? profileId = null, CancellationToken cancellationToken = default);
    Task<Medicine?> GetAsync(string id, CancellationToken cancellationToken = default);
    Task SaveAsync(Medicine medicine, CancellationToken cancellationToken = default);
    Task SaveScheduleAsync(MedicineSchedule schedule, IReadOnlyCollection<ScheduleTime> times, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task ApplyStockAdjustmentAsync(string medicineId, decimal delta, string? reason, string? logEntryId = null, CancellationToken cancellationToken = default);
}

public interface IAppointmentService
{
    Task<IReadOnlyList<Appointment>> ListAsync(string? profileId = null, CancellationToken cancellationToken = default);
    Task SaveAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task RebuildRemindersAsync(CancellationToken cancellationToken = default);
}

public interface IDocumentService
{
    Task<IReadOnlyList<CareDocument>> ListAsync(string? profileId = null, CancellationToken cancellationToken = default);
    Task<CareDocument> ImportAsync(string profileId, string title, Domain.Enums.DocumentCategory category, string? notes, PickedFile file, CancellationToken cancellationToken = default);
    Task<string> ExportToTemporaryFileAsync(string documentId, string temporaryDirectory, CancellationToken cancellationToken = default);
    Task DeleteAsync(string documentId, CancellationToken cancellationToken = default);
}
