using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Domain.Helper;
using Core.Features.Auth.LoginActivities;
using Core.Features.Auth.OtpVerifications;
using Core.Features.Auth.RefreshTokens;
using Core.Features.PermissionManagement.UserRoles;
using Core.Features.Users.ValueObject.ContactInfo;
using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.Password;
using Core.Features.Users.ValueObject.PersonalName;
using Core.Features.Users.ValueObject.Username;

namespace Core.Features.Users.Entity;

public class User : IEntity, IDeletableEntity
{
    public long Id { get; }
    public UserPersonalName PersonalName { get; private set; } = UserPersonalName.DefaultName;
    public required UserUsername Username { get; init; }
    public required UserEmail Email { get; init; }
    public UserPassword Password { get; private set; } = UserPassword.NoPassword;
    public UserContactInfo ContactInfo { get; private set; } = UserContactInfo.NoContactInfo;

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeHelperStatic.Now;
    public DeletionInfo DeletionInfo { get; } = new();

    public string? UserProfileImageUrl { get; set; }

    public List<LoginActivity> LoginActivities { get; private set; } = [];
    public List<RefreshToken> RefreshTokens { get; private set; } = [];
    public List<OtpVerification> Verifications { get; private set; } = [];  

    public List<UserRole> UserRoles { get; } = []; 

    public static Result<User> RegisterWithEmail(UserEmail userEmail, UserUsername userUsername, UserPassword password, UserPersonalName userPersonalName)
    {
        return new User
        {
            Email = userEmail,
            Password = password,
            Username = userUsername,
            PersonalName = userPersonalName,
        };
    }
    public static User EditPassword(User user, UserPassword password)
    {
        user.Password = password!;
        return user;

    }
    public void SetPersonalName(UserPersonalName userPersonalName)
    {
        PersonalName = userPersonalName;
    }
}
