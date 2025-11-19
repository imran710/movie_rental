namespace Core.Features.Users.ValueObject.PersonalName;

public record UserPersonalName
{
    public const int MinLength = 2;
    public const int MaxLength = 100;

    public string FirstName { get; init; }
    public string LastName { get; init; }

    private UserPersonalName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static Result<UserPersonalName> Create(Value<string> firstName, Value<string> lastName)
    {
        var errors = new ValidationErrors();

        switch (firstName.Data.Length)
        {
            case < MinLength:
                errors.Add(firstName, UserPersonalNameErrors.MinimumLength);
                break;
            case > MaxLength:
                errors.Add(firstName, UserPersonalNameErrors.MaximumLength);
                break;
        }

        switch (lastName.Data.Length)
        {
            case < MinLength:
                errors.Add(lastName, UserPersonalNameErrors.MinimumLength);
                break;
            case > MaxLength:
                errors.Add(lastName, UserPersonalNameErrors.MaximumLength);
                break;
        }

        return errors.Count switch
        {
            0 => new UserPersonalName(firstName.Data, lastName.Data),
            _ => Error.Validation(errors),
        };
    }

    public static UserPersonalName DefaultName => new("Anonymous", "User");

    public string FullName => $"{FirstName} {LastName}";
    public string ShortName => $"{FirstName} {LastName[0]}.";

    public override string ToString() => FullName;
}

