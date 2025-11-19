namespace Core.Features.PermissionManagement.Roles;

public static class RoleQuery
{
    public static IQueryable<Role> GetSystemRoles(this IQueryable<Role> query, List<Role> roles)
    {
        var roleNames = roles.Select(r => r.Name);
        return query.Where(r => roleNames.Contains(r.Name) && r.IsSystemManaged);
    }

    public static IQueryable<Role> GetRoleByName(this IQueryable<Role> query, string roleName)
    {
        return query.Where(x => x.Name == roleName);
    }

    public static IQueryable<Role> GetRolesByType(this IQueryable<Role> query, RoleType roleType)
    {
        return query.Where(x => x.Type == roleType);
    }

    public static IQueryable<Role> GetRoleById(this IQueryable<Role> query, long id)
    {
        return query.Where(x => x.Id == id);
    }
}
