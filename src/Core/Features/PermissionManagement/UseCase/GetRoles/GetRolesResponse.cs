using Core.Features.PermissionManagement.Common.Model;

namespace Core.Features.PermissionManagement.UseCase.GetRoles;

public class GetRolesResponse(List<RoleModel> roles) : List<RoleModel>(roles)
{
}
