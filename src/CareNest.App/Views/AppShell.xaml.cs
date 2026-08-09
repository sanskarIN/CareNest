using CareNest.App.Navigation;

namespace CareNest.App.Views;

public partial class AppShell : Shell
{
    private readonly IServiceProvider _services;

    public AppShell(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;

        Items.Add(CreateItem("Home", RouteNames.Dashboard, services.GetRequiredService<DashboardPage>()));
        Items.Add(CreateItem("Profiles", RouteNames.Profiles, services.GetRequiredService<ProfilesPage>()));
        Items.Add(CreateItem("Medicines", RouteNames.Medicines, services.GetRequiredService<MedicinesPage>()));
        Items.Add(CreateItem("Medication log", RouteNames.MedicationLog, services.GetRequiredService<MedicationLogPage>()));
        Items.Add(CreateItem("Appointments", RouteNames.Appointments, services.GetRequiredService<AppointmentsPage>()));
        Items.Add(CreateItem("Documents", RouteNames.Documents, services.GetRequiredService<DocumentsPage>()));
        Items.Add(CreateItem("Reports", RouteNames.Reports, services.GetRequiredService<ReportsPage>()));
        Items.Add(CreateItem("Settings", RouteNames.Settings, services.GetRequiredService<SettingsPage>()));
        Items.Add(CreateItem("About", RouteNames.About, services.GetRequiredService<AboutPage>()));

        Routing.RegisterRoute(RouteNames.ProfileEditor, typeof(ProfileEditorPage));
        Routing.RegisterRoute(RouteNames.MedicineEditor, typeof(MedicineEditorPage));
        Routing.RegisterRoute(RouteNames.ScheduleEditor, typeof(ScheduleEditorPage));
        Routing.RegisterRoute(RouteNames.AppointmentEditor, typeof(AppointmentEditorPage));
    }

    private static FlyoutItem CreateItem(
        string title,
        string route,
        Page page)
    {
        var item = new FlyoutItem
        {
            Title = title,
            Route = route
        };

        item.Items.Add(new ShellContent
        {
            Title = title,
            Route = route + "-content",
            Content = page
        });

        return item;
    }
}
