using Core.Domain.Common;
using Core.Domain.Helper;
using Core.Features.Auth.OtpVerifications;
using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.Password;
using Core.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Auth.UseCase.ResetPassword;

public class ResetPasswordHandler(AppDbContext appDbContext, SecurityHelper securityHelper) : BaseHandler<ResetPasswordRequest, ResetPasswordResponse>
{
    protected override async Task<Result<ResetPasswordResponse>> Handle(ResetPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var email = UserEmail.Create(Value.New(request.Email));
        if (email.IsError)
            return email.Error;

        var user = await appDbContext.Users.FindByEmail(email.Value).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
            return OtpVerificationErrors.InvalidOtp;

        var otp = await appDbContext.OtpVerifications
            .FindVerifiedResetPasswordOtpByUserId(user.Id, request.Otp)
            .FirstOrDefaultAsync(cancellationToken);

        if (otp is null)
            return OtpVerificationErrors.InvalidOtp;
        if (otp.IsExpired())
            return OtpVerificationErrors.OtpExpired;

        var userPassword = UserPassword.Create(Value.New(request.PassWord), securityHelper);
        User.EditPassword(user, userPassword.Value);

        await appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new ResetPasswordResponse(user.Id);
    }
}
