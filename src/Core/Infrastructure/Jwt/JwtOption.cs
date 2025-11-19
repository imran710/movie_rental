namespace Core.Infrastructure.Jwt;

public record JwtOption
{
    public const string SectionName = "Auth:Jwt";

    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required AccessTokenOption AccessToken { get; init; }
    public required RefreshTokenOption RefreshToken { get; init; }

    public record AccessTokenOption
    {
        public required int ExpireInMinutes { get; init; }
        public required string SecretKey { get; init; }

        public bool IsSecretKeyLengthSecure => SecretKey.Length >= 32;
    }

    public record RefreshTokenOption
    {
        public required int ExpireInMinutesIfNotRemember { get; init; }
        public required int ExpireInMinutesIfRemember { get; init; }
        public required int ClockSkewInMinutes { get; init; }

        public bool IsRefreshTokenPositive()
            => ExpireInMinutesIfNotRemember >= 0 || ExpireInMinutesIfRemember >= 0;

        public bool IsExpirationValid(int accessTokenExpiration) =>
            ExpireInMinutesIfNotRemember > accessTokenExpiration && ExpireInMinutesIfRemember > accessTokenExpiration;
    }
}
