using System.Text;
using CareNest.Application.Contracts;
using CareNest.Infrastructure.Configuration;
using CareNest.Infrastructure.Documents;
using CareNest.Shared;

namespace CareNest.IntegrationTests;

public sealed class EncryptedDocumentStoreTests
{
    [Fact]
    public async Task Import_StoresCiphertext_AndExportsOriginalBytes()
    {
        await using var store = await TestStore.CreateAsync();
        var plain = Encoding.UTF8.GetBytes("private test document bytes");
        await using var input = new MemoryStream(plain);

        var stored = await store.Documents.ImportAsync(input, "report.txt", "text/plain");

        var raw = await File.ReadAllBytesAsync(Path.Combine(store.Options.DocumentDirectory, stored.EncryptedFileName));
        Assert.DoesNotContain("private test document bytes", Encoding.UTF8.GetString(raw));

        await using var output = new MemoryStream();
        await store.Documents.ExportDecryptedAsync(stored.EncryptedFileName, output);
        Assert.Equal(plain, output.ToArray());

        var key = await store.Secrets.GetBytesAsync(SecretKeys.DocumentMasterKey);
        Assert.NotNull(key);
        Assert.Equal(32, key!.Length);
    }

    [Fact]
    public async Task TamperedCiphertext_FailsAuthentication()
    {
        await using var store = await TestStore.CreateAsync();
        await using var input = new MemoryStream(Encoding.UTF8.GetBytes("document"));
        var stored = await store.Documents.ImportAsync(input, "a.txt", "text/plain");
        var path = Path.Combine(store.Options.DocumentDirectory, stored.EncryptedFileName);
        var bytes = await File.ReadAllBytesAsync(path);
        bytes[^1] ^= 0x40;
        await File.WriteAllBytesAsync(path, bytes);

        await using var output = new MemoryStream();
        await Assert.ThrowsAnyAsync<Exception>(() =>
            store.Documents.ExportDecryptedAsync(stored.EncryptedFileName, output));
    }

    [Fact]
    public async Task ImportAndExport_ClearCallerOwnedDocumentKeyBuffers()
    {
        var root = Path.Combine(Path.GetTempPath(), "CareNestDocumentKeyTests", Guid.NewGuid().ToString("N"));
        var options = new CareNestStorageOptions(
            Path.Combine(root, "Data", "carenest.db"),
            Path.Combine(root, "Documents"),
            Path.Combine(root, "Work"));
        options.EnsureDirectories();
        var secrets = new TrackingSecretStore();
        var documents = new EncryptedDocumentStore(options, secrets);

        try
        {
            await using var input = new MemoryStream(Encoding.UTF8.GetBytes("key hygiene test"));
            var stored = await documents.ImportAsync(input, "key-test.txt", "text/plain");

            var setBuffer = Assert.IsType<byte[]>(secrets.LastSetBuffer);
            Assert.All(setBuffer, value => Assert.Equal(0, value));

            await using var output = new MemoryStream();
            await documents.ExportDecryptedAsync(stored.EncryptedFileName, output);

            var getBuffer = Assert.IsType<byte[]>(secrets.LastGetBuffer);
            Assert.All(getBuffer, value => Assert.Equal(0, value));
            Assert.Equal("key hygiene test", Encoding.UTF8.GetString(output.ToArray()));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    private sealed class TrackingSecretStore : ISecretStore
    {
        private byte[]? _storedBytes;
        private string? _storedString;

        public byte[]? LastSetBuffer { get; private set; }

        public byte[]? LastGetBuffer { get; private set; }

        public Task<byte[]?> GetBytesAsync(string key, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_storedBytes is null)
            {
                return Task.FromResult<byte[]?>(null);
            }

            LastGetBuffer = _storedBytes.ToArray();
            return Task.FromResult<byte[]?>(LastGetBuffer);
        }

        public Task SetBytesAsync(string key, byte[] value, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            LastSetBuffer = value;
            _storedBytes = value.ToArray();
            return Task.CompletedTask;
        }

        public Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(_storedString);
        }

        public Task SetStringAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _storedString = value;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _storedBytes = null;
            _storedString = null;
            return Task.CompletedTask;
        }
    }
}
