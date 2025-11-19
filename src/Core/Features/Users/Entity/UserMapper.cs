using Core.Features.Users.Common;
using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.PersonalName;

namespace Core.Features.Users.Entity;

public static class UserMapper
{
    public static UserModel MapToUserModel(this User user)
    {
        return new UserModel
        {
            Id = user.Id,
            PersonalName = user.PersonalName.MapToUserPersonalNameModel(),
            Username = user.Username.ToString(),
            Email = user.Email.MapToUserEmailModel(),
            UserProfileImageUrl = user.UserProfileImageUrl,
            RegistrationDate = user.CreatedAt,
        };
    }
}
