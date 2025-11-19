using Core.Domain.Helper;
using Core.Infrastructure.ServiceInjector;

namespace Api.Presentation.Helper;

public class ConfigureHelperService : IInjectServicesWithConfiguration
{
    public void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtTokenHelper, JwtTokenHelper>();
    }
}
