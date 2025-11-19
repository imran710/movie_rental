using Core.Domain.Common;
using Core.Features.PermissionManagement.Permissions;
using Core.Infrastructure.Database;

namespace Core.Features.PermissionManagement.UseCase.EditPermission;

public class EditPermissionHandler(AppDbContext appDbContext) : BaseHandler<EditPermissionRequest, EditPermissionResponse>
{
    protected override async Task<Result<EditPermissionResponse>> Handle(EditPermissionRequest request, CancellationToken cancellationToken = default)
    {
        var permission = await appDbContext.Permissions.FindAsync([request.Id], cancellationToken: cancellationToken);
        if (permission is null)
        {
            return Error.NotFound($"Permission with id {request.Id} not found");
        }

        var result = Permission.CreatePermission(Value.New(request.Name), Value.New(request.Description));
        if (result.IsError)
        {
            return result.Error;
        }

        permission.Edit(result.Value);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return new EditPermissionResponse();
    }
}
