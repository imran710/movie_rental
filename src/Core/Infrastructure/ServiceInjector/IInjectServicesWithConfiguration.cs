using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.ServiceInjector;

public interface IInjectServicesWithConfiguration
{
    void Configure(IServiceCollection services, IConfiguration configuration);
}
