using Core.Domain.Common;
using Core.Features.PermissionManagement.Permissions;
using Core.Features.PermissionManagement.RolePermissions;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.PermissionManagement.UseCase.AssignPermissions;

public class AssignPermissionsHandler(AppDbContext dbContext) : BaseHandler<AssignPermissionsRequest, AssignPermissionsResponse>
{
    protected override async Task<Result<AssignPermissionsResponse>> Handle(AssignPermissionsRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var role = await dbContext.Roles.FindAsync([request.RoleId], cancellationToken: cancellationToken);
        if (role is null)
        {
            return Error.NotFound("Role not found");
        }

        var permissions = await dbContext.Permissions.GetPermissions(request.PermissionIds).ToListAsync(cancellationToken);
        if (permissions.Count != request.PermissionIds.Count)
        {
            return Error.NotFound("Some permissions not found");
        }

        var rolePermissions = await dbContext.RolePermissions.GetRolePermissions(request.RoleId).ToListAsync(cancellationToken);
        rolePermissions.ForEach(rp => rp.DeletionInfo.MarkAsDeleted(currentUser.UserId));

        await dbContext.RolePermissions.AddRangeAsync(permissions.Select(p => RolePermission.Create(role, p)), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AssignPermissionsResponse { PermissionIds = [.. permissions.Select(p => p.Id)] };
    }
}
