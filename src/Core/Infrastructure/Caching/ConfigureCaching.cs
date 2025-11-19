using Core.Infrastructure.ServiceInjector;

using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ZiggyCreatures.Caching.Fusion;

namespace Core.Infrastructure.Caching;

public class ConfigureCaching : IInjectServicesWithConfiguration
{
    public void Configure(IServiceCollection services, IConfiguration configuration)
    {
        var option = configuration
            .GetSection(CachingOption.SectionName)
            .Get<CachingOption>() ?? throw new NullReferenceException($"Failed to load section {CachingOption.SectionName} from appsettings");

        var fusionCacheBuilder = services
            .AddFusionCache()
            .WithDefaultEntryOptions(DefaultCache.OneSecondCache)
            .WithCysharpMemoryPackSerializer();

        if (option.Type == CachingOption.CachingType.Redis)
        {
            var redisCache = new RedisCacheOptions
            {
                Configuration = $"{option.Redis.Host}:{option.Redis.Port}",
                InstanceName = option.Redis.InstanceName,
                ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                {
                    User = option.Redis.Username,
                    Password = option.Redis.Password,
                },
            };
            fusionCacheBuilder
                .WithDistributedCache(new RedisCache(redisCache));
        }
    }
}
