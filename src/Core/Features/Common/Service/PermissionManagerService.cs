using Core.Features.PermissionManagement.Permissions;
using Core.Features.PermissionManagement.Roles;
using Core.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Common.Service;

public class PermissionManagerService(AppDbContext dbContext)
{
    public async Task HasPermissionAsync(long userId, RoleType roleType, params Permission[] permissions)
    {
        var userRole = await dbContext.UserRoles
            .Where(x => x.UserId == userId && x.Role!.Type == roleType)
            .Select(x => x.RoleId)
            .FirstOrDefaultAsync();
        if (userRole == default)
        {
            throw new PermissionException("User does not have expected role");
        }

        var rolePermissions = await dbContext.RolePermissions
            .Where(x => x.RoleId == userRole)
            .Select(x => x.Permission!.Code)
            .ToListAsync();
        if (!rolePermissions.Any(x => permissions.Any(y => y.Code == x)))
        {
            throw new PermissionException($"User does not have expected permissions");
        }
    }
}

public class PermissionException(string message) : Exception(message)
{
}

