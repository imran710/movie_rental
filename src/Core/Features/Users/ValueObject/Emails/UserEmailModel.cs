namespace Core.Features.Users.ValueObject.Emails;

public class UserEmailModel
{
    public required string Email { get; init; }
    public required bool EmailConfirmed { get; init; }
}
