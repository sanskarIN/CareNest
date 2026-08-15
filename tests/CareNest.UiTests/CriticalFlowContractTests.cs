using System.Xml.Linq;

namespace CareNest.UiTests;

public sealed class CriticalFlowContractTests
{
    [Fact]
    public void AllMauiPages_AreWellFormedXaml()
    {
        var root = Path.Combine(RepositoryLocator.Root, "src", "CareNest.App");
        var xaml = Directory.EnumerateFiles(root, "*.xaml", SearchOption.AllDirectories).ToArray();
        Assert.NotEmpty(xaml);

        foreach (var path in xaml)
        {
            var exception = Record.Exception(() => XDocument.Load(path));
            Assert.True(exception is null, $"{Path.GetRelativePath(root, path)} is not well-formed XML: {exception}");
        }
    }

    [Fact]
    public void Onboarding_ContainsLocalFirstBackupAndMedicalLimitations()
    {
        var xaml = RepositoryLocator.Read("src", "CareNest.App", "Views", "OnboardingPage.xaml");

        Assert.Contains("Local-first storage", xaml, StringComparison.Ordinal);
        Assert.Contains("BackupResponsibility", xaml, StringComparison.Ordinal);
        Assert.Contains("MedicalDisclaimer", xaml, StringComparison.Ordinal);
        Assert.Contains("ReminderLimitations", xaml, StringComparison.Ordinal);
        Assert.Contains("DisclaimerAccepted", xaml, StringComparison.Ordinal);
    }

    [Fact]
    public void Onboarding_DoesNotRequestNotificationPermission()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "OnboardingViewModel.cs");

        Assert.DoesNotContain("RequestPermissionAsync", source, StringComparison.Ordinal);
        Assert.DoesNotContain("INotificationService", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ScheduleSave_IsTheFirstMedicineReminderPermissionSurface()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "ScheduleEditorViewModel.cs");

        Assert.Contains("RequestPermissionAsync", source, StringComparison.Ordinal);
        Assert.Contains("Kind != ScheduleKind.AsNeeded", source, StringComparison.Ordinal);
    }

    [Fact]
    public void About_RepeatsMedicalAndEmergencyLimitations()
    {
        var xaml = RepositoryLocator.Read("src", "CareNest.App", "Views", "AboutPage.xaml");

        Assert.Contains("does not provide medical diagnosis", xaml, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("In an emergency", xaml, StringComparison.Ordinal);
        Assert.Contains("MadeBy", xaml, StringComparison.Ordinal);
    }

    [Fact]
    public void About_VoluntaryFundingSurface_IsBuildConfigurable()
    {
        var project = RepositoryLocator.Read("src", "CareNest.App", "CareNest.App.csproj");
        var source = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "AboutViewModel.cs");
        var enabled = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "FundingLinkPolicy.Enabled.cs");
        var disabled = RepositoryLocator.Read("src", "CareNest.App", "ViewModels", "FundingLinkPolicy.Disabled.cs");
        var sharedConstants = RepositoryLocator.Read("src", "CareNest.Shared", "AppConstants.cs");
        var xaml = RepositoryLocator.Read("src", "CareNest.App", "Views", "AboutPage.xaml");

        Assert.Contains("CareNestShowFundingLink", project, StringComparison.Ordinal);
        Assert.Contains("FundingLinkPolicy.Enabled.cs", project, StringComparison.Ordinal);
        Assert.Contains("FundingLinkPolicy.Disabled.cs", project, StringComparison.Ordinal);
        Assert.DoesNotContain("CARENEST_FUNDING_LINK", project, StringComparison.Ordinal);
        Assert.DoesNotContain("buymeacoffee.com", source, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("FundingLinkPolicy.CreateCommand(OpenAsync)", source, StringComparison.Ordinal);
        Assert.Contains("FundingLinkPolicy.IsVisible", source, StringComparison.Ordinal);
        Assert.Contains("https://buymeacoffee.com/sanskarIN", enabled, StringComparison.Ordinal);
        Assert.DoesNotContain("buymeacoffee.com", disabled, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("AppConstants.FundingUrl", source, StringComparison.Ordinal);
        Assert.DoesNotContain("buymeacoffee.com", sharedConstants, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("IsVisible=\"{Binding IsProjectSupportVisible}\"", xaml, StringComparison.Ordinal);
        Assert.Contains("Project support is voluntary", xaml, StringComparison.Ordinal);
        Assert.Contains("does not unlock medical advice", xaml, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ResetData_UsesTwoConfirmations()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Views", "SettingsPage.xaml.cs");

        var confirmations = source.Split("DisplayAlertAsync", StringSplitOptions.None).Length - 1;
        Assert.True(confirmations >= 3, "Settings should include restore/reset safety confirmations, including two-step reset.");
        Assert.Contains("Final confirmation", source, StringComparison.Ordinal);
        Assert.Contains("Delete everything", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Shell_ExposesAllPrimaryOrganizerAreas()
    {
        var shell = RepositoryLocator.Read("src", "CareNest.App", "Views", "AppShell.xaml.cs");
        foreach (var title in new[]
                 {
                     "Home", "Profiles", "Medicines", "Medication log",
                     "Appointments", "Documents", "Reports", "Settings", "About"
                 })
        {
            Assert.Contains($"\"{title}\"", shell, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void DocumentUi_StatesNoMedicalInterpretation()
    {
        var xaml = RepositoryLocator.Read("src", "CareNest.App", "Views", "DocumentsPage.xaml");
        Assert.Contains("does not interpret", xaml, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("encrypted", xaml, StringComparison.OrdinalIgnoreCase);
    }
}
