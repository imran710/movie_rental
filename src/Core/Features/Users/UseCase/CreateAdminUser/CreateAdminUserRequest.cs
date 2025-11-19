using Core.Features.Users.ValueObject.PersonalName;

namespace Core.Features.Users.UseCase.CreateAdminUser;

public record CreateAdminUserRequest(
    UserPersonalNameModel PersonalName,
    string Email,
    string Password,
    long RoleId);

