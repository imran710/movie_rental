namespace Core.Features.PermissionManagement.UseCase.EditRole;

public class EditRoleRequest
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
}
