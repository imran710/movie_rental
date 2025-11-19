namespace Core.Features.Users.UseCase.GetUserByRole;

public class GetUsersByRoleRequest(long? roleId)
{
    public long? RoleId { get; init; } = roleId;
}
