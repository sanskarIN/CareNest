namespace CareNest.Infrastructure.Configuration;

public sealed record CareNestStorageOptions(
    string DatabasePath,
    string DocumentDirectory,
    string WorkingDirectory)
{
    public void EnsureDirectories()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath) ?? throw new InvalidOperationException("Database directory is invalid."));
        Directory.CreateDirectory(DocumentDirectory);
        Directory.CreateDirectory(WorkingDirectory);
    }
}
