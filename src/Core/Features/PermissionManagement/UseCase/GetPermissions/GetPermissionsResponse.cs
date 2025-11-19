using Core.Features.PermissionManagement.Common.Model;

namespace Core.Features.PermissionManagement.UseCase.GetPermissions;

public class GetPermissionsResponse(List<PermissionModel> permissions) : List<PermissionModel>(permissions)
{
}
