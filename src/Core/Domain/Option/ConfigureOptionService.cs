using Core.Infrastructure.ServiceInjector;

using Microsoft.Extensions.DependencyInjection;

namespace Core.Domain.Option;

public class ConfigureOptionService : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddOptions<DomainOption>().BindConfiguration(DomainOption.SectionName);
        services.AddOptions<DomainOption.SecurityOption>().BindConfiguration(DomainOption.SecurityOption.SectionName);
        services.AddOptions<DomainOption.OtpVerificationOption>().BindConfiguration(DomainOption.OtpVerificationOption.SectionName);
        services.AddOptions<DomainOption.AuthOption>().BindConfiguration(DomainOption.AuthOption.SectionName);
        services.AddOptions<ApiCallKeys>().BindConfiguration(ApiCallKeys.SectionName);
    }
}
