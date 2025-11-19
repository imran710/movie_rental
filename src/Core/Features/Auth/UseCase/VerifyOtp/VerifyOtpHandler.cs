using Core.Domain.Common;
using Core.Domain.Option;
using Core.Features.Auth.Aggregates;
using Core.Features.Auth.OtpVerifications;
using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.Emails;
using Core.Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Core.Features.Auth.UseCase.VerifyOtp;

public class VerifyOtpHandler(AppDbContext appDbContext, IOptions<DomainOption.OtpVerificationOption> options) : BaseHandler<VerityOtpRequest, Success>
{
    protected override async Task<Result<Success>> Handle(VerityOtpRequest request, CancellationToken cancellationToken = default)
    {
        var email = UserEmail.Create(Value.New(request.Email));
        if (email.IsError)
            return email.Error;

        var user = await appDbContext.Users.FindByEmail(email.Value).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
            return OtpVerificationErrors.InvalidOtp;

        var otp = await appDbContext
            .OtpVerifications
            .FindByUserId(user.Id, request.Otp)
            .FirstOrDefaultAsync(cancellationToken);
        if (otp is null)
            return OtpVerificationErrors.InvalidOtp;
        if (otp.IsExpired())
            return OtpVerificationErrors.OtpExpired;

        var verifyOtp = new UserOtpVerificationAggregate(otp, user).VerifyOtp(request.Otp, options.Value);
        if (verifyOtp.IsError)
            return verifyOtp.Error;

        return SuccessBuilder.Default;
    }

    protected override async ValueTask<Result<Success>> AfterProcess(VerityOtpRequest request, CancellationToken cancellationToken = default)
    {
        await appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new Result<Success>(SuccessBuilder.Default);
    }
}
