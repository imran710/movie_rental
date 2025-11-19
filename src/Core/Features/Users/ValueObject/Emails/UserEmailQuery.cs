using System.Linq.Expressions;

namespace Core.Features.Users.ValueObject.Emails;

public static class UserEmailQuery
{
    public static Expression<Func<UserEmail, bool>> MatchByEmail(UserEmail userEmail)
    {
        return email => email.EmailNormalized == userEmail.EmailNormalized;
    }
}
