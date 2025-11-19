namespace Core.Features.PermissionManagement.UseCase.CheckUserPermissions;

public class CheckUserPermissionsResponse(Dictionary<string, bool> values) : Dictionary<string, bool>(values)
{
}

