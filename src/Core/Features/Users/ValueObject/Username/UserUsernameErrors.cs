namespace Core.Features.Users.ValueObject.Username;

public class UserUsernameErrors
{
    public static readonly ValidationError MinimumLength
        = ValidationError.Create($"Minimum length must be at least {UserUsername.MinLength} characters long", code: "Error.UserName.MinLength");
    public static readonly ValidationError MaximumLength
        = ValidationError.Create($"Maximum length can not be greater than {UserUsername.MaxLength} characters long", code: "Error.UserName.MaxLength");
    public static readonly ValidationError InvalidCharacters
        = ValidationError.Create("Invalid characters is not allowed", code: "Error.UserName.InvalidCharacters");
}
