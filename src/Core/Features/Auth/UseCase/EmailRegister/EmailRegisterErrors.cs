namespace Core.Features.Auth.UseCase.EmailRegister;

public static class EmailRegisterErrors
{
    public static readonly Error AlreadyRegistered
        = Error.Failure("Already registered with this Email", code: "Error.EmailLogin.AlreadyRegistered");
}
