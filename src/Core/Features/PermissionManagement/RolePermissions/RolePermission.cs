using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Features.PermissionManagement.Permissions;
using Core.Features.PermissionManagement.Roles;

namespace Core.Features.PermissionManagement.RolePermissions;

public class RolePermission : IEntity, IDeletableEntity
{
    public long Id { get; set; }

    public Role? Role { get; private set; }
    public long RoleId { get; init; }

    public Permission? Permission { get; private set; }
    public long PermissionId { get; init; }

    public DeletionInfo DeletionInfo { get; private set; } = DeletionInfo.NoDeleted();

    public static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission
        {
            Role = role,
            RoleId = role.Id,
            Permission = permission,
            PermissionId = permission.Id
        };
    }

    public static RolePermission Create(long roleId, long permissionId)
    {
        return new RolePermission
        {
            RoleId = roleId,
            PermissionId = permissionId
        };
    }
}
