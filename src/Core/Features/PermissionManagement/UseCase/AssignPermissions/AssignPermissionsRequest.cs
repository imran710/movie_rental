namespace Core.Features.PermissionManagement.UseCase.AssignPermissions;

public class AssignPermissionsRequest
{
    public required long RoleId { get; init; }
    public required List<long> PermissionIds { get; init; }
}
