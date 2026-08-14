namespace CareNest.UiTests;

public sealed class AppLockSecurityContractTests
{
    [Fact]
    public void AppLock_UsesSaltedPbkdf2Sha256AndFixedTimeVerification()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");

        Assert.Contains("private const int SaltSize = 16", source, StringComparison.Ordinal);
        Assert.Contains("private const int VerifierSize = 32", source, StringComparison.Ordinal);
        Assert.Contains("RandomNumberGenerator.GetBytes(SaltSize)", source, StringComparison.Ordinal);
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
        Assert.Contains("RestoreSnapshotOrThrowAsync(", source, StringComparison.Ordinal);
        Assert.Contains("rollbackFailures.Insert(0, primaryFailure)", source, StringComparison.Ordinal);
        Assert.Contains("CancellationToken.None", source, StringComparison.Ordinal);
    }

    [Fact]
    public void AppLock_DisableRestoresPreviousSecureStorageStateAfterRemovalFailure()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");
        var disableMethod = source[source.IndexOf(
            "public async Task DisableAsync(",
            StringComparison.Ordinal)..];

        Assert.Contains("previousEnabled = await secretStore.GetStringAsync", disableMethod, StringComparison.Ordinal);
        Assert.Contains("previousSalt = await secretStore.GetBytesAsync", disableMethod, StringComparison.Ordinal);
        Assert.Contains("previousVerifier = await secretStore.GetBytesAsync", disableMethod, StringComparison.Ordinal);
        Assert.Contains("catch (Exception disableFailure)", disableMethod, StringComparison.Ordinal);
        Assert.Contains("RestoreSnapshotOrThrowAsync(", disableMethod, StringComparison.Ordinal);
        Assert.Contains("ZeroIfPresent(previousSalt)", disableMethod, StringComparison.Ordinal);
        Assert.Contains("ZeroIfPresent(previousVerifier)", disableMethod, StringComparison.Ordinal);
    }

    [Fact]
    public void AppLock_VerificationFailsClosedForInvalidPinOrCorruptSecureMaterial()
    {
        var source = RepositoryLocator.Read("src", "CareNest.App", "Services", "AppLockService.cs");
        var verifyMethod = source[source.IndexOf(
            "public async Task<bool> VerifyPinAsync(",
            StringComparison.Ordinal)..source.IndexOf(
            "public async Task DisableAsync(",
            StringComparison.Ordinal)];

        Assert.Contains("if (!IsValidPin(pin))", verifyMethod, StringComparison.Ordinal);
        Assert.Contains("return false", verifyMethod, StringComparison.Ordinal);
        Assert.Contains("salt is not { Length: SaltSize }", verifyMethod, StringComparison.Ordinal);
        Assert.Contains("expected is not { Length: VerifierSize }", verifyMethod, StringComparison.Ordinal);
        Assert.Contains("VerifierSize", verifyMethod, StringComparison.Ordinal);
        Assert.DoesNotContain("expected.Length", verifyMethod, StringComparison.Ordinal);
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

        Assert.Contains("pin.Length is >= 6 and <= 32", source, StringComparison.Ordinal);
        Assert.Contains("pin.All(char.IsDigit)", source, StringComparison.Ordinal);
    }
}
