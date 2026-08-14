using System.Security.Cryptography;
using CareNest.Application.Contracts;

namespace CareNest.App.Services;

public sealed class AppLockService(ISecretStore secretStore) : IAppLockService
{
    private const string EnabledKey = "applock.enabled";
    private const string SaltKey = "applock.salt";
    private const string VerifierKey = "applock.verifier";
    private const int Iterations = 210_000;

    public async Task<bool> IsEnabledAsync(
        CancellationToken cancellationToken = default) =>
        string.Equals(
            await secretStore.GetStringAsync(EnabledKey, cancellationToken),
            "1",
            StringComparison.Ordinal);

    public async Task SetPinAsync(
        string pin,
        CancellationToken cancellationToken = default)
    {
        ValidatePin(pin);

        string? previousEnabled = null;
        byte[]? previousSalt = null;
        byte[]? previousVerifier = null;
        byte[]? salt = null;
        byte[]? verifier = null;

        try
        {
            previousEnabled = await secretStore.GetStringAsync(
                EnabledKey,
                cancellationToken);
            previousSalt = await secretStore.GetBytesAsync(
                SaltKey,
                cancellationToken);
            previousVerifier = await secretStore.GetBytesAsync(
                VerifierKey,
                cancellationToken);

            salt = RandomNumberGenerator.GetBytes(16);
            verifier = Rfc2898DeriveBytes.Pbkdf2(
                pin,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                32);

            try
            {
                await secretStore.SetBytesAsync(
                    SaltKey,
                    salt,
                    cancellationToken);
                await secretStore.SetBytesAsync(
                    VerifierKey,
                    verifier,
                    cancellationToken);
                await secretStore.SetStringAsync(
                    EnabledKey,
                    "1",
                    cancellationToken);
            }
            catch (Exception updateFailure)
            {
                await RestoreSnapshotOrThrowAsync(
                    previousEnabled,
                    previousSalt,
                    previousVerifier,
                    updateFailure,
                    "The app-lock PIN update failed and the previous secure-storage state could not be fully restored.");
                throw;
            }
        }
        finally
        {
            ZeroIfPresent(salt);
            ZeroIfPresent(verifier);
            ZeroIfPresent(previousSalt);
            ZeroIfPresent(previousVerifier);
        }
    }

    public async Task<bool> VerifyPinAsync(
        string pin,
        CancellationToken cancellationToken = default)
    {
        if (!await IsEnabledAsync(cancellationToken))
        {
            return true;
        }

        var salt = await secretStore.GetBytesAsync(SaltKey, cancellationToken);
        var expected = await secretStore.GetBytesAsync(VerifierKey, cancellationToken);
        byte[]? actual = null;

        try
        {
            if (salt is null || expected is null)
            {
                return false;
            }

            actual = Rfc2898DeriveBytes.Pbkdf2(
                pin,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                expected.Length);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        finally
        {
            ZeroIfPresent(actual);
            ZeroIfPresent(salt);
            ZeroIfPresent(expected);
        }
    }

    public async Task DisableAsync(
        CancellationToken cancellationToken = default)
    {
        string? previousEnabled = null;
        byte[]? previousSalt = null;
        byte[]? previousVerifier = null;

        try
        {
            previousEnabled = await secretStore.GetStringAsync(
                EnabledKey,
                cancellationToken);
            previousSalt = await secretStore.GetBytesAsync(
                SaltKey,
                cancellationToken);
            previousVerifier = await secretStore.GetBytesAsync(
                VerifierKey,
                cancellationToken);

            try
            {
                await secretStore.RemoveAsync(SaltKey, cancellationToken);
                await secretStore.RemoveAsync(VerifierKey, cancellationToken);
                await secretStore.RemoveAsync(EnabledKey, cancellationToken);
            }
            catch (Exception disableFailure)
            {
                await RestoreSnapshotOrThrowAsync(
                    previousEnabled,
                    previousSalt,
                    previousVerifier,
                    disableFailure,
                    "Disabling app lock failed and the previous secure-storage state could not be fully restored.");
                throw;
            }
        }
        finally
        {
            ZeroIfPresent(previousSalt);
            ZeroIfPresent(previousVerifier);
        }
    }

    private async Task RestoreSnapshotOrThrowAsync(
        string? enabled,
        byte[]? salt,
        byte[]? verifier,
        Exception primaryFailure,
        string aggregateMessage)
    {
        var rollbackFailures = new List<Exception>();
        await RestoreBytesAsync(SaltKey, salt, rollbackFailures);
        await RestoreBytesAsync(VerifierKey, verifier, rollbackFailures);
        await RestoreStringAsync(EnabledKey, enabled, rollbackFailures);

        if (rollbackFailures.Count > 0)
        {
            rollbackFailures.Insert(0, primaryFailure);
            throw new AggregateException(aggregateMessage, rollbackFailures);
        }
    }

    private async Task RestoreBytesAsync(
        string key,
        byte[]? value,
        ICollection<Exception> failures)
    {
        try
        {
            if (value is null)
            {
                await secretStore.RemoveAsync(key, CancellationToken.None);
            }
            else
            {
                await secretStore.SetBytesAsync(key, value, CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            failures.Add(ex);
        }
    }

    private async Task RestoreStringAsync(
        string key,
        string? value,
        ICollection<Exception> failures)
    {
        try
        {
            if (value is null)
            {
                await secretStore.RemoveAsync(key, CancellationToken.None);
            }
            else
            {
                await secretStore.SetStringAsync(key, value, CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            failures.Add(ex);
        }
    }

    private static void ZeroIfPresent(byte[]? value)
    {
        if (value is not null)
        {
            CryptographicOperations.ZeroMemory(value);
        }
    }

    private static void ValidatePin(string pin)
    {
        if (pin.Length is < 6 or > 32 ||
            !pin.All(char.IsDigit))
        {
            throw new ArgumentException(
                "Use a numeric app-lock PIN containing 6 to 32 digits.",
                nameof(pin));
        }
    }
}
