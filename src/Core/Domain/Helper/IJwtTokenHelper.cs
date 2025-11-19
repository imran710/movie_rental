using System.Security.Claims;

using Core.Infrastructure.Jwt;

namespace Core.Domain.Helper;

public interface IJwtTokenHelper
{
    JwtOption JwtOption { get; }
    string GenerateAccessToken(IEnumerable<Claim> claims);
}
