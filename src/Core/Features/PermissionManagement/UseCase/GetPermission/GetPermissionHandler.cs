using Core.Domain.Common;
using Core.Features.PermissionManagement.Common.Mapper;
using Core.Infrastructure.Database;

namespace Core.Features.PermissionManagement.UseCase.GetPermission;

public class GetPermissionHandler(AppDbContext appDbContext) : BaseHandler<GetPermissionRequest, GetPermissionResponse>
{
    protected override async Task<Result<GetPermissionResponse>> Handle(GetPermissionRequest request, CancellationToken cancellationToken = default)
    {
        var permission = await appDbContext.Permissions.FindAsync([request.PermissionId, cancellationToken], cancellationToken: cancellationToken);
        if (permission == null)
        {
            return Error.NotFound("Permission not found");
        }

        return new GetPermissionResponse(permission.ToModel());
    }
}