using System.Collections.ObjectModel;
using System.Windows.Input;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.App.ViewModels;

public sealed record MedicationLogRow(
    string Id,
    string MedicineName,
    string ProfileName,
    MedicationLogStatus Status,
    DateTime EventUtc,
    string? Note,
    bool ManuallyEdited);

public sealed class MedicationLogViewModel : ObservableViewModel
{
    private readonly IReminderCoordinator _reminders;
    private readonly ICareNestRepository _repository;
    private readonly TimeProvider _timeProvider;
    private PersonProfile? _selectedProfile;
    private Medicine? _selectedMedicine;
    private bool _filterSingleDate;
    private DateTime _filterDate = DateTime.Today;

    public MedicationLogViewModel(
        IReminderCoordinator reminders,
        ICareNestRepository repository,
        TimeProvider timeProvider,
        SafeUiErrorService errors) : base(errors)
    {
        _reminders = reminders;
        _repository = repository;
        _timeProvider = timeProvider;

        RefreshCommand = new AsyncCommand(LoadAsync);
        ApplyFilterCommand = new AsyncCommand(LoadAsync);
        ClearFilterCommand = new AsyncCommand(ClearFilterAsync);
        TakenCommand = new AsyncCommand<ReminderPreview>(
            item => ChangeReminderAsync(item, ReminderState.Taken));
        SkippedCommand = new AsyncCommand<ReminderPreview>(
            item => ChangeReminderAsync(item, ReminderState.Skipped));
        DelayedCommand = new AsyncCommand<ReminderPreview>(
            item => ChangeReminderAsync(item, ReminderState.Delayed));
        SnoozeCommand = new AsyncCommand<ReminderPreview>(
            SnoozeAsync);
    }

    public ObservableCollection<PersonProfile> Profiles { get; } = [];
    public ObservableCollection<Medicine> Medicines { get; } = [];
    public ObservableCollection<ReminderPreview> Upcoming { get; } = [];
    public ObservableCollection<MedicationLogRow> Entries { get; } = [];

    public PersonProfile? SelectedProfile { get => _selectedProfile; set => SetProperty(ref _selectedProfile, value); }
    public Medicine? SelectedMedicine { get => _selectedMedicine; set => SetProperty(ref _selectedMedicine, value); }
    public bool FilterSingleDate { get => _filterSingleDate; set => SetProperty(ref _filterSingleDate, value); }
    public DateTime FilterDate { get => _filterDate; set => SetProperty(ref _filterDate, value); }

    public ICommand RefreshCommand { get; }
    public ICommand ApplyFilterCommand { get; }
    public ICommand ClearFilterCommand { get; }
    public ICommand TakenCommand { get; }
    public ICommand SkippedCommand { get; }
    public ICommand DelayedCommand { get; }
    public ICommand SnoozeCommand { get; }

    public Task LoadAsync() =>
        RunAsync(async ct =>
        {
            await _reminders.MarkOverdueAsMissedAsync(ct);

            var upcoming = await _reminders.GetUpcomingAsync(
                null,
                25,
                ct);

            Upcoming.Clear();
            foreach (var item in upcoming)
            {
                Upcoming.Add(item);
            }

            var profileRows = await _repository.GetProfilesAsync(true, ct);
            var medicineRows = await _repository.GetMedicinesAsync(null, true, ct);

            if (Profiles.Count == 0)
            {
                foreach (var p in profileRows) Profiles.Add(p);
            }
            if (Medicines.Count == 0)
            {
                foreach (var m in medicineRows) Medicines.Add(m);
            }

            DateTime? fromUtc;
            DateTime? toUtc;
            if (FilterSingleDate)
            {
                var localStart = DateTime.SpecifyKind(FilterDate.Date, DateTimeKind.Unspecified);
                var localEnd = localStart.AddDays(1);
                fromUtc = TimeZoneInfo.ConvertTimeToUtc(localStart, TimeZoneInfo.Local);
                toUtc = TimeZoneInfo.ConvertTimeToUtc(localEnd, TimeZoneInfo.Local);
            }
            else
            {
                fromUtc = _timeProvider.GetUtcNow().UtcDateTime.AddDays(-90);
                toUtc = null;
            }

            var logs = await _repository.GetMedicationLogAsync(
                SelectedProfile?.Id,
                SelectedMedicine?.Id,
                fromUtc,
                toUtc,
                ct);

            var medicines = medicineRows.ToDictionary(x => x.Id);
            var profiles = profileRows.ToDictionary(x => x.Id);

            Entries.Clear();
            foreach (var log in logs.Take(250))
            {
                Entries.Add(new MedicationLogRow(
                    log.Id,
                    medicines.TryGetValue(log.MedicineId, out var medicine)
                        ? medicine.Name
                        : "Unknown medicine",
                    profiles.TryGetValue(log.ProfileId, out var profile)
                        ? profile.Name
                        : "Unknown profile",
                    log.Status,
                    log.EventUtc,
                    log.Note,
                    log.ManuallyEdited));
            }
        },
        "CareNest could not load the medication log.");


    private Task ClearFilterAsync()
    {
        SelectedProfile = null;
        SelectedMedicine = null;
        FilterSingleDate = false;
        FilterDate = DateTime.Today;
        return LoadAsync();
    }


    public async Task<string> GetEditHistoryAsync(string id, CancellationToken cancellationToken = default)
    {
        var history = await _repository.GetAuditEntriesAsync(
            nameof(MedicationLogEntry),
            id,
            cancellationToken);

        if (history.Count == 0)
        {
            return "No manual edit history is recorded for this entry.";
        }

        return string.Join(
            Environment.NewLine,
            history.Select(x =>
                $"{x.EventUtc:u} — {x.SafeSummary ?? x.Action.ToString()} — changed: {x.ChangedFieldsCsv ?? "not specified"}"));
    }

    public Task EditEntryAsync(
        string id,
        MedicationLogStatus status,
        string? note) =>
        RunAsync(async ct =>
        {
            var entries = await _repository.GetMedicationLogAsync(
                cancellationToken: ct);

            var entry = entries.FirstOrDefault(x => x.Id == id)
                ?? throw new InvalidOperationException(
                    "Medication log entry was not found.");

            entry.Status = status;
            entry.Note = string.IsNullOrWhiteSpace(note)
                ? null
                : note.Trim();
            entry.ManuallyEdited = true;
            entry.UpdatedUtc = _timeProvider.GetUtcNow().UtcDateTime;

            await _repository.SaveMedicationLogEntryAsync(
                entry,
                ct);

            await _repository.AddAuditEntryAsync(new AuditEntry
            {
                EntityType = nameof(MedicationLogEntry),
                EntityId = entry.Id,
                Action = AuditAction.Updated,
                EventUtc = _timeProvider.GetUtcNow().UtcDateTime,
                ChangedFieldsCsv = "Status,Note",
                SafeSummary = "Medication log entry manually edited"
            }, ct);

            await LoadAsync();
        },
        "CareNest could not update this medication log entry.");

    private Task ChangeReminderAsync(
        ReminderPreview? item,
        ReminderState state)
    {
        if (item is null)
        {
            return Task.CompletedTask;
        }

        return RunAsync(async ct =>
        {
            await _reminders.HandleOccurrenceAsync(
                item.OccurrenceId,
                state,
                cancellationToken: ct);

            await LoadAsync();
        },
        "CareNest could not update this reminder.");
    }

    private Task SnoozeAsync(ReminderPreview? item)
    {
        if (item is null)
        {
            return Task.CompletedTask;
        }

        return RunAsync(async ct =>
        {
            var until = _timeProvider
                .GetUtcNow()
                .UtcDateTime
                .AddMinutes(10);

            await _reminders.HandleOccurrenceAsync(
                item.OccurrenceId,
                ReminderState.Snoozed,
                until,
                cancellationToken: ct);

            await LoadAsync();
        },
        "CareNest could not snooze this reminder.");
    }
}
