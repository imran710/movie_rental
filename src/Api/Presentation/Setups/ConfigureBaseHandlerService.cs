using Core;
using Core.Domain.Common;

namespace Api.Presentation.Setups;

public static class ConfigureBaseHandlerService
{
    public static IServiceCollection AddBaseHandlerConfiguration(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(ICoreAssemblyMarker))
            .AddClasses(classes => classes.AssignableTo(typeof(BaseHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
