using System.Windows.Input;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;
using CareNest.Domain.Enums;

namespace CareNest.App.ViewModels;

public sealed class ScheduleEditorViewModel : ObservableViewModel
{
    private static readonly char[] ReminderTimeSeparators = [',', ';', '\n'];

    private readonly IMedicineService _medicines;
    private readonly ICareNestRepository _repository;
    private readonly INotificationService _notifications;
    private readonly IAppNavigator _navigator;

    private string? _medicineId;
    private string? _scheduleId;
    private ScheduleKind _kind = ScheduleKind.Daily;
    private DateTime _startDate = DateTime.Today;
    private DateTime _endDate = DateTime.Today.AddMonths(1);
    private bool _hasEndDate;
    private TimeSpan _reminderTime = new(9, 0, 0);
    private string _reminderTimesText = "09:00";
    private string _intervalHoursText = "8";
    private string _cycleOnDaysText = "1";
    private string _cycleOffDaysText = "1";
    private bool _monday = true;
    private bool _tuesday = true;
    private bool _wednesday = true;
    private bool _thursday = true;
    private bool _friday = true;
    private bool _saturday = true;
    private bool _sunday = true;
    private bool _followUpEnabled;
    private string _followUpMinutesText = "15";
    private bool _enabled = true;
    private string _timeZoneId = TimeZoneInfo.Local.Id;

    public ScheduleEditorViewModel(
        IMedicineService medicines,
        ICareNestRepository repository,
        INotificationService notifications,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _medicines = medicines;
        _repository = repository;
        _notifications = notifications;
        _navigator = navigator;
        SaveCommand = new AsyncCommand(SaveAsync);
    }

    public IReadOnlyList<ScheduleKind> Kinds { get; } =
        Enum.GetValues<ScheduleKind>();

    public ScheduleKind Kind
    {
        get => _kind;
        set
        {
            if (SetProperty(ref _kind, value))
            {
                OnPropertyChanged(nameof(IsInterval));
                OnPropertyChanged(nameof(IsWeekdaySelection));
                OnPropertyChanged(nameof(IsCycle));
                OnPropertyChanged(nameof(HasAutomaticReminder));
                OnPropertyChanged(nameof(IsSpecificTimes));
            }
        }
    }

    public DateTime StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }
    public DateTime EndDate { get => _endDate; set => SetProperty(ref _endDate, value); }
    public bool HasEndDate { get => _hasEndDate; set => SetProperty(ref _hasEndDate, value); }
    public TimeSpan ReminderTime { get => _reminderTime; set => SetProperty(ref _reminderTime, value); }
    public string ReminderTimesText { get => _reminderTimesText; set => SetProperty(ref _reminderTimesText, value); }
    public string IntervalHoursText { get => _intervalHoursText; set => SetProperty(ref _intervalHoursText, value); }
    public string CycleOnDaysText { get => _cycleOnDaysText; set => SetProperty(ref _cycleOnDaysText, value); }
    public string CycleOffDaysText { get => _cycleOffDaysText; set => SetProperty(ref _cycleOffDaysText, value); }
    public bool Monday { get => _monday; set => SetProperty(ref _monday, value); }
    public bool Tuesday { get => _tuesday; set => SetProperty(ref _tuesday, value); }
    public bool Wednesday { get => _wednesday; set => SetProperty(ref _wednesday, value); }
    public bool Thursday { get => _thursday; set => SetProperty(ref _thursday, value); }
    public bool Friday { get => _friday; set => SetProperty(ref _friday, value); }
    public bool Saturday { get => _saturday; set => SetProperty(ref _saturday, value); }
    public bool Sunday { get => _sunday; set => SetProperty(ref _sunday, value); }
    public bool FollowUpEnabled { get => _followUpEnabled; set => SetProperty(ref _followUpEnabled, value); }
    public string FollowUpMinutesText { get => _followUpMinutesText; set => SetProperty(ref _followUpMinutesText, value); }
    public bool Enabled { get => _enabled; set => SetProperty(ref _enabled, value); }
    public string TimeZoneId { get => _timeZoneId; set => SetProperty(ref _timeZoneId, value); }

    public bool IsInterval => Kind == ScheduleKind.EveryNHours;
    public bool IsWeekdaySelection => Kind == ScheduleKind.SelectedWeekdays;
    public bool IsCycle => Kind == ScheduleKind.Cycle;
    public bool HasAutomaticReminder => Kind != ScheduleKind.AsNeeded;
    public bool IsSpecificTimes => HasAutomaticReminder && !IsInterval;

    public ICommand SaveCommand { get; }

    public async Task LoadAsync(string medicineId)
    {
        _medicineId = medicineId;

        await RunAsync(async ct =>
        {
            var medicine = await _medicines.GetAsync(medicineId, ct)
                ?? throw new InvalidOperationException("Medicine record was not found.");

            StartDate = medicine.StartDate;
            EndDate = medicine.EndDate ?? medicine.StartDate.AddMonths(1);
            HasEndDate = medicine.EndDate is not null;

            var schedules = await _repository.GetSchedulesForMedicineAsync(
                medicineId,
                ct);

            var schedule = schedules.Count == 0 ? null : schedules[0];
            if (schedule is null)
            {
                TimeZoneId = TimeZoneInfo.Local.Id;
                return;
            }

            _scheduleId = schedule.Id;
            Kind = schedule.Kind;
            StartDate = schedule.StartDate;
            EndDate = schedule.EndDate ?? schedule.StartDate.AddMonths(1);
            HasEndDate = schedule.EndDate is not null;
            IntervalHoursText = schedule.IntervalHours?.ToString() ?? "8";
            CycleOnDaysText = schedule.CycleOnDays?.ToString() ?? "1";
            CycleOffDaysText = schedule.CycleOffDays?.ToString() ?? "1";
            FollowUpEnabled = schedule.FollowUpMinutes is not null;
            FollowUpMinutesText = schedule.FollowUpMinutes?.ToString() ?? "15";
            Enabled = schedule.Enabled;
            TimeZoneId = schedule.TimeZoneId;

            Monday = HasWeekday(schedule.WeekdayMask, DayOfWeek.Monday);
            Tuesday = HasWeekday(schedule.WeekdayMask, DayOfWeek.Tuesday);
            Wednesday = HasWeekday(schedule.WeekdayMask, DayOfWeek.Wednesday);
            Thursday = HasWeekday(schedule.WeekdayMask, DayOfWeek.Thursday);
            Friday = HasWeekday(schedule.WeekdayMask, DayOfWeek.Friday);
            Saturday = HasWeekday(schedule.WeekdayMask, DayOfWeek.Saturday);
            Sunday = HasWeekday(schedule.WeekdayMask, DayOfWeek.Sunday);

            var times = await _repository.GetScheduleTimesAsync(
                schedule.Id,
                ct);

            var orderedTimes = times.OrderBy(x => x.Hour).ThenBy(x => x.Minute).ToArray();
            var firstTime = orderedTimes.Length == 0 ? null : orderedTimes[0];
            if (firstTime is not null)
            {
                ReminderTime = new TimeSpan(firstTime.Hour, firstTime.Minute, 0);
                ReminderTimesText = string.Join(", ", orderedTimes.Select(x => $"{x.Hour:00}:{x.Minute:00}"));
            }
        },
        "CareNest could not load this reminder schedule.");
    }

    private Task SaveAsync() =>
        RunAsync(async ct =>
        {
            if (_medicineId is null)
            {
                throw new InvalidOperationException(
                    "Medicine record is missing.");
            }

            int? interval = Kind == ScheduleKind.EveryNHours
                ? ParseRequiredPositiveInt(IntervalHoursText, "Interval hours")
                : null;

            int? cycleOn = Kind == ScheduleKind.Cycle
                ? ParseRequiredPositiveInt(CycleOnDaysText, "Cycle on-days")
                : null;

            int? cycleOff = Kind == ScheduleKind.Cycle
                ? ParseRequiredPositiveInt(CycleOffDaysText, "Cycle off-days")
                : null;

            int? followUp = FollowUpEnabled && Kind != ScheduleKind.AsNeeded
                ? ParseRequiredPositiveInt(FollowUpMinutesText, "Follow-up minutes")
                : null;

            var schedule = _scheduleId is null
                ? new MedicineSchedule()
                : (await _repository.GetSchedulesForMedicineAsync(
                        _medicineId,
                        ct))
                    .FirstOrDefault(x => x.Id == _scheduleId)
                    ?? new MedicineSchedule { Id = _scheduleId };

            schedule.MedicineId = _medicineId;
            schedule.Kind = Kind;
            schedule.StartDate = StartDate.Date;
            schedule.EndDate = HasEndDate ? EndDate.Date : null;
            schedule.IntervalHours = interval;
            schedule.CycleOnDays = cycleOn;
            schedule.CycleOffDays = cycleOff;
            schedule.WeekdayMask = BuildWeekdayMask();
            schedule.TimeZoneId = TimeZoneId.Trim();
            schedule.FollowUpMinutes = followUp;
            schedule.Enabled = Enabled;

            IReadOnlyCollection<ScheduleTime> times = Kind switch
            {
                ScheduleKind.AsNeeded => Array.Empty<ScheduleTime>(),
                ScheduleKind.EveryNHours => new[]
                {
                    new ScheduleTime
                    {
                        Hour = ReminderTime.Hours,
                        Minute = ReminderTime.Minutes
                    }
                },
                _ => ParseReminderTimes(ReminderTimesText)
            };

            if (Kind != ScheduleKind.AsNeeded && Enabled)
            {
                var granted = await _notifications.RequestPermissionAsync(ct);
                if (!granted)
                {
                    StatusMessage =
                        "The schedule will be saved, but notifications are not currently permitted. You can review diagnostics in Settings.";
                }
            }

            await _medicines.SaveScheduleAsync(
                schedule,
                times,
                ct);

            _scheduleId = schedule.Id;

            if (string.IsNullOrWhiteSpace(StatusMessage))
            {
                StatusMessage = "Reminder schedule saved.";
            }

            await _navigator.GoBackAsync(ct);
        },
        "CareNest could not save this schedule. Check the selected times and schedule rules.");

    private int BuildWeekdayMask()
    {
        var mask = 0;

        Add(DayOfWeek.Monday, Monday);
        Add(DayOfWeek.Tuesday, Tuesday);
        Add(DayOfWeek.Wednesday, Wednesday);
        Add(DayOfWeek.Thursday, Thursday);
        Add(DayOfWeek.Friday, Friday);
        Add(DayOfWeek.Saturday, Saturday);
        Add(DayOfWeek.Sunday, Sunday);

        return mask;

        void Add(DayOfWeek day, bool enabled)
        {
            if (enabled)
            {
                mask |= 1 << (int)day;
            }
        }
    }

    private static bool HasWeekday(
        int mask,
        DayOfWeek day) =>
        (mask & (1 << (int)day)) != 0;

    private static IReadOnlyCollection<ScheduleTime> ParseReminderTimes(string value)
    {
        var parts = value.Split(
            ReminderTimeSeparators,
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length == 0)
        {
            throw new ArgumentException("Enter at least one reminder time using HH:mm format.");
        }

        var result = new List<ScheduleTime>();
        foreach (var part in parts)
        {
            if (!TimeOnly.TryParseExact(part, "HH:mm", out var time) &&
                !TimeOnly.TryParse(part, out time))
            {
                throw new ArgumentException($"'{part}' is not a valid time. Use HH:mm, for example 08:00.");
            }

            if (result.Any(x => x.Hour == time.Hour && x.Minute == time.Minute))
            {
                continue;
            }

            result.Add(new ScheduleTime { Hour = time.Hour, Minute = time.Minute });
        }

        return result.OrderBy(x => x.Hour).ThenBy(x => x.Minute).ToArray();
    }

    private static int ParseRequiredPositiveInt(
        string value,
        string label)
    {
        if (!int.TryParse(value, out var parsed) ||
            parsed <= 0)
        {
            throw new ArgumentException(
                $"{label} must be a positive whole number.");
        }

        return parsed;
    }
}
