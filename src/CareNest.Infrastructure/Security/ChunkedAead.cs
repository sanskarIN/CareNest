using System.Buffers.Binary;
using System.Security.Cryptography;

namespace CareNest.Infrastructure.Security;

internal static class ChunkedAead
{
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private const int ChunkSize = 64 * 1024;

    public static async Task EncryptAsync(
        Stream plaintext,
        Stream destination,
        ReadOnlyMemory<byte> key,
        ReadOnlyMemory<byte> magic,
        ReadOnlyMemory<byte> associatedData,
        CancellationToken cancellationToken)
    {
        if (key.Length != 32)
        {
            throw new ArgumentException("AES-256 requires a 32-byte key.", nameof(key));
        }

        var baseNonce = RandomNumberGenerator.GetBytes(NonceSize);
        await destination.WriteAsync(magic, cancellationToken);
        await destination.WriteAsync(new byte[] { 1 }, cancellationToken);
        await destination.WriteAsync(baseNonce, cancellationToken);

        var plainBuffer = new byte[ChunkSize];
        var cipherBuffer = new byte[ChunkSize];
        var tag = new byte[TagSize];
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
            aes.Encrypt(nonce, plainBuffer.AsSpan(0, read), cipherBuffer.AsSpan(0, read), tag, aad);

            var header = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(header, read);
            await destination.WriteAsync(header, cancellationToken);
            await destination.WriteAsync(tag, cancellationToken);
            await destination.WriteAsync(cipherBuffer.AsMemory(0, read), cancellationToken);

            counter++;
            if (counter == uint.MaxValue)
            {
                throw new CryptographicException("Encrypted stream is too large.");
            }
        }

        await destination.WriteAsync(new byte[4], cancellationToken);
    }

    public static async Task DecryptAsync(
        Stream source,
        Stream destination,
        ReadOnlyMemory<byte> key,
        ReadOnlyMemory<byte> expectedMagic,
        ReadOnlyMemory<byte> associatedData,
        CancellationToken cancellationToken)
    {
        var magic = new byte[expectedMagic.Length];
        await ReadExactlyAsync(source, magic, cancellationToken);
        if (!CryptographicOperations.FixedTimeEquals(magic, expectedMagic.Span))
        {
            throw new InvalidDataException("Encrypted stream has an unsupported header.");
        }

        var version = source.ReadByte();
        if (version != 1)
        {
            throw new InvalidDataException("Encrypted stream version is not supported.");
        }

        var baseNonce = new byte[NonceSize];
        await ReadExactlyAsync(source, baseNonce, cancellationToken);

        var counter = 0u;
        var lengthBuffer = new byte[4];
        using var aes = new AesGcm(key.Span, TagSize);

        while (true)
        {
            await ReadExactlyAsync(source, lengthBuffer, cancellationToken);
            var length = BinaryPrimitives.ReadInt32LittleEndian(lengthBuffer);
            if (length == 0)
            {
                break;
            }
            if (length < 0 || length > ChunkSize)
            {
                throw new InvalidDataException("Encrypted chunk length is invalid.");
            }

            var tag = new byte[TagSize];
            var cipher = new byte[length];
            var plain = new byte[length];
            await ReadExactlyAsync(source, tag, cancellationToken);
            await ReadExactlyAsync(source, cipher, cancellationToken);

            var nonce = BuildNonce(baseNonce, counter);
            var aad = BuildAad(associatedData.Span, counter, length);
            aes.Decrypt(nonce, cipher, tag, plain, aad);
            await destination.WriteAsync(plain, cancellationToken);
            counter++;
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
