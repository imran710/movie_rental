namespace Core.Features.Auth.UseCase.EmailLogin;

public class EmailLoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public string? FcmToken { get; init; }
}
