namespace Core.Features.PermissionManagement.UseCase.CreateRole;

public class CreateRoleRequest
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}
