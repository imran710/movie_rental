namespace Core.Features.PermissionManagement.UseCase.AssignPermissions;

public class AssignPermissionsResponse
{
    public required List<long> PermissionIds { get; init; }
}
