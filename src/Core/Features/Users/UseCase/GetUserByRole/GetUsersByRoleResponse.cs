using Core.Features.Users.Common;

namespace Core.Features.Users.UseCase.GetUserByRole;

public record GetUsersByRoleResponse(IList<UserModel> Users);
