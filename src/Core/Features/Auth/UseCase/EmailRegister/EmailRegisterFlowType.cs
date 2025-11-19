namespace Core.Features.Auth.UseCase.EmailRegister;

public enum EmailRegisterFlowType
{
    EmailVerificationWithOtp,
    EmailVerificationWithLink,
    Direct,
}
