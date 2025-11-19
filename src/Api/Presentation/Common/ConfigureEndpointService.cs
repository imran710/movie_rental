namespace Api.Presentation.Common;

public static class ConfigureEndpointService
{
    public static IServiceCollection AddEndpointConfiguration(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblyOf<IApiAssemblyMarker>()
                .AddClasses(classes => classes.AssignableTo<IEndpoint>())
                .AsImplementedInterfaces()
            );

        return services;
    }
}
