using System.Security.Claims;

using Core.Features.PermissionManagement.Roles;
using Core.Features.Users.ValueObject.Username;

namespace Core.Features.Auth.Helper;

public static class UserClaimGenerator
{
    public const string RoleSeparator = "~";

    public static class CustomClaimTypes
    {
        public const string UserId = "UserId";
        public const string UserName = "UserName";
        public const string Role = "Role";
        public const string SubscriptionPlan = "SubscriptionPlan";
    }

    public static IEnumerable<Claim> GetUserClaims(long userId, UserUsername userName, List<Role>? roles = null)
    {
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, value: userId.ToString(provider: null)),
            new(CustomClaimTypes.UserName, value: userName.ToString()),
            new(CustomClaimTypes.Role, value: RoleToString(roles ?? [])),
        };

        return claims;
    }

    public static string RoleToString(List<Role> role)
    {
        return string.Join(RoleSeparator, role.Select(x => x.Name));
    }

    public static IEnumerable<string> StringToRoleNames(string roleString)
    {
        return roleString.Split(RoleSeparator);
    }
}
