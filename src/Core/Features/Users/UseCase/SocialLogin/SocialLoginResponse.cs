using Core.Features.Users.Common;

namespace Core.Features.Users.UseCase.SocialLogin;
public record SocialLoginResponse(UserModel User,
    TokenInfoModel TokenInfo);
