using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using CareNest.Infrastructure.Security;

namespace CareNest.IntegrationTests;

public sealed class ChunkedAeadTests
{
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private const int ChunkSize = 64 * 1024;
    private static readonly byte[] Magic = "TSAE"u8.ToArray();
    private static readonly byte[] Aad = "CareNest.Tests.ChunkedAead"u8.ToArray();

    [Fact]
    public async Task Version2_RoundTripsMultipleChunks()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var plaintext = RandomNumberGenerator.GetBytes(ChunkSize + 4096);
        try
        {
            await using var source = new MemoryStream(plaintext, writable: false);
            await using var encrypted = new MemoryStream();
            await ChunkedAead.EncryptAsync(source, encrypted, key, Magic, Aad, CancellationToken.None);

            var bytes = encrypted.ToArray();
            Assert.Equal(2, bytes[Magic.Length]);

            await using var encryptedInput = new MemoryStream(bytes, writable: false);
            await using var output = new MemoryStream();
            await ChunkedAead.DecryptAsync(encryptedInput, output, key, Magic, Aad, CancellationToken.None);

            Assert.Equal(plaintext, output.ToArray());
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
            CryptographicOperations.ZeroMemory(plaintext);
        }
    }

    [Fact]
    public async Task Version2_RejectsAuthenticatedPrefixTruncation()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var plaintext = RandomNumberGenerator.GetBytes(ChunkSize + 4096);
        try
        {
            await using var source = new MemoryStream(plaintext, writable: false);
            await using var encrypted = new MemoryStream();
            await ChunkedAead.EncryptAsync(source, encrypted, key, Magic, Aad, CancellationToken.None);
            var bytes = encrypted.ToArray();

            var streamHeaderLength = Magic.Length + 1 + NonceSize;
            var firstChunkLength = 4 + TagSize + ChunkSize;
            var secondChunkOffset = streamHeaderLength + firstChunkLength;
            var terminalLength = 4 + TagSize;
            var tampered = new byte[secondChunkOffset + terminalLength];
            bytes.AsSpan(0, secondChunkOffset).CopyTo(tampered);
            bytes.AsSpan(bytes.Length - terminalLength, terminalLength)
                .CopyTo(tampered.AsSpan(secondChunkOffset));

            await using var tamperedInput = new MemoryStream(tampered, writable: false);
            await using var output = new MemoryStream();

            await Assert.ThrowsAnyAsync<CryptographicException>(() =>
                ChunkedAead.DecryptAsync(tamperedInput, output, key, Magic, Aad, CancellationToken.None));
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
            CryptographicOperations.ZeroMemory(plaintext);
        }
    }

    [Fact]
    public async Task Version2_RejectsTrailingDataAfterAuthenticatedTerminal()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        try
        {
            await using var source = new MemoryStream(Encoding.UTF8.GetBytes("payload"), writable: false);
            await using var encrypted = new MemoryStream();
            await ChunkedAead.EncryptAsync(source, encrypted, key, Magic, Aad, CancellationToken.None);

            var bytes = encrypted.ToArray();
            Array.Resize(ref bytes, bytes.Length + 1);
            bytes[^1] = 0x7f;

            await using var tamperedInput = new MemoryStream(bytes, writable: false);
            await using var output = new MemoryStream();

            await Assert.ThrowsAsync<InvalidDataException>(() =>
                ChunkedAead.DecryptAsync(tamperedInput, output, key, Magic, Aad, CancellationToken.None));
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
        }
    }

    [Fact]
    public async Task LegacyVersion1_StreamStillDecrypts()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var plaintext = Encoding.UTF8.GetBytes("legacy CareNest encrypted stream");
        try
        {
            await using var legacy = await CreateLegacyVersion1Async(plaintext, key);
            await using var output = new MemoryStream();

            await ChunkedAead.DecryptAsync(legacy, output, key, Magic, Aad, CancellationToken.None);

            Assert.Equal(plaintext, output.ToArray());
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
            CryptographicOperations.ZeroMemory(plaintext);
        }
    }

    private static async Task<MemoryStream> CreateLegacyVersion1Async(byte[] plaintext, byte[] key)
    {
        var output = new MemoryStream();
        var baseNonce = RandomNumberGenerator.GetBytes(NonceSize);
        var tag = new byte[TagSize];
        var cipher = new byte[plaintext.Length];
        var nonce = BuildNonce(baseNonce, 0);
        var aad = BuildAad(Aad, 0, plaintext.Length);
        try
        {
            await output.WriteAsync(Magic);
            await output.WriteAsync(new byte[] { 1 });
            await output.WriteAsync(baseNonce);

            using var aes = new AesGcm(key, TagSize);
            aes.Encrypt(nonce, plaintext, cipher, tag, aad);

            var length = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(length, plaintext.Length);
            await output.WriteAsync(length);
            await output.WriteAsync(tag);
            await output.WriteAsync(cipher);
            await output.WriteAsync(new byte[4]);
            output.Position = 0;
            return output;
        }
        finally
        {
            CryptographicOperations.ZeroMemory(baseNonce);
            CryptographicOperations.ZeroMemory(tag);
            CryptographicOperations.ZeroMemory(cipher);
            CryptographicOperations.ZeroMemory(nonce);
            CryptographicOperations.ZeroMemory(aad);
        }
    }

    private static byte[] BuildNonce(ReadOnlySpan<byte> baseNonce, uint counter)
    {
        var nonce = baseNonce.ToArray();
        BinaryPrimitives.WriteUInt32LittleEndian(nonce.AsSpan(NonceSize - 4), counter);
        return nonce;
    }

    private static byte[] BuildAad(ReadOnlySpan<byte> prefix, uint counter, int length)
    {
        var result = new byte[prefix.Length + 8];
        prefix.CopyTo(result);
        BinaryPrimitives.WriteUInt32LittleEndian(result.AsSpan(prefix.Length, 4), counter);
        BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan(prefix.Length + 4, 4), length);
        return result;
    }
}
