using Core.Domain.Logger;
using Core.Infrastructure.Common.Logger;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Infrastructure.Jwt;

public record JwtOptionValidation : IValidateOptions<JwtOption>
{
    private readonly static ILogger<StaticLogger> Logger = StaticLogger.Logger;

    public ValidateOptionsResult Validate(string? name, JwtOption options)
    {
        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            var message = "Issuer can not be null or whitespace";
            Logger.OptionValidationFailed(JwtOption.SectionName, message);
            return ValidateOptionsResult.Fail(message);
        }

        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            var message = "Audience can not be null or whitespace";
            Logger.OptionValidationFailed(JwtOption.SectionName, message);
            return ValidateOptionsResult.Fail(message);
        }

        if (string.IsNullOrWhiteSpace(options.AccessToken.SecretKey))
        {
            var message = "Secret key cannot be empty";
            Logger.OptionValidationFailed(JwtOption.SectionName, message);
            return ValidateOptionsResult.Fail(message);
        }

        if (options.AccessToken.ExpireInMinutes <= 0)
        {
            var message = "Token expiration must be positive";
            Logger.OptionValidationFailed(JwtOption.SectionName, message);
            return ValidateOptionsResult.Fail(message);
        }

        if (!options.RefreshToken.IsRefreshTokenPositive())
        {
            var message = "Refresh token expiration must be positive";
            Logger.OptionValidationFailed(JwtOption.SectionName, message);
            return ValidateOptionsResult.Fail(message);
        }

        if (!options.AccessToken.IsSecretKeyLengthSecure)
        {
            var message = "AccessToken secret key must be at least 32 characters long";
            Logger.OptionValidationFailed(JwtOption.SectionName, message);
            return ValidateOptionsResult.Fail(message);
        }

        if (!options.RefreshToken.IsExpirationValid(options.AccessToken.ExpireInMinutes))
        {
            var message = "Invalid refresh token expiration";
            Logger.OptionValidationFailed(JwtOption.SectionName, message);
            return ValidateOptionsResult.Fail(message);
        }

        return ValidateOptionsResult.Success;
    }
}
