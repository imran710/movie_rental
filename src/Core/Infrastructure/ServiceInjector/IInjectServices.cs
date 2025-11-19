using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.ServiceInjector;

public interface IInjectServices
{
    void Configure(IServiceCollection services);
}
