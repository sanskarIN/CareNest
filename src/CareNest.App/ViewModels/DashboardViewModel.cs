using System.Collections.ObjectModel;
using System.Windows.Input;
using CareNest.App.Navigation;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;

namespace CareNest.App.ViewModels;

public sealed record ProfileCareSummary(
    string ProfileName,
    int MedicineCount,
    int UpcomingReminderCount,
    int UpcomingAppointmentCount,
    int DocumentCount,
    int LowStockCount);

public sealed class DashboardViewModel : ObservableViewModel
{
    private readonly IReminderCoordinator _reminders;
    private readonly IAppointmentService _appointments;
    private readonly IProfileService _profiles;
    private readonly ICareNestRepository _repository;
    private readonly IAppNavigator _navigator;

    private Appointment? _nextAppointment;
    private int _profileCount;

    public DashboardViewModel(
        IReminderCoordinator reminders,
        IAppointmentService appointments,
        IProfileService profiles,
        ICareNestRepository repository,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _reminders = reminders;
        _appointments = appointments;
        _profiles = profiles;
        _repository = repository;
        _navigator = navigator;

        RefreshCommand = new AsyncCommand(() => LoadAsync());
        OpenProfilesCommand = new AsyncCommand(
            () => _navigator.GoToAsync($"//{RouteNames.Profiles}"));
        OpenLogCommand = new AsyncCommand(
            () => _navigator.GoToAsync($"//{RouteNames.MedicationLog}"));
        OpenReportsCommand = new AsyncCommand(
            () => _navigator.GoToAsync($"//{RouteNames.Reports}"));
    }

    public ObservableCollection<ReminderPreview> UpcomingReminders { get; } = [];
    public ObservableCollection<ProfileCareSummary> CaregiverProfiles { get; } = [];

    public Appointment? NextAppointment
    {
        get => _nextAppointment;
        private set
        {
            if (SetProperty(ref _nextAppointment, value))
            {
                OnPropertyChanged(nameof(HasNextAppointment));
            }
        }
    }

    public bool HasNextAppointment => NextAppointment is not null;

    public int ProfileCount
    {
        get => _profileCount;
        private set => SetProperty(ref _profileCount, value);
    }

    public ICommand RefreshCommand { get; }
    public ICommand OpenProfilesCommand { get; }
    public ICommand OpenLogCommand { get; }
    public ICommand OpenReportsCommand { get; }

    public Task LoadAsync() =>
        RunAsync(async ct =>
        {
            var reminders = await _reminders.GetUpcomingAsync(null, 8, ct);
            UpcomingReminders.Clear();
            foreach (var reminder in reminders)
            {
                UpcomingReminders.Add(reminder);
            }

            var appointments = await _appointments.ListAsync(null, ct);
            NextAppointment = appointments
                .Where(x => x.StartsUtc >= DateTime.UtcNow)
                .OrderBy(x => x.StartsUtc)
                .FirstOrDefault();

            var profileRows = await _profiles.ListAsync(ct);
            ProfileCount = profileRows.Count;

            CaregiverProfiles.Clear();
            foreach (var profile in profileRows)
            {
                var medicines = await _repository.GetMedicinesAsync(profile.Id, false, ct);
                var profileUpcoming = await _reminders.GetUpcomingAsync(profile.Id, 100, ct);
                var profileAppointments = await _repository.GetAppointmentsAsync(profile.Id, false, ct);
                var documents = await _repository.GetDocumentsAsync(profile.Id, ct);
                var lowStock = 0;
                foreach (var medicine in medicines.Where(x => x.RefillThreshold is not null))
                {
                    var estimated = await _repository.CalculateCurrentStockAsync(medicine.Id, ct);
                    if (estimated is not null && estimated <= medicine.RefillThreshold)
                    {
                        lowStock++;
                    }
                }

                CaregiverProfiles.Add(new ProfileCareSummary(
                    profile.Name,
                    medicines.Count,
                    profileUpcoming.Count,
                    profileAppointments.Count(x => x.StartsUtc >= DateTime.UtcNow),
                    documents.Count,
                    lowStock));
            }
        },
        "CareNest could not refresh the dashboard.");
}
