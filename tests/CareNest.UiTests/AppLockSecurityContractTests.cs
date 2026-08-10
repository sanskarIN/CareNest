namespace CareNest.UiTests;

public sealed class AppLockSecurityContractTests
{
    [Fact]
    public void AppLock_UsesSaltedPbkdf2Sha256AndFixedTimeVerification()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");

        Assert.Contains("RandomNumberGenerator.GetBytes(16)", source, StringComparison.Ordinal);
        Assert.Contains("Rfc2898DeriveBytes.Pbkdf2", source, StringComparison.Ordinal);
        Assert.Contains("210_000", source, StringComparison.Ordinal);
        Assert.Contains("HashAlgorithmName.SHA256", source, StringComparison.Ordinal);
        Assert.Contains("CryptographicOperations.FixedTimeEquals", source, StringComparison.Ordinal);
    }

    [Fact]
    public void AppLock_VerificationClearsDerivedAndStoredVerifierBuffers()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");

        Assert.Contains("CryptographicOperations.ZeroMemory(actual)", source, StringComparison.Ordinal);
        Assert.Contains("CryptographicOperations.ZeroMemory(expected)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void AppLock_DoesNotPersistPlaintextPin()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");

        Assert.DoesNotContain("SetStringAsync(EnabledKey, pin", source, StringComparison.Ordinal);
        Assert.DoesNotContain("SetStringAsync(SaltKey, pin", source, StringComparison.Ordinal);
        Assert.DoesNotContain("SetStringAsync(VerifierKey, pin", source, StringComparison.Ordinal);
        Assert.DoesNotContain("= pin;", source, StringComparison.Ordinal);
    }

    [Fact]
    public void AppLock_DisableRemovesAllStoredLockMaterial()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");

        Assert.Contains("RemoveAsync(EnabledKey", source, StringComparison.Ordinal);
        Assert.Contains("RemoveAsync(SaltKey", source, StringComparison.Ordinal);
        Assert.Contains("RemoveAsync(VerifierKey", source, StringComparison.Ordinal);
    }

    [Fact]
    public void AppLock_PinPolicyRemainsNumericSixToThirtyTwoDigits()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");

        Assert.Contains("pin.Length is < 6 or > 32", source, StringComparison.Ordinal);
        Assert.Contains("pin.All(char.IsDigit)", source, StringComparison.Ordinal);
    }
}
