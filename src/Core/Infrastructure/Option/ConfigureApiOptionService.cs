using Core.Infrastructure.ServiceInjector;

using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Option;

public class ConfigureOptionService : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddOptions<ApiOption>().BindConfiguration(ApiOption.SectionName);
        services.AddOptions<ApiOption.OpenApiOption>().BindConfiguration(ApiOption.OpenApiOption.SectionName);
        services.AddOptions<ApiOption.SpoonacularOption>().BindConfiguration(ApiOption.SpoonacularOption.SectionName);
        services.AddOptions<ApiOption.EdamamOption>().BindConfiguration(ApiOption.EdamamOption.SectionName);
        services.AddOptions<ApiOption.OpenAiOption>().BindConfiguration(ApiOption.OpenAiOption.SectionName);

        services.AddOptions<PaymentOption>().BindConfiguration(PaymentOption.SectionName);
        services.AddOptions<PaymentOption.StripeOption>().BindConfiguration(PaymentOption.StripeOption.SectionName);
    }
}
