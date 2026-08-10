using CareNest.App.Services;
using CareNest.App.ViewModels;
using CareNest.App.Views;
using CareNest.Application.Contracts;
using CareNest.Application.Services;
using CareNest.Infrastructure.Backup;
using CareNest.Infrastructure.Configuration;
using CareNest.Infrastructure.Documents;
using CareNest.Infrastructure.Persistence;
using CareNest.Infrastructure.Reports;
using Microsoft.Extensions.Logging;

namespace CareNest.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var appData = FileSystem.Current.AppDataDirectory;
        var cache = FileSystem.Current.CacheDirectory;

        builder.Services.AddSingleton(new CareNestStorageOptions(
            Path.Combine(appData, "Data", "carenest.db"),
            Path.Combine(appData, "Documents"),
            Path.Combine(cache, "CareNestWork")));

        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddSingleton<SqliteDatabase>();
        builder.Services.AddSingleton<ICareNestRepository, CareNestRepository>();

        builder.Services.AddSingleton<ISecretStore, SecureSecretStore>();
        builder.Services.AddSingleton<IDocumentStore, EncryptedDocumentStore>();
        builder.Services.AddSingleton<IBackupService, EncryptedBackupService>();
        builder.Services.AddSingleton<IReportService, ReportService>();

        builder.Services.AddSingleton<ReminderPlanner>();
        builder.Services.AddSingleton<IReminderCoordinator, ReminderCoordinator>();
        builder.Services.AddSingleton<BackupReminderCoordinator>();
        builder.Services.AddSingleton<IProfileService, ProfileService>();
        builder.Services.AddSingleton<IMedicineService, MedicineService>();
        builder.Services.AddSingleton<IAppointmentService, AppointmentService>();
        builder.Services.AddSingleton<IDocumentService, DocumentService>();

        builder.Services.AddSingleton<INotificationService, PlatformNotificationService>();
        builder.Services.AddSingleton<IAppFileGateway, MauiFileGateway>();
        builder.Services.AddSingleton<IAppLockService, AppLockService>();
        builder.Services.AddSingleton<IAppNavigator, MauiNavigator>();
        builder.Services.AddSingleton<AppStateService>();
        builder.Services.AddSingleton<StartupCoordinator>();
        builder.Services.AddSingleton<SafeUiErrorService>();
        builder.Services.AddSingleton<GlobalExceptionHandler>();

        RegisterViewModels(builder.Services);
        RegisterViews(builder.Services);

        return builder.Build();
    }

    private static void RegisterViewModels(IServiceCollection services)
    {
        services.AddTransient<OnboardingViewModel>();
        services.AddTransient<LockViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<ProfilesViewModel>();
        services.AddTransient<ProfileEditorViewModel>();
        services.AddTransient<MedicinesViewModel>();
        services.AddTransient<MedicineEditorViewModel>();
        services.AddTransient<ScheduleEditorViewModel>();
        services.AddTransient<MedicationLogViewModel>();
        services.AddTransient<AppointmentsViewModel>();
        services.AddTransient<AppointmentEditorViewModel>();
        services.AddTransient<DocumentsViewModel>();
        services.AddTransient<ReportsViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<AboutViewModel>();
    }

    private static void RegisterViews(IServiceCollection services)
    {
        services.AddTransient<StartupPage>();
        services.AddTransient<OnboardingPage>();
        services.AddTransient<LockPage>();
        services.AddTransient<AppShell>();
        services.AddTransient<DashboardPage>();
        services.AddTransient<ProfilesPage>();
        services.AddTransient<ProfileEditorPage>();
        services.AddTransient<MedicinesPage>();
        services.AddTransient<MedicineEditorPage>();
        services.AddTransient<ScheduleEditorPage>();
        services.AddTransient<MedicationLogPage>();
        services.AddTransient<AppointmentsPage>();
        services.AddTransient<AppointmentEditorPage>();
        services.AddTransient<DocumentsPage>();
        services.AddTransient<ReportsPage>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<AboutPage>();
    }
}
