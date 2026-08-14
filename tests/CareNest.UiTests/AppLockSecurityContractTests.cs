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
    public void AppLock_ClearsNewAndRetrievedSaltVerifierBuffers()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");

        Assert.Contains("ZeroIfPresent(salt)", source, StringComparison.Ordinal);
        Assert.Contains("ZeroIfPresent(verifier)", source, StringComparison.Ordinal);
        Assert.Contains("ZeroIfPresent(previousSalt)", source, StringComparison.Ordinal);
        Assert.Contains("ZeroIfPresent(previousVerifier)", source, StringComparison.Ordinal);
        Assert.Contains("ZeroIfPresent(actual)", source, StringComparison.Ordinal);
        Assert.Contains("ZeroIfPresent(expected)", source, StringComparison.Ordinal);
        Assert.Contains("CryptographicOperations.ZeroMemory(value)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void AppLock_PinUpdateRestoresPreviousSecureStorageStateAfterWriteFailure()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");

        Assert.Contains("previousEnabled = await secretStore.GetStringAsync", source, StringComparison.Ordinal);
        Assert.Contains("previousSalt = await secretStore.GetBytesAsync", source, StringComparison.Ordinal);
        Assert.Contains("previousVerifier = await secretStore.GetBytesAsync", source, StringComparison.Ordinal);
        Assert.Contains("catch (Exception updateFailure)", source, StringComparison.Ordinal);
        Assert.Contains("RestoreBytesAsync(", source, StringComparison.Ordinal);
        Assert.Contains("RestoreStringAsync(", source, StringComparison.Ordinal);
        Assert.Contains("CancellationToken.None", source, StringComparison.Ordinal);
        Assert.Contains("rollbackFailures.Insert(0, updateFailure)", source, StringComparison.Ordinal);
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
