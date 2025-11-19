using Core.Domain.Common;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.PermissionManagement.UseCase.CheckUserPermissions;

public class CheckUserPermissionsHandler(AppDbContext appDbContext) : BaseHandler<CheckUserPermissionsRequest, CheckUserPermissionsResponse>
{
    protected override async Task<Result<CheckUserPermissionsResponse>> Handle(CheckUserPermissionsRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = request.UserId ?? _httpContext.GetCurrentUser().UserId;

        var permissions = await appDbContext.RolePermissions
            .Where(up => up.RoleId == currentUser)
            .Include(up => up.Permission)
            .Select(up => up.Permission!)
            .ToListAsync(cancellationToken);

        return new CheckUserPermissionsResponse(permissions.ToDictionary(p => p.Name, p => request.Permissions.Contains(p.Name)));
    }
}

