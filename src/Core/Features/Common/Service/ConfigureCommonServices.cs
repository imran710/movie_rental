using Core.Infrastructure.ServiceInjector;

using Microsoft.Extensions.DependencyInjection;

namespace Core.Features.Common.Service;

public class ConfigureCommonServices : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddScoped<PermissionManagerService>();
    }
}

