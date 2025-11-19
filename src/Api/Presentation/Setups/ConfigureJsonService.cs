using System.Text.Json.Serialization;

namespace Api.Presentation.Setups;

public static class ConfigureJsonService
{
    public static IServiceCollection AddJsonConfiguration(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = null;
            options.SerializerOptions.MaxDepth = 1000;
            options.SerializerOptions.DictionaryKeyPolicy = null;
            options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.SerializerOptions.PropertyNameCaseInsensitive = false;
        });

        return services;
    }
}
