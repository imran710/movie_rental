namespace Core.Features.PermissionManagement.Permissions;

public static class PermissionQuery
{
    public static IQueryable<Permission> GetPermissions(this IQueryable<Permission> queryable, List<long> permissionIds)
    {
        return queryable.Where(p => permissionIds.Contains(p.Id));
    }

    public static IQueryable<Permission> GetSystemPermissions(this IQueryable<Permission> queryable, List<Permission> permissions)
    {
        var permissionCodes = permissions.Select(p => p.Code);
        return queryable.Where(p => permissionCodes.Contains(p.Code) && p.IsSystemManaged);
    }
}
