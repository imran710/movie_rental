using Core.Infrastructure.Common.Enums;
using Core.Infrastructure.Database.Interceptors;
using Core.Infrastructure.Database.Seeds;
using Core.Infrastructure.ServiceInjector;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Database;

public class ConfigureDatabaseService : IInjectServicesWithConfiguration
{
    public void Configure(IServiceCollection services, IConfiguration configuration)
    {
        // Bind configuration
        services.AddOptions<DatabaseOption>().BindConfiguration(DatabaseOption.SectionName);

        // Get configuration
        var option = configuration
            .GetSection(DatabaseOption.SectionName)
            .Get<DatabaseOption>() ?? throw new NullReferenceException($"Failed to load section {DatabaseOption.SectionName} from appsettings");

        // Check database type
        if (option.DatabaseType != DatabaseType.PostgreSql)
            throw new NotSupportedException("Only PostgreSql database is supported for Entity Framework");

        // Add interceptor
        services.AddSingleton<AuditableEntityInterceptor>();

        // Add DbContext
        services.AddDbContextPool<AppDbContext>((provider, options) =>
        {
            var auditableEntityInterceptor = provider.GetRequiredService<AuditableEntityInterceptor>();

            options.UseNpgsql(option.ConnectionString, npgsql => npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            options.AddInterceptors(auditableEntityInterceptor);
#if DEBUG
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
#endif
        });

        // Add seed initializer
        services.AddScoped<SeedInitializer>();
    }
}
