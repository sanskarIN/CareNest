using CareNest.Application.Contracts;

namespace CareNest.UnitTests.TestDoubles;

internal sealed class DocumentStoreSpy : IDocumentStore
{
    public StoredDocument ImportResult { get; set; } = new("stored.cndoc", 0, "sha256", 1);

    public byte[] ExportPayload { get; set; } = [];

    public List<string> DeletedFiles { get; } = [];

    public List<string> StoredFiles { get; } = [];

    public byte[]? LastImportedBytes { get; private set; }

    public string? LastOriginalFileName { get; private set; }

    public string? LastContentType { get; private set; }

    public Task<long> GetStorageUsageBytesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult((long)StoredFiles.Count);
    }

    public Task<IReadOnlyList<string>> ListStoredFilesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult<IReadOnlyList<string>>(StoredFiles.ToArray());
    }

    public async Task<StoredDocument> ImportAsync(
        Stream source,
        string originalFileName,
        string? contentType,
        CancellationToken cancellationToken = default)
    {
        await using var copy = new MemoryStream();
        await source.CopyToAsync(copy, cancellationToken);
        LastImportedBytes = copy.ToArray();
        LastOriginalFileName = originalFileName;
        LastContentType = contentType;
        return ImportResult;
    }

    public async Task ExportDecryptedAsync(
        string encryptedFileName,
        Stream destination,
        CancellationToken cancellationToken = default)
    {
        await destination.WriteAsync(ExportPayload, cancellationToken);
    }

    public Task DeleteAsync(string encryptedFileName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        DeletedFiles.Add(encryptedFileName);
        return Task.CompletedTask;
    }
}
