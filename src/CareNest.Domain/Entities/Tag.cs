using CareNest.Domain.Common;

namespace CareNest.Domain.Entities;

public sealed class Tag : EntityBase
{
    public string Name { get; set; } = string.Empty;
}
