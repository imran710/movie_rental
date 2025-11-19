using Core.Domain.Logger;

using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Database.Seeds;

public class UserSeed
{
    private const string SeedName = "User";

    public static async Task ExecuteAsync(AppDbContext appDbContext, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        StaticLogger.Logger.LogInformation($"Started `{SeedName}` seeding");
        StaticLogger.Logger.LogInformation($"Successfully completed `{SeedName}` seeding");
    }
}
