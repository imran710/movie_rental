using Core.Domain.Common;
using Core.Domain.Helper;
using Core.Domain.Helper.Enums;
using Core.Domain.Option;
using Core.Features.Auth.DeviceInformations;
using Core.Features.Auth.Helper;
using Core.Features.Auth.LoginActivities;
using Core.Features.Auth.OtpVerifications;
using Core.Features.Auth.RefreshTokens;
using Core.Features.Auth.UseCase.EmailRegister;
using Core.Features.PermissionManagement.Common.Mapper;
using Core.Features.PermissionManagement.UserRoles;
using Core.Features.Users.Common;
using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.Emails;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Core.Features.Auth.UseCase.EmailLogin;

public class EmailLoginHandler(
    AppDbContext appDbContext,
    SecurityHelper securityHelper,
    IJwtTokenHelper jwtTokenHelper,
    IOptionsMonitor<DomainOption.AuthOption> authOptions) : BaseHandler<EmailLoginRequest, EmailLoginResponse>
{
    protected override async Task<Result<EmailLoginResponse>> Handle(EmailLoginRequest request, CancellationToken cancellationToken = default)
    {
        // Check if the email is valid
        var email = UserEmail.Create(Value.New(request.Email));
        if (email.IsError)
            return email.Error;

        // Check if the user exists
        var user = await appDbContext.Users.FindByEmail(email.Value).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            await Task.Delay(50, cancellationToken); // To avoid timing attacks
            return EmailLoginErrors.InvalidCredentials;
        }

     /*   if (authOptions.CurrentValue.EmailRegisterFlowType != EmailRegisterFlowType.Direct)
        {
            // Check if OTP is verified
            var otp = await appDbContext.OtpVerifications.FindVerifiedLoginOtpByUserId(user.Id).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            if (otp is not null && !otp.IsVerified)
                return EmailLoginErrors.OtpNotVerified;
        }*/

        // Check if the password is correct
        if (securityHelper.VerifyHashedPassword(user.Password.PasswordHash, request.Password) != UserPasswordVerificationResult.Success)
            return EmailLoginErrors.InvalidCredentials;

        // Roles
        var userRoles = await appDbContext.UserRoles
            .GetUserRolesByUser(user.Id)
            .Include(x => x.Role)
            .Select(x => x.Role!)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        // Generate the claims
        var claims = UserClaimGenerator.GetUserClaims(user.Id, user.Username, userRoles);
        var accessToken = jwtTokenHelper.GenerateAccessToken(claims);
        var newRefreshToken = RefreshToken.Create(user.Id, _httpContext.GetClientIpAddress(), jwtTokenHelper.JwtOption);
        if (newRefreshToken.IsError)
            return newRefreshToken.Error;

        // Add device information
        var deviceInformation = DeviceInformation.Create(request.FcmToken, user.Id, _httpContext.GetDeviceInformation());
        var loginActivity = LoginActivity.Create(user.Id, _httpContext.GetClientIpAddress(), deviceInformation);
        appDbContext.LoginActivities.Add(loginActivity);

        // Add the refresh token to the database
        appDbContext.RefreshTokens.Add(newRefreshToken.Value);
        await appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new EmailLoginResponse(
            user.MapToUserModel(),
            new TokenInfoModel(
                accessToken,
                jwtTokenHelper.JwtOption.AccessToken.ExpireInMinutes,
                newRefreshToken.Value.EncryptedToken(securityHelper)),
            [.. userRoles.Select(x => x.MapToShortModel())]);
    }
}
