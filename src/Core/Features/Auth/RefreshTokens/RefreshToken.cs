using System.Net;

using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Domain.Helper;
using Core.Features.Users.Entity;
using Core.Infrastructure.Jwt;

namespace Core.Features.Auth.RefreshTokens;

public class RefreshToken : IEntity, ICreationEntity
{
    public long Id { get; }

    public required long UserId { get; init; }
    public User? User { get; }

    public required string Token { get; init; }
    public string EncryptedToken(SecurityHelper securityHelper) => securityHelper.Encrypt(Token);
    public DateTimeOffset ExpiresAt { get; init; }

    public IPAddress IPAddress { get; init; } = IPAddress.None;

    public CreationTime CreationTime { get; } = new();

    public bool IsExpired => DateTimeHelperStatic.Now >= ExpiresAt;

    public static Result<RefreshToken> Create(long userId, IPAddress? ipAddress, JwtOption jwtOption)
    {
        return new RefreshToken
        {
            UserId = userId,
            Token = Guid.CreateVersion7().ToString(),
            ExpiresAt = DateTimeHelperStatic.Now.AddMinutes(jwtOption.RefreshToken.ExpireInMinutesIfRemember),
            IPAddress = ipAddress ?? IPAddress.None,
        };
    }
}
