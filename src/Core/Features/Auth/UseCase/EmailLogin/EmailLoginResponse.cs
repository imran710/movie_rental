using Core.Features.PermissionManagement.Common.Model;
using Core.Features.Users.Common;

namespace Core.Features.Auth.UseCase.EmailLogin;

public record EmailLoginResponse(
    UserModel User,
    TokenInfoModel TokenInfo,
    List<RoleShortModel>? Roles);
