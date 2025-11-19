using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.PersonalName;

namespace Core.Features.Users.Common;

public class UserModel
{
    public required long Id { get; init; }
    public required UserPersonalNameModel PersonalName { get; init; }
    public required string Username { get; init; }
    public required UserEmailModel Email { get; init; }
    public required string? UserProfileImageUrl { get; init; }
    public DateTimeOffset  RegistrationDate { get; init; }
}
