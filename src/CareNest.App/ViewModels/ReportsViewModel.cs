using System.Collections.ObjectModel;
using System.Windows.Input;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;

namespace CareNest.App.ViewModels;

public sealed class ReportsViewModel : ObservableViewModel
{
    private readonly IProfileService _profiles;
    private readonly IReportService _reports;
    private readonly IAppFileGateway _files;
    private PersonProfile? _selectedProfile;

    public ReportsViewModel(
        IProfileService profiles,
        IReportService reports,
        IAppFileGateway files,
        SafeUiErrorService errors) : base(errors)
    {
        _profiles = profiles;
        _reports = reports;
        _files = files;
        ExportProfileDataCommand = new AsyncCommand(ExportProfileDataAsync);
        ExportProfileSummaryCommand = new AsyncCommand(ExportProfileSummaryAsync);
        ExportMedicationLogCommand = new AsyncCommand(() => ExportCsvAsync("medication-log", _reports.CreateMedicationLogCsvAsync));
        ExportUpcomingCommand = new AsyncCommand(() => ExportCsvAsync("upcoming-schedule", _reports.CreateUpcomingScheduleCsvAsync));
        ExportAppointmentsCommand = new AsyncCommand(() => ExportCsvAsync("appointment-history", _reports.CreateAppointmentHistoryCsvAsync));
        ExportDocumentsCommand = new AsyncCommand(() => ExportCsvAsync("document-list", _reports.CreateDocumentListCsvAsync));
        ExportStockRefillCommand = new AsyncCommand(() => ExportCsvAsync("stock-refill", _reports.CreateStockRefillCsvAsync));
        ExportMissedRemindersCommand = new AsyncCommand(() => ExportCsvAsync("missed-reminders", _reports.CreateMissedRemindersCsvAsync));
    }

    public ObservableCollection<PersonProfile> Profiles { get; } = [];
    public PersonProfile? SelectedProfile
    {
        get => _selectedProfile;
        set => SetProperty(ref _selectedProfile, value);
    }

    public ICommand ExportProfileDataCommand { get; }
    public ICommand ExportProfileSummaryCommand { get; }
    public ICommand ExportMedicationLogCommand { get; }
    public ICommand ExportUpcomingCommand { get; }
    public ICommand ExportAppointmentsCommand { get; }
    public ICommand ExportDocumentsCommand { get; }
    public ICommand ExportStockRefillCommand { get; }
    public ICommand ExportMissedRemindersCommand { get; }

    public Task LoadAsync() =>
        RunAsync(async ct =>
        {
            var selectedId = SelectedProfile?.Id;
            Profiles.Clear();
            foreach (var profile in await _profiles.ListAsync(ct))
            {
                Profiles.Add(profile);
            }

            SelectedProfile = selectedId is null
                ? Profiles.FirstOrDefault(x => x.IsPrimary) ?? Profiles.FirstOrDefault()
                : Profiles.FirstOrDefault(x => x.Id == selectedId)
                    ?? Profiles.FirstOrDefault(x => x.IsPrimary)
                    ?? Profiles.FirstOrDefault();

            StatusMessage = Profiles.Count == 0 ? "Create a profile before exporting reports." : null;
        }, "CareNest could not load report options.");

    private Task ExportProfileDataAsync() =>
        RunAsync(async ct =>
        {
            if (SelectedProfile is null)
            {
                throw new InvalidOperationException("Choose a profile first.");
            }

            var path = OutputPath("profile-data", "json");
            await _reports.CreateProfileDataJsonAsync(SelectedProfile.Id, path, ct);
            await _files.ShareFileAsync(path, "CareNest profile data export", ct);
            StatusMessage = "Profile data export created. Document contents are exported separately from Documents.";
        }, "CareNest could not create the profile data export.");

    private Task ExportProfileSummaryAsync() =>
        RunAsync(async ct =>
        {
            if (SelectedProfile is null)
            {
                throw new InvalidOperationException("Choose a profile first.");
            }

            var path = OutputPath("profile-summary", "pdf");
            await _reports.CreateProfileSummaryPdfAsync(SelectedProfile.Id, path, ct);
            await _files.ShareFileAsync(path, "CareNest profile summary", ct);
            StatusMessage = "Profile summary created. Review the privacy warning before sharing.";
        }, "CareNest could not create the profile summary.");

    private Task ExportCsvAsync(
        string baseName,
        Func<string?, string, CancellationToken, Task<string>> export) =>
        RunAsync(async ct =>
        {
            var path = OutputPath(baseName, "csv");
            await export(SelectedProfile?.Id, path, ct);
            await _files.ShareFileAsync(path, $"CareNest {baseName.Replace('-', ' ')}", ct);
            StatusMessage = "Report created from user-entered CareNest data.";
        }, "CareNest could not create the report.");

    private static string OutputPath(string baseName, string extension)
    {
        var directory = Path.Combine(FileSystem.Current.CacheDirectory, "Reports");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, $"carenest-{baseName}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.{extension}");
    }
}
