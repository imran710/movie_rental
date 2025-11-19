namespace Core.Features.PermissionManagement.UserRoles;

public static class UserRoleQuery
{
    public static IQueryable<UserRole> GetUserRolesByUserAndRole(this IQueryable<UserRole> queryable, long userId, long roleId)
    {
        return queryable.Where(ur => ur.UserId == userId && ur.RoleId == roleId);
    }

    public static IQueryable<UserRole> GetUserRolesByUser(this IQueryable<UserRole> queryable, long userId)
    {
        return queryable.Where(ur => ur.UserId == userId);
    }

    public static IQueryable<UserRole> GetUserRolesByRole(this IQueryable<UserRole> queryable, long roleId)
    {
        return queryable.Where(ur => ur.RoleId == roleId);
    }
}
