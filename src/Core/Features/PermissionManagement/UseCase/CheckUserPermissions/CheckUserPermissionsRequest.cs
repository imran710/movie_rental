namespace Core.Features.PermissionManagement.UseCase.CheckUserPermissions;

public class CheckUserPermissionsRequest
{
    public long? UserId { get; init; }
    public string[] Permissions { get; init; } = [];
}
