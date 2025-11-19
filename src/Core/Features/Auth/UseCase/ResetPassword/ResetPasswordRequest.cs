namespace Core.Features.Auth.UseCase.ResetPassword;

public record ResetPasswordRequest(string Otp, string Email, string PassWord);