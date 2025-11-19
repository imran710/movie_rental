using Core.Features.PermissionManagement.Roles;

namespace Core.Features.PermissionManagement.UseCase.GetRoles;

public class GetRolesRequest
{
    public RoleType? RoleType { get; init; }
}
