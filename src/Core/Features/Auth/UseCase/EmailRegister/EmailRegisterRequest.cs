using Core.Features.Users.ValueObject.PersonalName;

namespace Core.Features.Auth.UseCase.EmailRegister;

public record EmailRegisterRequest(
    UserPersonalNameModel PersonalName,
    string Email,
    string Password);
