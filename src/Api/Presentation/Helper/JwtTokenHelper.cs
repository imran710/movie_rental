using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Core.Domain.Helper;
using Core.Infrastructure.Jwt;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Presentation.Helper;

public class JwtTokenHelper(IOptions<JwtOption> jwtOptions, IDateTimeHelper dateTimeHelper) : IJwtTokenHelper
{
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    public JwtOption JwtOption => jwtOptions.Value;

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var jwtOption = jwtOptions.Value;
        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        var encodedKey = Encoding.UTF8.GetBytes(jwtOption.AccessToken.SecretKey);
#if DEBUG
        var expiredAt = dateTimeHelper.UtcNow.AddMinutes(2880); // 2 days
#else
        var expiredAt = dateTimeHelper.UtcNow.AddMinutes(jwtOption.AccessToken.ExpireInMinutes);
#endif

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = expiredAt.UtcDateTime,
            Audience = jwtOption.Audience,
            Issuer = jwtOption.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(encodedKey), SecurityAlgorithms.HmacSha256Signature),
        };

        return TokenHandler.WriteToken(TokenHandler.CreateToken(tokenDescriptor));
    }
}
