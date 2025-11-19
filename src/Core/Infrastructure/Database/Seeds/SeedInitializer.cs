using Core.Domain.Logger;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Infrastructure.Database.Seeds;

public class SeedInitializer(AppDbContext dbContext, IOptions<DatabaseOption> databaseOption)
{
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!databaseOption.Value.Seed)
            {
                StaticLogger.Logger.LogInformation("Database seeding is disabled");
                return;
            }

            StaticLogger.Logger.LogInformation("Started database seeding");
            await UserSeed.ExecuteAsync(dbContext, cancellationToken);
            await RoleSeed.ExecuteAsync(dbContext, cancellationToken);
            await PermissionSeed.ExecuteAsync(dbContext, cancellationToken);
            StaticLogger.Logger.LogInformation("Successfully completed database seeding");
        }
        catch (Exception ex)
        {
            StaticLogger.Logger.LogCritical("Failed to seed database. {SeedException}", ex);
        }
    }
}
