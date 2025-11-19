namespace Core.Features.Users.ValueObject.PersonalName;

public class UserPersonalNameErrors
{
    public static readonly ValidationError MinimumLength
        = ValidationError.Create($"Must be at least {UserPersonalName.MinLength} characters long", code: "Error.PersonalName.MinLength");
    public static readonly ValidationError MaximumLength
        = ValidationError.Create($"Can not be greater than {UserPersonalName.MaxLength} characters", code: "Error.PersonalName.MaxLength");
}

