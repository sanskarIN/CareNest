namespace CareNest.Shared;

public static class TimeProviderExtensions
{
    public static DateTime UtcNowDateTime(this TimeProvider provider) => provider.GetUtcNow().UtcDateTime;
}
