namespace Core.Features.Users.ValueObject.Password;

public static class UserPasswordErrors
{
    public static readonly ValidationError TooShort
        = ValidationError.Create($"Must be at least {UserPassword.MinLength} characters long", code: "Error.UserPassword.TooShort");
    public static readonly ValidationError TooLong
        = ValidationError.Create($"Can not be greater than {UserPassword.MaxLength} characters", code: "Error.UserPassword.TooLong");
}