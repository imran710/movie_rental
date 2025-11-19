namespace Core.Features.Auth.OtpVerifications;

public static class OtpVerificationErrors
{
    public static readonly Error OtpExpired
        = Error.Failure("OTP has expired", code: "Error.OtpVerification.OtpExpired");
    public static readonly Error AlreadyVerified
        = Error.Failure("OTP is already verified", code: "Error.OtpVerification.AlreadyVerified");
    public static readonly Error InvalidOtp
        = Error.Failure("Invalid OTP provided", code: "Error.OtpVerification.InvalidOtp");
}
