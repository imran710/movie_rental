using Core.Infrastructure.Option;
using Core.Infrastructure.ServiceInjector;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.HttpClient;

public class ConfigureHttpClientService : IInjectServicesWithConfiguration
{
    public void Configure(IServiceCollection services, IConfiguration configuration)
    { 
        var apiOption = configuration.GetSection(ApiOption.SectionName).Get<ApiOption>() ?? throw new NullReferenceException(); 

    }
}
