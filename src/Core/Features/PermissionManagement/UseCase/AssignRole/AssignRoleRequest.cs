namespace Core.Features.PermissionManagement.UseCase.AssignRole;

public class AssignRoleRequest
{
    public required long UserId { get; init; }
    public required long RoleId { get; init; }
}

