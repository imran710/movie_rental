namespace Core.Features.Users.Common;

public record TokenInfoModel(string AccessToken, int AccessTokenExpireInMinutes, string RefreshToken);
