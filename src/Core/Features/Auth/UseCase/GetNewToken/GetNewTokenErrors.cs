namespace Core.Features.Auth.UseCase.GetNewToken;

public static class GetNewTokenErrors
{
    public static readonly Error InvalidToken = Error.Failure("Invalid token", "Error.NewToken.InvalidToken");
    public static readonly Error TokenExpired = Error.Failure("Token expired", "Error.NewToken.TokenExpired");
}
