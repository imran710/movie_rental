namespace Core.Features.PermissionManagement.UseCase.EditPermission;

public class EditPermissionRequest
{
    public long Id { get; set; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}
