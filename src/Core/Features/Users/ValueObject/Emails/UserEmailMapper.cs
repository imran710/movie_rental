namespace Core.Features.Users.ValueObject.Emails;

public static class UserEmailMapper
{
    public static UserEmailModel MapToUserEmailModel(this UserEmail userEmail)
    {
        return new UserEmailModel
        {
            Email = userEmail.Email,
            EmailConfirmed = userEmail.EmailConfirmed
        };
    }
}
