using Core.Domain.Option;
using Core.Features.Auth.OtpVerifications;
using Core.Features.Users.Entity;

namespace Core.Features.Auth.Aggregates;

public class UserOtpVerificationAggregate(OtpVerification otpVerification, User user)
{
    public Result<Success> VerifyOtp(string inputOtp, DomainOption.OtpVerificationOption otpVerificationOption)
    {
        var otpResult = otpVerification.VerifyOtp(inputOtp, otpVerificationOption);
        if (otpResult.IsError)
            return otpResult;

        user.Email.Confirm();

        return SuccessBuilder.Default;
    }
}
