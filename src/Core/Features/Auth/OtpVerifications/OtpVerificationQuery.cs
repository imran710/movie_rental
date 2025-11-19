using Core.Features.Users.ValueObject.Emails;

namespace Core.Features.Auth.OtpVerifications;

public static class OtpVerificationQuery
{
    public static IQueryable<OtpVerification> FindByUserId(this IQueryable<OtpVerification> query, long userId, string otp)
    {
        query = query.Where(x => x.UserId == userId && !x.IsVerified && x.Otp == otp);

        return query;
    }

    public static IQueryable<OtpVerification> FindByUserEmail(this IQueryable<OtpVerification> query, UserEmail userEmail)
    {
        query = query.Where(x => x.User!.Email.Email == userEmail.Email && !x.IsVerified);

        return query;
    }

    public static IQueryable<OtpVerification> FindVerifiedLoginOtpByUserId(this IQueryable<OtpVerification> query, long userId)
    {
        query = query.Where(x => x.UserId == userId && !x.IsVerified && x.Purpose == "Login");

        return query;
    }

    public static IQueryable<OtpVerification> FindVerifiedResetPasswordOtpByUserId(this IQueryable<OtpVerification> query, long userId, string otp)
    {
        query = query.Where(x => x.UserId == userId && !x.IsVerified && x.Purpose == "PasswordReset" && x.Otp == otp);

        return query;
    }
}
