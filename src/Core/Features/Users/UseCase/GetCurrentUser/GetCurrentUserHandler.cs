using Core.Domain.Common;
using Core.Domain.Parser;
using Core.Features.PermissionManagement.Common.Mapper;
using Core.Features.PermissionManagement.Common.Model;
using Core.Features.PermissionManagement.UserRoles;
using Core.Features.Users.Common;
using Core.Features.Users.Entity;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Users.UseCase.GetCurrentUser;

public class GetCurrentUserHandler(AppDbContext appDbContext) : BaseHandler<GetCurrentUserRequest, GetCurrentUserResponse>
{
    protected override async Task<Result<GetCurrentUserResponse>> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();
        var requestEnums = request.Include.ParseIncludeString<IncludeRequestEnum>();

        User? user = null;
        if (requestEnums.Contains(IncludeRequestEnum.Profile))
        {
            user = await appDbContext.Users.FindById(currentUser.UserId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (user is null)
                return Error.NotFound();
        }

        List<RoleModel> roles = new();
        if (requestEnums.Contains(IncludeRequestEnum.Role))
        {
            roles = await appDbContext.UserRoles
                .GetUserRolesByUser(currentUser.UserId)
                .Include(x => x.Role)
                .Select(x => x.Role!)
                .Select(x => x.MapToModel())
                .ToListAsync(cancellationToken: cancellationToken);
        }
         
        return new GetCurrentUserResponse(
            user?.MapToUserModel(), 
            roles);
    }
}
