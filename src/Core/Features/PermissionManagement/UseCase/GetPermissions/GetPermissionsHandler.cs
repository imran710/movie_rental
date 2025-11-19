using Core.Domain.Common;
using Core.Features.PermissionManagement.Common.Mapper;
using Core.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.PermissionManagement.UseCase.GetPermissions;

public class GetPermissionsHandler(AppDbContext appContext) : BaseHandler<GetPermissionsRequest, GetPermissionsResponse>
{
    protected override async Task<Result<GetPermissionsResponse>> Handle(GetPermissionsRequest request, CancellationToken cancellationToken = default)
    {
        var permissions = await appContext.Permissions
            .Select(p => p.ToModel())
            .ToListAsync(cancellationToken);

        return new GetPermissionsResponse(permissions);
    }
}
