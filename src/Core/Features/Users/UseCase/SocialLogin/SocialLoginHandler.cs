using Core.Domain.Common;
using Core.Domain.Helper;
using Core.Domain.Option;
using Core.Features.Auth.DeviceInformations;
using Core.Features.Auth.Helper;
using Core.Features.Auth.LoginActivities;
using Core.Features.Auth.RefreshTokens;
using Core.Features.Users.Common;
using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.Password;
using Core.Features.Users.ValueObject.PersonalName;
using Core.Features.Users.ValueObject.Username;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Core.Features.Users.UseCase.SocialLogin;

public class SocialLoginHandler(AppDbContext appDbContext,
    SecurityHelper securityHelper,
    IJwtTokenHelper jwtTokenHelper,
    IOptionsMonitor<ApiCallKeys> apiCallKeysOptions) : BaseHandler<SocialLoginRequest, SocialLoginResponse>
{
    protected override async Task<Result<SocialLoginResponse>> Handle(SocialLoginRequest request, CancellationToken cancellationToken = default)
    {
        // Validate token
        var socialKey = apiCallKeysOptions.CurrentValue.SocialKey;
        if (string.IsNullOrWhiteSpace(request.token) || !request.token.Equals(socialKey))
        {
            return Error.Unexpected("Social key is not valid, provide a valid social key.");
        }
        var email = UserEmail.Create(Value.New(request.Email));
        var existingUser = await appDbContext.Users.FindByEmail(email.Value)
          .FirstOrDefaultAsync(cancellationToken);
      

        User finalUser;
        if (existingUser is not null)
        {
            finalUser = existingUser;
        }
        else
        {
            // Prepare values
            var userPersonalName = UserPersonalName.Create(
                Value.New(request.PersonalName.FirstName),
                Value.New(request.PersonalName.LastName));


            var dummyPassword = UserPassword.Create(Value.New("dummyPass"), securityHelper);

            var result = userPersonalName.Merge(email, dummyPassword);
            if (result.IsError) return result.Error;
            var newUser = User.RegisterWithEmail(
                email.Value,
                UserUsername.GenerateUnique(userPersonalName.Value),
                dummyPassword.Value,
                userPersonalName.Value);

            if (newUser.IsError) return newUser.Error;

            finalUser = newUser.Value;
            appDbContext.Users.Add(finalUser);
            await appDbContext.SaveChangesAsync(cancellationToken);
            var id = finalUser.Id;
        }

        // Generate tokens
        var claims = UserClaimGenerator.GetUserClaims(finalUser.Id, finalUser.Username);
        var accessToken = jwtTokenHelper.GenerateAccessToken(claims);
        var refreshToken = RefreshToken.Create(finalUser.Id, _httpContext.GetClientIpAddress(), jwtTokenHelper.JwtOption);
        if (refreshToken.IsError) return refreshToken.Error;

        // Create login activity and device info
        var deviceInfo = DeviceInformation.Create(request.FcmToken, finalUser.Id, _httpContext.GetDeviceInformation());
        var loginActivity = LoginActivity.Create(finalUser.Id, _httpContext.GetClientIpAddress(), deviceInfo);

        appDbContext.LoginActivities.Add(loginActivity);
        appDbContext.RefreshTokens.Add(refreshToken.Value);

            await appDbContext.SaveChangesAsync(cancellationToken);
     

        return new SocialLoginResponse(
            finalUser.MapToUserModel(),
            new TokenInfoModel(
                accessToken,
                jwtTokenHelper.JwtOption.AccessToken.ExpireInMinutes,
                refreshToken.Value.EncryptedToken(securityHelper)));
    }
}
