using Core.Domain.Logger;
using Core.Features.PermissionManagement.Common.System;
using Core.Features.PermissionManagement.Permissions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Database.Seeds;

public class PermissionSeed
{
    private const string SeedName = "Permission";

    public static async Task ExecuteAsync(AppDbContext appDbContext, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        StaticLogger.Logger.LogInformation("Started {SeedName} seeding", SeedName);

        var systemPermissions = await appDbContext.Permissions
            .GetSystemPermissions([.. SystemPermissions.AllPermissions])
            .ToListAsync(cancellationToken);
        if (systemPermissions.Count != 0)
        {
            StaticLogger.Logger.LogInformation("All system permissions already exists");
        }
        else
        {
            await appDbContext.Permissions.AddRangeAsync(SystemPermissions.AllPermissions, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);
            StaticLogger.Logger.LogInformation("System permissions created");
        }
    }
}
