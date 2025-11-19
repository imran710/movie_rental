using Core.Domain.Helper;

namespace Core.Features.Users.ValueObject.Password;

public class UserPassword
{
    public const int MinLength = 8;
    public const int MaxLength = 64;

    public string PasswordHash { get; private set; } = string.Empty;

    private UserPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public static UserPassword NoPassword => new(string.Empty);

    public static Result<UserPassword> Create(Value<string> password, SecurityHelper securityHelper)
    {
        var errors = new ValidationErrors();

        switch (password.Data.Length)
        {
            case < MinLength:
                errors.Add(password, UserPasswordErrors.TooShort);
                break;
            case > MaxLength:
                errors.Add(password, UserPasswordErrors.TooLong);
                break;
        }

        return errors.Count switch
        {
            0 => new UserPassword(securityHelper.HashPassword(password.Data)),
            _ => Error.Validation(errors),
        };
    }
}
