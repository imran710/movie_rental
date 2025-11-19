using Core.Features.Users.ValueObject.PersonalName;

namespace Core.Features.Users.UseCase.SocialLogin;
public record SocialLoginRequest(
    UserPersonalNameModel PersonalName,
    string Email,string token, string FcmToken);
