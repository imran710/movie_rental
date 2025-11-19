using System.Globalization;

using Core.Domain.Common;

using Microsoft.AspNetCore.Http;

using static Core.Features.Auth.Helper.UserClaimGenerator;

namespace Core.Infrastructure.Extensions;

public static partial class UserInfoExtensions
{
    public static CurrentUserInfo GetCurrentUser(this HttpContext context)
    {
        var currentUserId = context.User.FindFirst(CustomClaimTypes.UserId)?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId))
            throw new Exception("Can not get current user info");

        if (!long.TryParse(currentUserId, NumberStyles.None, provider: null, out var userId) || userId == default)
            throw new Exception("Can not get current user info");

        var userNameString = context.User.FindFirst(CustomClaimTypes.UserName)?.Value;
        if (string.IsNullOrWhiteSpace(userNameString))
            throw new Exception("Can not get current user info");

        return new CurrentUserInfo
        {
            UserId = userId,
            Username = userNameString ?? string.Empty,
        };
    }
}
