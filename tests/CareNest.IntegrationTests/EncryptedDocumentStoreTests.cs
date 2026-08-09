using System.Text;
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
}
