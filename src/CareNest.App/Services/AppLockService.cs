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

        var salt = RandomNumberGenerator.GetBytes(16);
        var verifier = Rfc2898DeriveBytes.Pbkdf2(
            pin,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            32);

        await secretStore.SetBytesAsync(SaltKey, salt, cancellationToken);
        await secretStore.SetBytesAsync(VerifierKey, verifier, cancellationToken);
        await secretStore.SetStringAsync(EnabledKey, "1", cancellationToken);
        CryptographicOperations.ZeroMemory(verifier);
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

        if (salt is null || expected is null)
        {
            if (expected is not null)
            {
                CryptographicOperations.ZeroMemory(expected);
            }

            return false;
        }

        var actual = Rfc2898DeriveBytes.Pbkdf2(
            pin,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            expected.Length);

        try
        {
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(actual);
            CryptographicOperations.ZeroMemory(expected);
        }
    }

    public async Task DisableAsync(
        CancellationToken cancellationToken = default)
    {
        await secretStore.RemoveAsync(EnabledKey, cancellationToken);
        await secretStore.RemoveAsync(SaltKey, cancellationToken);
        await secretStore.RemoveAsync(VerifierKey, cancellationToken);
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
