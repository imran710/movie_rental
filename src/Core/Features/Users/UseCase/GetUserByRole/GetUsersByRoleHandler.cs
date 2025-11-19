using Core.Domain.Common;
using Core.Features.PermissionManagement.Roles;
using Core.Features.Users.Entity;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Users.UseCase.GetUserByRole;

public class GetUsersByRoleHandler(AppDbContext appDbContext) : BaseHandler<GetUsersByRoleRequest, GetUsersByRoleResponse>
{
    protected override async Task<Result<GetUsersByRoleResponse>> Handle(GetUsersByRoleRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var users = await appDbContext.UserRoles
            .WhereIf(request.RoleId is not null, x => x.RoleId == request.RoleId)
            .Where(x => x.Role!.Type == RoleType.Admin)
            .Include(x => x.User)
            .Select(x => x.User!.MapToUserModel())
            .ToListAsync(cancellationToken: cancellationToken);

        return new GetUsersByRoleResponse(users);
    }
}
