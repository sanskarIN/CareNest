namespace CareNest.UiTests;

public sealed class OnboardingRollbackContractTests
{
    [Fact]
    public void Onboarding_ValidatesOptionalPinBeforeCreatingProfile()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "OnboardingViewModel.cs");

        var pinCheck = source.IndexOf("if (EnableLock && !IsValidPin(Pin))", StringComparison.Ordinal);
        var profileSave = source.IndexOf("await _profiles.SaveAsync(profile, ct)", StringComparison.Ordinal);

        Assert.True(pinCheck >= 0);
        Assert.True(profileSave > pinCheck);
    }

    [Fact]
    public void Onboarding_SetsCompletionOnlyAfterProfileLockAndDefaultPreferenceWork()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "OnboardingViewModel.cs");

        var profileSave = source.IndexOf("await _profiles.SaveAsync(profile, ct)", StringComparison.Ordinal);
        var lockSet = source.IndexOf("await _appLock.SetPinAsync(Pin, ct)", StringComparison.Ordinal);
        var genericDefault = source.IndexOf("SettingKeys.GenericNotificationLabels", StringComparison.Ordinal);
        var complete = source.IndexOf("await _appState.SetOnboardingCompleteAsync(ct)", StringComparison.Ordinal);

        Assert.True(profileSave >= 0);
        Assert.True(lockSet > profileSave);
        Assert.True(genericDefault > profileSave);
        Assert.True(complete > genericDefault);
    }

    [Fact]
    public void OnboardingFailure_AttemptsLockProfileAndCompletionRollbackWithoutCallerCancellation()
    {
        var source = RepositoryLocator.Read(
            "src",
            "CareNest.App",
            "ViewModels",
            "OnboardingViewModel.cs");
        var rollback = source[source.IndexOf("catch (Exception onboardingFailure)", StringComparison.Ordinal)..];

        Assert.Contains("await _appLock.DisableAsync(CancellationToken.None)", rollback, StringComparison.Ordinal);
        Assert.Contains("await _profiles.DeleteAsync(profile.Id, CancellationToken.None)", rollback, StringComparison.Ordinal);
        Assert.Contains("SettingKeys.OnboardingComplete", rollback, StringComparison.Ordinal);
        Assert.Contains("CancellationToken.None", rollback, StringComparison.Ordinal);
        Assert.Contains("rollbackFailures.Insert(0, onboardingFailure)", rollback, StringComparison.Ordinal);
    }
}
