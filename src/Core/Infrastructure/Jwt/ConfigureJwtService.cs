using Core.Infrastructure.ServiceInjector;

using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Jwt;

public class ConfigureJwtService : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddOptions<JwtOption>().BindConfiguration(JwtOption.SectionName).ValidateOnStart();
    }
}
