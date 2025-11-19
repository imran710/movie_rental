using Core.Domain.Common;
using Core.Features.PermissionManagement.Common.Mapper;
using Core.Infrastructure.Database;

namespace Core.Features.PermissionManagement.UseCase.GetRole;

public class GetRoleHandler(AppDbContext appDbContext) : BaseHandler<GetRoleRequest, GetRoleResponse>
{
    protected override async Task<Result<GetRoleResponse>> Handle(GetRoleRequest request, CancellationToken cancellationToken = default)
    {
        var role = await appDbContext.Roles.FindAsync([request.RoleId, cancellationToken], cancellationToken: cancellationToken);
        if (role == null)
        {
            return Error.NotFound("Role not found");
        }

        return new GetRoleResponse
        {
            Role = role.MapToModel()
        };
    }
}
