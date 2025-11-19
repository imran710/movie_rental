using Api.Presentation.Common;
using Api.Presentation.ExceptionHandler;
using Api.Presentation.ServiceInjector;

using Serilog;

using Stripe;

namespace Api.Presentation.Setups;

public static class ConfigurePresentationService
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
    {
        StripeConfiguration.ApiKey = "sk_test_51HKCyMEhH1tRlGGZiZd7gQ7Yxr6gUpfECTf7HtaFzcEVIfTMEY0kbKpjRPetz2W8T92OWi6CmfV3bh00JRsRkXwA00NPot0ie9";

        services
            .AddExceptionHandler<AppExceptionHandler>();

        services
            .AddSerilog((serviceProvider, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(serviceProvider)
                .Enrich.FromLogContext()
            );

        services
            .AddInjectServices()
            .AddInjectServicesWithConfiguration(builder.Configuration)
            .AddBaseHandlerConfiguration()
            .AddEndpointConfiguration()
            .AddJsonConfiguration()
            .AddOpenApiConfiguration()
            .AddAuthConfiguration(builder.Configuration);

        services
            .AddOptions()
            .AddResourceMonitoring()
            .AddHealthChecks();

        return services;
    }
}
