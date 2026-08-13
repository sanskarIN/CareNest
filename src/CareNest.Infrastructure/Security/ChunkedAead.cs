using System.Buffers.Binary;
using System.Security.Cryptography;

namespace CareNest.Infrastructure.Security;

internal static class ChunkedAead
{
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private const int ChunkSize = 64 * 1024;
    private const byte LegacyVersion = 1;
    private const byte CurrentVersion = 2;
    private static readonly byte[] CurrentVersionBytes = [CurrentVersion];
    private static readonly byte[] ZeroLength = new byte[4];

    public static async Task EncryptAsync(
        Stream plaintext,
        Stream destination,
        ReadOnlyMemory<byte> key,
        ReadOnlyMemory<byte> magic,
        ReadOnlyMemory<byte> associatedData,
        CancellationToken cancellationToken)
    {
        ValidateKey(key);

        var baseNonce = RandomNumberGenerator.GetBytes(NonceSize);
        var plainBuffer = new byte[ChunkSize];
        var cipherBuffer = new byte[ChunkSize];
        var tag = new byte[TagSize];
        var terminalTag = new byte[TagSize];

        try
        {
            await destination.WriteAsync(magic, cancellationToken);
            await destination.WriteAsync(CurrentVersionBytes, cancellationToken);
            await destination.WriteAsync(baseNonce, cancellationToken);

            var counter = 0u;
            using var aes = new AesGcm(key.Span, TagSize);
            while (true)
            {
                var read = await ReadChunkAsync(plaintext, plainBuffer, cancellationToken);
                if (read == 0)
                {
                    break;
                }

                var nonce = BuildNonce(baseNonce, counter);
                var aad = BuildAad(associatedData.Span, counter, read);
                try
                {
                    aes.Encrypt(
                        nonce,
                        plainBuffer.AsSpan(0, read),
                        cipherBuffer.AsSpan(0, read),
                        tag,
                        aad);

                    var header = new byte[4];
                    BinaryPrimitives.WriteInt32LittleEndian(header, read);
                    await destination.WriteAsync(header, cancellationToken);
                    await destination.WriteAsync(tag, cancellationToken);
                    await destination.WriteAsync(cipherBuffer.AsMemory(0, read), cancellationToken);
                }
                finally
                {
                    CryptographicOperations.ZeroMemory(nonce);
                    CryptographicOperations.ZeroMemory(aad);
                    CryptographicOperations.ZeroMemory(plainBuffer.AsSpan(0, read));
                    CryptographicOperations.ZeroMemory(cipherBuffer.AsSpan(0, read));
                    CryptographicOperations.ZeroMemory(tag);
                }

                counter = IncrementCounter(counter);
            }

            var terminalNonce = BuildNonce(baseNonce, counter);
            var terminalAad = BuildAad(associatedData.Span, counter, 0);
            try
            {
                aes.Encrypt(
                    terminalNonce,
                    ReadOnlySpan<byte>.Empty,
                    Span<byte>.Empty,
                    terminalTag,
                    terminalAad);
                await destination.WriteAsync(ZeroLength, cancellationToken);
                await destination.WriteAsync(terminalTag, cancellationToken);
            }
            finally
            {
                CryptographicOperations.ZeroMemory(terminalNonce);
                CryptographicOperations.ZeroMemory(terminalAad);
                CryptographicOperations.ZeroMemory(terminalTag);
            }
        }
        finally
        {
            CryptographicOperations.ZeroMemory(baseNonce);
            CryptographicOperations.ZeroMemory(plainBuffer);
            CryptographicOperations.ZeroMemory(cipherBuffer);
            CryptographicOperations.ZeroMemory(tag);
            CryptographicOperations.ZeroMemory(terminalTag);
        }
    }

    public static async Task DecryptAsync(
        Stream source,
        Stream destination,
        ReadOnlyMemory<byte> key,
        ReadOnlyMemory<byte> expectedMagic,
        ReadOnlyMemory<byte> associatedData,
        CancellationToken cancellationToken)
    {
        ValidateKey(key);

        var magic = new byte[expectedMagic.Length];
        var baseNonce = new byte[NonceSize];
        var lengthBuffer = new byte[4];

        try
        {
            await ReadExactlyAsync(source, magic, cancellationToken);
            if (!CryptographicOperations.FixedTimeEquals(magic, expectedMagic.Span))
            {
                throw new InvalidDataException("Encrypted stream has an unsupported header.");
            }

            var version = source.ReadByte();
            if (version is not LegacyVersion and not CurrentVersion)
            {
                throw new InvalidDataException("Encrypted stream version is not supported.");
            }

            await ReadExactlyAsync(source, baseNonce, cancellationToken);

            var counter = 0u;
            using var aes = new AesGcm(key.Span, TagSize);

            while (true)
            {
                await ReadExactlyAsync(source, lengthBuffer, cancellationToken);
                var length = BinaryPrimitives.ReadInt32LittleEndian(lengthBuffer);
                if (length == 0)
                {
                    if (version == CurrentVersion)
                    {
                        await VerifyTerminalAsync(
                            source,
                            aes,
                            baseNonce,
                            associatedData,
                            counter,
                            cancellationToken);
                    }

                    EnsureEndOfStream(source);
                    break;
                }

                if (length < 0 || length > ChunkSize)
                {
                    throw new InvalidDataException("Encrypted chunk length is invalid.");
                }

                var tag = new byte[TagSize];
                var cipher = new byte[length];
                var plain = new byte[length];
                try
                {
                    await ReadExactlyAsync(source, tag, cancellationToken);
                    await ReadExactlyAsync(source, cipher, cancellationToken);

                    var nonce = BuildNonce(baseNonce, counter);
                    var aad = BuildAad(associatedData.Span, counter, length);
                    try
                    {
                        aes.Decrypt(nonce, cipher, tag, plain, aad);
                        await destination.WriteAsync(plain, cancellationToken);
                    }
                    finally
                    {
                        CryptographicOperations.ZeroMemory(nonce);
                        CryptographicOperations.ZeroMemory(aad);
                    }
                }
                finally
                {
                    CryptographicOperations.ZeroMemory(tag);
                    CryptographicOperations.ZeroMemory(cipher);
                    CryptographicOperations.ZeroMemory(plain);
                }

                counter = IncrementCounter(counter);
            }
        }
        finally
        {
            CryptographicOperations.ZeroMemory(magic);
            CryptographicOperations.ZeroMemory(baseNonce);
            CryptographicOperations.ZeroMemory(lengthBuffer);
        }
    }

    private static async Task VerifyTerminalAsync(
        Stream source,
        AesGcm aes,
        ReadOnlyMemory<byte> baseNonce,
        ReadOnlyMemory<byte> associatedData,
        uint counter,
        CancellationToken cancellationToken)
    {
        var terminalTag = new byte[TagSize];
        var nonce = BuildNonce(baseNonce.Span, counter);
        var aad = BuildAad(associatedData.Span, counter, 0);
        try
        {
            await ReadExactlyAsync(source, terminalTag, cancellationToken);
            aes.Decrypt(
                nonce,
                ReadOnlySpan<byte>.Empty,
                terminalTag,
                Span<byte>.Empty,
                aad);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(terminalTag);
            CryptographicOperations.ZeroMemory(nonce);
            CryptographicOperations.ZeroMemory(aad);
        }
    }

    private static void EnsureEndOfStream(Stream source)
    {
        if (source.ReadByte() != -1)
        {
            throw new InvalidDataException("Encrypted stream contains trailing data.");
        }
    }

    private static void ValidateKey(ReadOnlyMemory<byte> key)
    {
        if (key.Length != 32)
        {
            throw new ArgumentException("AES-256 requires a 32-byte key.", nameof(key));
        }
    }

    private static uint IncrementCounter(uint counter)
    {
        if (counter == uint.MaxValue)
        {
            throw new CryptographicException("Encrypted stream is too large.");
        }

        return counter + 1;
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

    private static async Task<int> ReadChunkAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var total = 0;
        while (total < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(total, buffer.Length - total), cancellationToken);
            if (read == 0)
            {
                break;
            }
            total += read;
        }
        return total;
    }

    private static async Task ReadExactlyAsync(Stream stream, Memory<byte> buffer, CancellationToken cancellationToken)
    {
        var total = 0;
        while (total < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer[total..], cancellationToken);
            if (read == 0)
            {
                throw new EndOfStreamException();
            }
            total += read;
        }
    }
}
