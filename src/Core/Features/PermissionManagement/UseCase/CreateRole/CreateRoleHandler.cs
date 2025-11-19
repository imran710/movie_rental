using Core.Domain.Common;
using Core.Features.PermissionManagement.Roles;
using Core.Infrastructure.Database;

namespace Core.Features.PermissionManagement.UseCase.CreateRole;

public class CreateRoleHandler(AppDbContext dbContext) : BaseHandler<CreateRoleRequest, CreateRoleResponse>
{
  protected override async Task<Result<CreateRoleResponse>> Handle(CreateRoleRequest request, CancellationToken cancellationToken = default)
  {
    var role = Role.Create(Value.New(request.Name), Value.New(request.Description));
    if (role.IsError)
      return role.Error;

    await dbContext.Roles.AddAsync(role.Value, cancellationToken);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new CreateRoleResponse { RoleId = role.Value.Id };
  }
}
