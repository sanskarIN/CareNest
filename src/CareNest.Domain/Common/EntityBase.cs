namespace CareNest.Domain.Common;

public abstract class EntityBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

    public void Touch(DateTime utcNow) => UpdatedUtc = DateTime.SpecifyKind(utcNow, DateTimeKind.Utc);
}
