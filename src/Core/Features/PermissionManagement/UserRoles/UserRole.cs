using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Features.PermissionManagement.Roles;
using Core.Features.Users.Entity;

namespace Core.Features.PermissionManagement.UserRoles;

public class UserRole : IEntity, IDeletableEntity
{
    public long Id { get; init; }

    public long UserId { get; init; }
    public User? User { get; init; }

    public long RoleId { get; init; }
    public Role? Role { get; init; }

    public DeletionInfo DeletionInfo { get; private set; } = DeletionInfo.NoDeleted();

    public static UserRole Create(User user, Role role)
    {
        return new UserRole
        {
            UserId = user.Id,
            User = user,
            RoleId = role.Id,
            Role = role,
        };
    }

    public static UserRole Create(long userId, long roleId)
    {
        return new UserRole
        {
            UserId = userId,
            RoleId = roleId,
        };
    }
}
