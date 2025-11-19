using Core.Domain.Common;
using Core.Features.PermissionManagement.UserRoles;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.PermissionManagement.UseCase.AssignRole;

public class AssignRoleHandler(AppDbContext dbContext) : BaseHandler<AssignRoleRequest, AssignRoleResponse>
{
    protected override async Task<Result<AssignRoleResponse>> Handle(AssignRoleRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var user = await dbContext.Users.FindAsync([request.UserId], cancellationToken: cancellationToken);
        if (user is null)
        {
            return Error.NotFound("User not found");
        }

        var role = await dbContext.Roles.FindAsync([request.RoleId], cancellationToken: cancellationToken);
        if (role is null)
        {
            return Error.NotFound("Role not found");
        }

        var userRoles = await dbContext.UserRoles.GetUserRolesByUserAndRole(request.UserId, request.RoleId).ToListAsync(cancellationToken);
        if (userRoles.Count > 0)
            userRoles.ForEach(ur => ur.DeletionInfo.MarkAsDeleted(currentUser.UserId));

        await dbContext.UserRoles.AddAsync(UserRole.Create(request.UserId, request.RoleId), cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AssignRoleResponse();
    }
}
