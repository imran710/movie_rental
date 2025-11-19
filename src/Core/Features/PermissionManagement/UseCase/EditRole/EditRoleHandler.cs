using Core.Domain.Common;
using Core.Features.PermissionManagement.Roles;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

namespace Core.Features.PermissionManagement.UseCase.EditRole;

public class EditRoleHandler(AppDbContext dbContext) : BaseHandler<EditRoleRequest, EditRoleResponse>
{
    protected override async Task<Result<EditRoleResponse>> Handle(EditRoleRequest request, CancellationToken cancellationToken = default)
    {
        var user = _httpContext.GetCurrentUser();

        var role = await dbContext.Roles.FindAsync([request.Id], cancellationToken: cancellationToken).ConfigureAwait(false);
        if (role is null)
        {
            return Error.NotFound("Role not found");
        }

        // Check if the role is system managed
        if (role.IsSystemManaged)
        {
            return Error.Forbidden("Cannot edit system managed role");
        }

        var newRole = Role.Create(Value.New(request.Name), Value.New(request.Description ?? string.Empty));
        if (newRole.IsError)
        {
            return newRole.Error;
        }

        role.Edit(newRole.Value, user.UserId);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new EditRoleResponse();
    }
}
