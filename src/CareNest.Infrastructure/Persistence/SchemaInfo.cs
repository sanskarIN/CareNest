namespace CareNest.Infrastructure.Persistence;

internal sealed class SchemaInfo
{
    public int Version { get; set; }
    public DateTime AppliedUtc { get; set; }
}
