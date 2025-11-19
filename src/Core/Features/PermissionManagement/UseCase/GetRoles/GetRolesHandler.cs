using Core.Domain.Common;
using Core.Features.PermissionManagement.Common.Mapper;
using Core.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.PermissionManagement.UseCase.GetRoles;

public class GetRolesHandler(AppDbContext dbContext) : BaseHandler<GetRolesRequest, GetRolesResponse>
{
    protected override async Task<Result<GetRolesResponse>> Handle(GetRolesRequest request, CancellationToken cancellationToken = default)
    {
        var roles = await dbContext.Roles
            .OrderBy(r => r.Name)
            .WhereIf(request.RoleType is not null, r => r.Type == request.RoleType)
            .Select(r => r.MapToModel())
            .ToListAsync(cancellationToken);

        return new GetRolesResponse(roles);
    }
}
