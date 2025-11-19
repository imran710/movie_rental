using Core.Features.PermissionManagement.Roles;

namespace Core.Features.PermissionManagement.Common.Model;

public class RoleModel : RoleShortModel
{
    public required bool IsSystemManaged { get; init; }
    public required string? Description { get; init; }
}

public class RoleShortModel
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required RoleType Type { get; init; }
}
