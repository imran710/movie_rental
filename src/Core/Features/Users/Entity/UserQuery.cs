using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.Username;

namespace Core.Features.Users.Entity;

public static class UserQuery
{
    public static IQueryable<User> FindByEmail(this IQueryable<User> query, UserEmail email)
    {
        query = query.Where(x => x.Email.EmailNormalized == email.EmailNormalized);

        return query;
    }
    public static IQueryable<User> FindByEmailID(this IQueryable<User> query, string email)
    {
        query = query.Where(x => x.Email.EmailNormalized == email.ToUpper());

        return query;
    }
    public static IQueryable<User> FindById(this IQueryable<User> query, long userId)
    {
        query = query.Where(x => x.Id == userId);

        return query;
    }

    public static IQueryable<User> FindByUsername(this IQueryable<User> query, UserUsername userUsername)
    {
        query = query.Where(x => x.Username.UsernameNormalized == userUsername.UsernameNormalized);

        return query;
    }
    public static IQueryable<User> FindByRole(this IQueryable<User> query, long roleId)
    {
        return query
            .Where(user => user.DeletionInfo.IsDeleted == false)
            .Where(user => user.UserRoles.Any(userRole =>
                userRole.RoleId == roleId &&
                userRole.DeletionInfo.IsDeleted == false));
    }
}
