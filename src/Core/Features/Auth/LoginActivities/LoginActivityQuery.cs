using Core.Features.Users.Common;
using Core.Features.Users.Entity;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Auth.LoginActivities;

public static class LoginActivityQuery
{
    public static IQueryable<LoginActivity> ActiveUsers(this IQueryable<LoginActivity> query, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        return query
            .Where(la => la.LoginTime >= startDate && la.LoginTime <= endDate && (la.LogoutTime == null || la.LogoutTime > startDate))
            .GroupBy(la => la.UserId)
            .Select(g => g.OrderByDescending(la => la.LoginTime).FirstOrDefault()!)
            .Where(la => la != null);
    }

    public static IQueryable<UserModel> GetActiveUsers(this IQueryable<LoginActivity> query, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        return query
            .Where(la => la.LoginTime >= startDate && la.LoginTime <= endDate && (la.LogoutTime == null || la.LogoutTime > startDate))
            .Include(x => x.User)
            .GroupBy(la => la.UserId)
            .Select(g => g.OrderByDescending(la => la.LoginTime).Select(y => y.User!.MapToUserModel()).FirstOrDefault()!);
    }
}
