namespace Core.Features.PermissionManagement.RolePermissions;

public static class RolePermissionQuery
{
    public static IQueryable<RolePermission> GetRolePermissions(this IQueryable<RolePermission> queryable, long roleId)
    {
        return queryable.Where(rp => rp.RoleId == roleId);
    }
}
