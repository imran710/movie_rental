namespace Core.Features.Users.ValueObject.Emails;

public static class UserEmailErrors
{
    public static readonly ValidationError MinimumLength
        = ValidationError.Create($"Must be at least {UserEmail.MinLength} characters long", code: "Error.UserEmail.MinLength");
    public static readonly ValidationError MaximumLength
        = ValidationError.Create($"Can not be greater than {UserEmail.MaxLength} characters", code: "Error.UserEmail.MaxLength");
    public static readonly ValidationError InvalidEmail
        = ValidationError.Create("Invalid email address", code: "Error.UserEmail.InvalidEmail");
    public static readonly ValidationError AlreadyRegistered
        = ValidationError.Create("Email already registered", code: "Error.UserEmail.AlreadyRegistered");
}

