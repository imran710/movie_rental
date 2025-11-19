namespace Core.Features.Auth.UseCase.EmailLogin;

public static class EmailLoginErrors
{
    public static readonly Error InvalidCredentials
        = Error.Failure("Invalid credentials", code: "Error.EmailLogin.InvalidCredentials");
    public static readonly Error OtpNotVerified
        = Error.Failure("OTP is not verified yet", code: "Error.EmailLogin.OtpNotVerified");
}
