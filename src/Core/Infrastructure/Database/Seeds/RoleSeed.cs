using Core.Domain.Logger;
using Core.Features.PermissionManagement.Common.System;
using Core.Features.PermissionManagement.Roles;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Database.Seeds;

public class RoleSeed
{
    private const string SeedName = "Role";

    public static async Task ExecuteAsync(AppDbContext appDbContext, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        StaticLogger.Logger.LogInformation("Started {SeedName} seeding", SeedName);

        var systemRoles = await appDbContext.Roles
            .GetSystemRoles([.. SystemRoles.AllRoles])
            .ToListAsync(cancellationToken);
        if (systemRoles.Count != 0)
        {
            StaticLogger.Logger.LogInformation("All system roles already exists");
        }
        else
        {
            await appDbContext.Roles.AddRangeAsync(SystemRoles.AllRoles, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);
            StaticLogger.Logger.LogInformation("System roles created");
        }

        StaticLogger.Logger.LogInformation("Successfully completed {SeedName} seeding", SeedName);
    }
}
