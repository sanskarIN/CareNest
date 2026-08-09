using System.Collections.ObjectModel;
using System.Windows.Input;
using CareNest.App.Navigation;
using CareNest.App.Services;
using CareNest.Application.Contracts;
using CareNest.Domain.Entities;

namespace CareNest.App.ViewModels;

public sealed record AppointmentRow(
    string Id,
    string Title,
    string ProfileName,
    DateTime StartsUtc,
    string? ClinicianOrFacility,
    string? Location,
    bool Upcoming);

public sealed class AppointmentsViewModel : ObservableViewModel
{
    private readonly IAppointmentService _appointments;
    private readonly IProfileService _profiles;
    private readonly IAppNavigator _navigator;

    public AppointmentsViewModel(
        IAppointmentService appointments,
        IProfileService profiles,
        IAppNavigator navigator,
        SafeUiErrorService errors) : base(errors)
    {
        _appointments = appointments;
        _profiles = profiles;
        _navigator = navigator;

        AddCommand = new AsyncCommand(
            () => _navigator.GoToAsync(RouteNames.AppointmentEditor));

        EditCommand = new AsyncCommand<AppointmentRow>(
            row => row is null
                ? Task.CompletedTask
                : _navigator.GoToAsync(
                    RouteNames.AppointmentEditor,
                    new Dictionary<string, object>
                    {
                        ["AppointmentId"] = row.Id
                    }));
    }

    public ObservableCollection<AppointmentRow> Upcoming { get; } = [];
    public ObservableCollection<AppointmentRow> History { get; } = [];
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }

    public Task LoadAsync() =>
        RunAsync(async ct =>
        {
            var profiles = (await _profiles.ListAsync(ct))
                .ToDictionary(x => x.Id);

            var appointments = await _appointments.ListAsync(null, ct);
            var rows = appointments
                .Select(x => new AppointmentRow(
                    x.Id,
                    x.Title,
                    profiles.TryGetValue(x.ProfileId, out var profile)
                        ? profile.Name
                        : "Unknown profile",
                    x.StartsUtc,
                    x.ClinicianOrFacility,
                    x.Location,
                    x.StartsUtc >= DateTime.UtcNow))
                .ToArray();

            Upcoming.Clear();
            foreach (var row in rows
                .Where(x => x.Upcoming)
                .OrderBy(x => x.StartsUtc))
            {
                Upcoming.Add(row);
            }

            History.Clear();
            foreach (var row in rows
                .Where(x => !x.Upcoming)
                .OrderByDescending(x => x.StartsUtc))
            {
                History.Add(row);
            }
        },
        "CareNest could not load appointments.");
}
