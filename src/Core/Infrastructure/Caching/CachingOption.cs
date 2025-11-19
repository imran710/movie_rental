namespace Core.Infrastructure.Caching;

public class CachingOption
{
    public const string SectionName = "Caching";

    public required CachingType Type { get; init; }
    public required RedisOption Redis { get; set; }

    public enum CachingType
    {
        InMemory,
        Redis,
    }

    public class RedisOption
    {
        public required string InstanceName { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Host { get; set; }
        public required string Port { get; set; }
    }
}
