using System.Buffers;
using System.Text.RegularExpressions;

namespace Core.Features.Users.ValueObject.Emails;

public partial record UserEmail
{
    public const int MinLength = 3;
    public const int MaxLength = 50;
    private static readonly SearchValues<char> AllowedChars = SearchValues.Create("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_+");

    public string Email { get; private set; } = string.Empty;
    public string EmailNormalized { get; private set; } = string.Empty;
    public bool EmailConfirmed { get; private set; } = false;

    private UserEmail(string email, bool emailConfirmed)
    {
        Email = email.ToLowerInvariant();
        EmailNormalized = email.ToUpperInvariant();
        EmailConfirmed = emailConfirmed;
    }

    public static Result<UserEmail> Create(Value<string> email)
    {
        var errors = new ValidationErrors();

        switch (email.Data.Length)
        {
            case < MinLength:
                errors.Add(email, UserEmailErrors.MinimumLength);
                break;
            case > MaxLength:
                errors.Add(email, UserEmailErrors.MaximumLength);
                break;
        }

        if (email.Data.AsSpan().ContainsAnyExcept(AllowedChars) || !IsEmailPartValid(email.Data) || !EmailValidationRegex().IsMatch(email.Data))
            errors.Add(email, UserEmailErrors.InvalidEmail);

        return errors.Count switch
        {
            0 => new UserEmail(email.Data, emailConfirmed: false),
            _ => Error.Validation(errors),
        };
    }

    public void Confirm()
    {
        if (EmailConfirmed)
            throw new InvalidOperationException("Email is already confirmed");

        EmailConfirmed = true;
    }

    public override string ToString() => $"{Email} (Confirmed: {EmailConfirmed})";

    private static bool IsEmailPartValid(string email)
    {
        var parts = email.Split('@');
        if (parts.Length != 2)
            return false;

        var localPart = parts[0];
        var domainPart = parts[1];

        if (localPart.StartsWith('.') || localPart.EndsWith('.') || localPart.Contains(".."))
            return false;

        var domainSections = domainPart.Split('.');
        if (domainSections.Length < 2)
            return false;

        if (domainSections[^1].Length < 2 || domainSections[^1].Length > 6)
            return false;

        return true;
    }

    public void SetEmail(UserEmail email)
    {
        Email = email.Email;
        EmailConfirmed = email.EmailConfirmed;
        EmailNormalized = email.EmailNormalized;
    }

    [GeneratedRegex(@"^(?!.*\.{2})[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-001")]
    private static partial Regex EmailValidationRegex();
}

