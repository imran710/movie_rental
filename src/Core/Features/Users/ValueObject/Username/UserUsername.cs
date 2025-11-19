using System.Buffers;
using System.Text.RegularExpressions;

using Core.Features.Users.ValueObject.PersonalName;

namespace Core.Features.Users.ValueObject.Username;

public partial record UserUsername
{
    public const int MinLength = 3;
    public const int MaxLength = 50;
    private static readonly SearchValues<char> AllowedChars = SearchValues.Create("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_");

    public string Username { get; private set; }
    public string UsernameNormalized { get; private set; }

    private UserUsername(string username)
    {
        Username = username;
        UsernameNormalized = username.ToUpperInvariant();
    }

    public static UserUsername GenerateUnique(UserPersonalName userPersonalName)
    {
        if (userPersonalName == UserPersonalName.DefaultName)
            return new UserUsername(UserNameClean().Replace(input : Guid.CreateVersion7().ToString(), replacement: string.Empty));

        var userName = userPersonalName.FirstName + Guid.CreateVersion7();
        var cleanUserName = UserNameClean().Replace(input : userName.ToLowerInvariant(), replacement: string.Empty);
        return new UserUsername(cleanUserName);
    }

    public static Result<UserUsername> Create(Value<string> username)
    {
        var errors = new ValidationErrors();

        switch (username.Data.Length)
        {
            case < MinLength:
                errors.Add(username, UserUsernameErrors.MinimumLength);
                break;
            case > MaxLength:
                errors.Add(username, UserUsernameErrors.MaximumLength);
                break;
        }

        if (username.Data.AsSpan().ContainsAnyExcept(AllowedChars))
            errors.Add(username, UserUsernameErrors.InvalidCharacters);

        return errors.Count switch
        {
            0 => new UserUsername(username.Data),
            _ => Error.Validation(errors),
        };
    }

    public void SetUsername(UserUsername username)
    {
        Username = username.Username;
        UsernameNormalized = username.UsernameNormalized;
    }

    public override string ToString() => Username;

    [GeneratedRegex(@"[^a-z0-9]")]
    private static partial Regex UserNameClean();
}
