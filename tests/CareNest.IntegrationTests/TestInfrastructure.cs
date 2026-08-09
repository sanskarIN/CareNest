using System.Collections.Concurrent;
using CareNest.Application.Contracts;
using CareNest.Infrastructure.Backup;
using CareNest.Infrastructure.Configuration;
using CareNest.Infrastructure.Documents;
using CareNest.Infrastructure.Persistence;
using Microsoft.Extensions.Logging.Abstractions;

namespace CareNest.IntegrationTests;

internal sealed class MemorySecretStore : ISecretStore
{
    private readonly ConcurrentDictionary<string, byte[]> _bytes = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, string> _strings = new(StringComparer.Ordinal);

    public Task<byte[]?> GetBytesAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_bytes.TryGetValue(key, out var value) ? value.ToArray() : null);
    }

    public Task SetBytesAsync(string key, byte[] value, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _bytes[key] = value.ToArray();
        return Task.CompletedTask;
    }

    public Task<string?> GetStringAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_strings.TryGetValue(key, out var value) ? value : null);
    }

    public Task SetStringAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _strings[key] = value;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _bytes.TryRemove(key, out _);
        _strings.TryRemove(key, out _);
        return Task.CompletedTask;
    }
}

internal sealed class TestStore : IAsyncDisposable
{
    private static int _sqliteInitialized;

    private TestStore(
        string root,
        CareNestStorageOptions options,
        MemorySecretStore secrets,
        SqliteDatabase database,
        CareNestRepository repository,
        EncryptedDocumentStore documents,
        EncryptedBackupService backups)
    {
        Root = root;
        Options = options;
        Secrets = secrets;
        Database = database;
        Repository = repository;
        Documents = documents;
        Backups = backups;
    }

    public string Root { get; }
    public CareNestStorageOptions Options { get; }
    public MemorySecretStore Secrets { get; }
    public SqliteDatabase Database { get; }
    public CareNestRepository Repository { get; }
    public EncryptedDocumentStore Documents { get; }
    public EncryptedBackupService Backups { get; }

    public static async Task<TestStore> CreateAsync(MemorySecretStore? secrets = null)
    {
        if (Interlocked.Exchange(ref _sqliteInitialized, 1) == 0)
        {
            SQLitePCL.Batteries_V2.Init();
        }

        var root = Path.Combine(Path.GetTempPath(), "CareNestTests", Guid.NewGuid().ToString("N"));
        var options = new CareNestStorageOptions(
            Path.Combine(root, "Data", "carenest.db"),
            Path.Combine(root, "Documents"),
            Path.Combine(root, "Work"));
        options.EnsureDirectories();

        secrets ??= new MemorySecretStore();
        var database = new SqliteDatabase(options, NullLogger<SqliteDatabase>.Instance);
        var repository = new CareNestRepository(database);
        await repository.InitializeAsync();
        var documents = new EncryptedDocumentStore(options, secrets);
        var backups = new EncryptedBackupService(database, repository, options, secrets, TimeProvider.System);
        return new TestStore(root, options, secrets, database, repository, documents, backups);
    }

    public async ValueTask DisposeAsync()
    {
        await Database.DisposeAsync();
        try
        {
            if (Directory.Exists(Root))
            {
                Directory.Delete(Root, recursive: true);
            }
        }
        catch
        {
            // Test cleanup only.
        }
    }
}
