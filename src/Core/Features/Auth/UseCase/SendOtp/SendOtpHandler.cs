using Core.Domain.Common;
using Core.Domain.Option;
using Core.Features.Auth.OtpVerifications;
using Core.Features.Auth.UseCase.EmailRegister;
using Core.Features.Auth.UseCase.ForgetPassword;
using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.Emails;
using Core.Infrastructure.Database;
using Core.Infrastructure.Email;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Core.Features.Auth.UseCase.SendOtp;

public class SendOtpHandler(AppDbContext appDbContext, IOptions<DomainOption.OtpVerificationOption> otpVerificationOptions, IEmail emailSender) : BaseHandler<SendOtpRequest, Success>
{
    protected override async Task<Result<Success>> Handle(SendOtpRequest request, CancellationToken cancellationToken = default)
    {
        var email = UserEmail.Create(Value.New(request.Email));
        if (email.IsError)
            return email.Error;

        var user = await appDbContext.Users.FindByEmail(email.Value).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
            return OtpVerificationErrors.InvalidOtp;

        using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        var otp = OtpVerification.CreateForLogin(user, otpVerificationOptions.Value);
        appDbContext.OtpVerifications.Add(otp);

        await appDbContext.SaveChangesAsync(cancellationToken);

        var emailTemplate = new EmailRegisterOtpVerifyEmailTemplate(
            new EmailRegisterOtpVerifyEmailTemplate.Model
            {
                OTP = otp.Otp,
                OTPExpireInMinutes = otpVerificationOptions.Value.OtpValidityDurationInMinutes
            });
        var emailSendResult = await emailSender.SendEmailAsync([email.Value.Email], emailTemplate.Subject, emailTemplate.Body).ConfigureAwait(false);
        if (emailSendResult.IsError)
        {
            await transaction.RollbackAsync(cancellationToken);
            return emailSendResult.Error;
        }

        await transaction.CommitAsync(cancellationToken);

        return SuccessBuilder.Default;
    }
}