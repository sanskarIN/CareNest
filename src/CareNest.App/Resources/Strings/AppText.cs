using System.Globalization;
using System.Resources;

namespace CareNest.App.Resources.Strings;

public static class AppText
{
    private static readonly ResourceManager Manager = new(
        "CareNest.App.Resources.Strings.AppResources",
        typeof(AppText).Assembly);

    public static string ProductName => Get(nameof(ProductName));
    public static string OnboardingWelcome => Get(nameof(OnboardingWelcome));
    public static string LocalFirstDescription => Get(nameof(LocalFirstDescription));
    public static string BackupResponsibility => Get(nameof(BackupResponsibility));
    public static string MedicalDisclaimer => Get(nameof(MedicalDisclaimer));
    public static string ReminderLimitations => Get(nameof(ReminderLimitations));
    public static string MadeBy => Get(nameof(MadeBy));

    private static string Get(string key) =>
        Manager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
}
