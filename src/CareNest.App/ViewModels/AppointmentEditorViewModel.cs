using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;

namespace CareNest.App.ViewModels;

public sealed class AppointmentEditorViewModel : ObservableViewModel
{
    private readonly IAppointmentService _appointments;
    private readonly IProfileService _profiles;
    private readonly IDocumentService _documents;
    private readonly ICareNestRepository _repository;
    private readonly IAppFileGateway _files;
    private readonly IAppNavigator _navigator;

    private string? _appointmentId;
    private PersonProfile? _selectedProfile;
    private string _title = string.Empty;
    private string _clinicianOrFacility = string.Empty;
    private DateTime _date = DateTime.Today.AddDays(1);
    private TimeSpan _time = new(10, 0, 0);
    private string _timeZoneId = TimeZoneInfo.Local.Id;
    private string _location = string.Empty;
    private string _preparationNotes = string.Empty;
    private string _questionsToAsk = string.Empty;
    private bool _hasFollowUp;
    private DateTime _followUpDate = DateTime.Today.AddMonths(1);
    private bool _hasReminder = true;
    private string _reminderMinutesText = "60";
    private bool _archived;
    private bool _isExisting;
    private string? _attachmentDocumentId;
    private string _attachmentLabel = "No attachment";

    public AppointmentEditorViewModel(
        IAppointmentService appointments,
        IProfileService profiles,
        IDocumentService documents,
        ICareNestRepository repository,
        IAppFileGateway files,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _appointments = appointments;
        _profiles = profiles;
        _documents = documents;
        _repository = repository;
        _files = files;
        _navigator = navigator;

        SaveCommand = new AsyncCommand(SaveAsync);
        ExportCalendarCommand = new AsyncCommand(ExportCalendarAsync);
        AttachDocumentCommand = new AsyncCommand(AttachDocumentAsync);
        ExportAttachmentCommand = new AsyncCommand(ExportAttachmentAsync);
        DetachDocumentCommand = new AsyncCommand(DetachDocumentAsync);
    }

    public ObservableCollection<PersonProfile> Profiles { get; } = [];

    public PersonProfile? SelectedProfile { get => _selectedProfile; set => SetProperty(ref _selectedProfile, value); }
    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public string ClinicianOrFacility { get => _clinicianOrFacility; set => SetProperty(ref _clinicianOrFacility, value); }
    public DateTime Date { get => _date; set => SetProperty(ref _date, value); }
    public TimeSpan Time { get => _time; set => SetProperty(ref _time, value); }
    public string TimeZoneId { get => _timeZoneId; set => SetProperty(ref _timeZoneId, value); }
    public string Location { get => _location; set => SetProperty(ref _location, value); }
    public string PreparationNotes { get => _preparationNotes; set => SetProperty(ref _preparationNotes, value); }
    public string QuestionsToAsk { get => _questionsToAsk; set => SetProperty(ref _questionsToAsk, value); }
    public bool HasFollowUp { get => _hasFollowUp; set => SetProperty(ref _hasFollowUp, value); }
    public DateTime FollowUpDate { get => _followUpDate; set => SetProperty(ref _followUpDate, value); }
    public bool HasReminder { get => _hasReminder; set => SetProperty(ref _hasReminder, value); }
    public string ReminderMinutesText { get => _reminderMinutesText; set => SetProperty(ref _reminderMinutesText, value); }
    public bool Archived { get => _archived; set => SetProperty(ref _archived, value); }
    public bool IsExisting { get => _isExisting; private set => SetProperty(ref _isExisting, value); }
    public string AttachmentLabel { get => _attachmentLabel; private set => SetProperty(ref _attachmentLabel, value); }
    public bool HasAttachment => !string.IsNullOrWhiteSpace(_attachmentDocumentId);

    public ICommand SaveCommand { get; }
    public ICommand ExportCalendarCommand { get; }
    public ICommand AttachDocumentCommand { get; }
    public ICommand ExportAttachmentCommand { get; }
    public ICommand DetachDocumentCommand { get; }

    public async Task LoadAsync(string? appointmentId)
    {
        _appointmentId = string.IsNullOrWhiteSpace(appointmentId)
            ? null
            : appointmentId;

        await RunAsync(async ct =>
        {
            Profiles.Clear();
            foreach (var profile in await _profiles.ListAsync(ct))
            {
                Profiles.Add(profile);
            }

            if (_appointmentId is null)
            {
                SelectedProfile = Profiles.FirstOrDefault(x => x.IsPrimary)
                    ?? Profiles.FirstOrDefault();
                IsExisting = false;
                return;
            }

            var appointment = await _repository.GetAppointmentAsync(
                _appointmentId,
                ct)
                ?? throw new InvalidOperationException(
                    "Appointment was not found.");

            SelectedProfile = Profiles.FirstOrDefault(
                x => x.Id == appointment.ProfileId);
            Title = appointment.Title;
            ClinicianOrFacility = appointment.ClinicianOrFacility ?? string.Empty;
            TimeZoneId = appointment.TimeZoneId;

            var zone = TimeZoneInfo.FindSystemTimeZoneById(
                appointment.TimeZoneId);
            var local = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(
                    appointment.StartsUtc,
                    DateTimeKind.Utc),
                zone);

            Date = local.Date;
            Time = local.TimeOfDay;
            Location = appointment.Location ?? string.Empty;
            PreparationNotes = appointment.PreparationNotes ?? string.Empty;
            QuestionsToAsk = appointment.QuestionsToAsk ?? string.Empty;
            HasFollowUp = appointment.FollowUpDate is not null;
            FollowUpDate = appointment.FollowUpDate ?? DateTime.Today.AddMonths(1);
            HasReminder = appointment.ReminderMinutesBefore is not null;
            ReminderMinutesText = appointment.ReminderMinutesBefore?.ToString() ?? "60";
            Archived = appointment.Archived;
            _attachmentDocumentId = appointment.AttachmentDocumentId;
            if (!string.IsNullOrWhiteSpace(_attachmentDocumentId))
            {
                var attachment = await _repository.GetDocumentAsync(_attachmentDocumentId, ct);
                AttachmentLabel = attachment?.Title ?? "Attached document";
            }
            OnPropertyChanged(nameof(HasAttachment));
            IsExisting = true;
        },
        "CareNest could not load this appointment.");
    }

    public Task DeleteAsync() =>
        RunAsync(async ct =>
        {
            if (_appointmentId is null)
            {
                await _navigator.GoBackAsync(ct);
                return;
            }

            await _appointments.DeleteAsync(
                _appointmentId,
                ct);

            await _navigator.GoBackAsync(ct);
        },
        "CareNest could not delete this appointment.");

    private Task SaveAsync() =>
        RunAsync(async ct =>
        {
            if (SelectedProfile is null)
            {
                StatusMessage = "Choose a local profile.";
                return;
            }

            var zone = TimeZoneInfo.FindSystemTimeZoneById(
                TimeZoneId.Trim());
            var local = DateTime.SpecifyKind(
                Date.Date.Add(Time),
                DateTimeKind.Unspecified);

            if (zone.IsInvalidTime(local))
            {
                throw new ArgumentException(
                    "The selected local time does not exist because of a daylight-saving transition. Choose another time.");
            }

            DateTime startsUtc;
            if (zone.IsAmbiguousTime(local))
            {
                var offset = zone.GetAmbiguousTimeOffsets(local).Max();
                startsUtc = new DateTimeOffset(local, offset).UtcDateTime;
            }
            else
            {
                startsUtc = TimeZoneInfo.ConvertTimeToUtc(local, zone);
            }

            int? reminder = null;
            if (HasReminder)
            {
                if (!int.TryParse(ReminderMinutesText, out var parsed) ||
                    parsed < 0 ||
                    parsed > 43_200)
                {
                    throw new ArgumentException(
                        "Reminder lead time must be between 0 and 43,200 minutes.");
                }

                reminder = parsed;
            }

            var appointment = _appointmentId is null
                ? new Appointment()
                : await _repository.GetAppointmentAsync(
                    _appointmentId,
                    ct)
                    ?? throw new InvalidOperationException(
                        "Appointment was not found.");

            appointment.ProfileId = SelectedProfile.Id;
            appointment.Title = Title;
            appointment.ClinicianOrFacility = NullIfBlank(ClinicianOrFacility);
            appointment.StartsUtc = startsUtc;
            appointment.TimeZoneId = zone.Id;
            appointment.Location = NullIfBlank(Location);
            appointment.PreparationNotes = NullIfBlank(PreparationNotes);
            appointment.QuestionsToAsk = NullIfBlank(QuestionsToAsk);
            appointment.AttachmentDocumentId = _attachmentDocumentId;
            appointment.FollowUpDate = HasFollowUp
                ? FollowUpDate.Date
                : null;
            appointment.ReminderMinutesBefore = reminder;
            appointment.Archived = Archived;

            await _appointments.SaveAsync(
                appointment,
                ct);

            _appointmentId = appointment.Id;
            IsExisting = true;
            await _navigator.GoBackAsync(ct);
        },
        "CareNest could not save this appointment. Check the date, time, time zone, and reminder fields.");


    private Task AttachDocumentAsync() =>
        RunAsync(async ct =>
        {
            if (SelectedProfile is null)
            {
                throw new InvalidOperationException("Choose a profile before importing an appointment attachment.");
            }

            var picked = await _files.PickDocumentAsync(ct);
            if (picked is null) return;

            var title = string.IsNullOrWhiteSpace(Title)
                ? $"Appointment attachment - {picked.FileName}"
                : $"{Title.Trim()} - {picked.FileName}";

            var document = await _documents.ImportAsync(
                SelectedProfile.Id,
                title,
                CareNest.Domain.Enums.DocumentCategory.Custom,
                "Imported as an appointment attachment. CareNest does not interpret this file.",
                picked,
                ct);

            _attachmentDocumentId = document.Id;
            AttachmentLabel = document.Title;
            OnPropertyChanged(nameof(HasAttachment));
            StatusMessage = "Attachment encrypted and stored locally. Save the appointment to keep the association.";
        }, "CareNest could not import the appointment attachment.");

    private Task ExportAttachmentAsync() =>
        RunAsync(async ct =>
        {
            if (string.IsNullOrWhiteSpace(_attachmentDocumentId)) return;
            var directory = Path.Combine(FileSystem.Current.CacheDirectory, "Exports");
            Directory.CreateDirectory(directory);
            var path = await _documents.ExportToTemporaryFileAsync(_attachmentDocumentId, directory, ct);
            await _files.ShareFileAsync(path, "Export CareNest appointment attachment", ct);
        }, "CareNest could not export the appointment attachment.");

    private Task DetachDocumentAsync() =>
        RunAsync(ct =>
        {
            ct.ThrowIfCancellationRequested();
            _attachmentDocumentId = null;
            AttachmentLabel = "No attachment";
            OnPropertyChanged(nameof(HasAttachment));
            StatusMessage = "Attachment detached from the appointment. The encrypted document remains in Documents until you delete it there.";
            return Task.CompletedTask;
        }, "CareNest could not detach the appointment document.");

    private Task ExportCalendarAsync() =>
        RunAsync(async ct =>
        {
            if (_appointmentId is null)
            {
                StatusMessage = "Save the appointment before exporting it.";
                return;
            }

            var appointment = await _repository.GetAppointmentAsync(
                _appointmentId,
                ct)
                ?? throw new InvalidOperationException(
                    "Appointment was not found.");

            var start = DateTime.SpecifyKind(
                appointment.StartsUtc,
                DateTimeKind.Utc);
            var end = start.AddHours(1);

            static string Escape(string? value) =>
                (value ?? string.Empty)
                    .Replace("\\", "\\\\", StringComparison.Ordinal)
                    .Replace(";", "\\;", StringComparison.Ordinal)
                    .Replace(",", "\\,", StringComparison.Ordinal)
                    .Replace("\r", string.Empty, StringComparison.Ordinal)
                    .Replace("\n", "\\n", StringComparison.Ordinal);

            var ics = new StringBuilder()
                .AppendLine("BEGIN:VCALENDAR")
                .AppendLine("VERSION:2.0")
                .AppendLine("PRODID:-//CareNest//Appointment Export//EN")
                .AppendLine("BEGIN:VEVENT")
                .AppendLine($"UID:{appointment.Id}@carenest.local")
                .AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMdd'T'HHmmss'Z'}")
                .AppendLine($"DTSTART:{start:yyyyMMdd'T'HHmmss'Z'}")
                .AppendLine($"DTEND:{end:yyyyMMdd'T'HHmmss'Z'}")
                .AppendLine($"SUMMARY:{Escape(appointment.Title)}")
                .AppendLine($"LOCATION:{Escape(appointment.Location)}")
                .AppendLine($"DESCRIPTION:{Escape(appointment.PreparationNotes)}")
                .AppendLine("END:VEVENT")
                .AppendLine("END:VCALENDAR")
                .ToString();

            var path = Path.Combine(
                FileSystem.Current.CacheDirectory,
                $"CareNest-appointment-{appointment.Id}.ics");

            await File.WriteAllTextAsync(
                path,
                ics,
                Encoding.UTF8,
                ct);

            await _files.ShareFileAsync(
                path,
                "Export CareNest appointment",
                ct);
        },
        "CareNest could not create the calendar export.");

    private static string? NullIfBlank(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
}
