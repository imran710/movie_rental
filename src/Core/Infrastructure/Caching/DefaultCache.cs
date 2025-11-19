using ZiggyCreatures.Caching.Fusion;

namespace Core.Infrastructure.Caching;

public static class DefaultCache
{
    public static readonly FusionCacheEntryOptions OneSecondCache = new()
    {
        Duration = TimeSpan.FromSeconds(1),
    };

    public static readonly FusionCacheEntryOptions TwoSecondsCache = new()
    {
        Duration = TimeSpan.FromSeconds(2),
    };

    public static readonly FusionCacheEntryOptions ThreeSecondsCache = new()
    {
        Duration = TimeSpan.FromSeconds(3),
    };

    public static readonly FusionCacheEntryOptions FiveSecondsCache = new()
    {
        Duration = TimeSpan.FromSeconds(5),
    };

    public static readonly FusionCacheEntryOptions TenSecondsCache = new()
    {
        Duration = TimeSpan.FromSeconds(10),
    };

    public static readonly FusionCacheEntryOptions ThirtySecondsCache = new()
    {
        Duration = TimeSpan.FromSeconds(30),
    };

    public static readonly FusionCacheEntryOptions OneMinuteCache = new()
    {
        Duration = TimeSpan.FromMinutes(1),
    };
}
