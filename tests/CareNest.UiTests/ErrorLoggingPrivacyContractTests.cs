namespace CareNest.UiTests;

public sealed class ErrorLoggingPrivacyContractTests
{
    [Fact]
    public void SafeUiErrorService_DoesNotPassExceptionObjectToLogger()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "SafeUiErrorService.cs");

        Assert.Contains("exception.GetType().FullName", source, StringComparison.Ordinal);
        Assert.DoesNotContain("logger.LogError(\n                exception,", source, StringComparison.Ordinal);
        Assert.DoesNotContain("exception.Message", source, StringComparison.Ordinal);
        Assert.DoesNotContain("exception.StackTrace", source, StringComparison.Ordinal);
    }

    [Fact]
    public void SafeUiErrorService_StillDisplaysOnlyCallerSuppliedSafeMessage()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "SafeUiErrorService.cs");

        Assert.Contains("DisplayAlertAsync(\"CareNest\", safeMessage, \"OK\")", source, StringComparison.Ordinal);
        Assert.DoesNotContain("DisplayAlertAsync(\"CareNest\", exception", source, StringComparison.Ordinal);
    }
}
