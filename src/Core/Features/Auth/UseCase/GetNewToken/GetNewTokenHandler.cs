using Core.Domain.Common;
using Core.Domain.Helper;
using Core.Features.Auth.Helper;
using Core.Features.Auth.RefreshTokens;
using Core.Features.PermissionManagement.UserRoles;
using Core.Features.Users.Common;
using Core.Features.Users.Entity;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Auth.UseCase.GetNewToken;

public class GetNewTokenHandler(AppDbContext appDbContext, IJwtTokenHelper jwtTokenHelper, SecurityHelper securityHelper) : BaseHandler<GetNewTokenRequest, GetNewTokenResponse>
{
    protected override async Task<Result<GetNewTokenResponse>> Handle(GetNewTokenRequest rawRequest, CancellationToken cancellationToken = default)
    {
        var request = rawRequest with { RefreshToken = securityHelper.Decrypt(rawRequest.RefreshToken)};

        var refreshToken = await appDbContext.RefreshTokens.Where(x => x.Token == request.RefreshToken).FirstOrDefaultAsync(cancellationToken);
        if (refreshToken is null)
            return GetNewTokenErrors.InvalidToken;

        if (refreshToken.IsExpired)
            return GetNewTokenErrors.TokenExpired;

        var user = await appDbContext.Users.FindById(refreshToken.UserId).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        if (user is null)
            return GetNewTokenErrors.InvalidToken;

        // Roles
        var userRoles = await appDbContext.UserRoles
            .GetUserRolesByUser(user.Id)
            .Include(x => x.Role)
            .Select(x => x.Role!)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var newRefreshToken = RefreshToken.Create(user.Id, _httpContext.GetClientIpAddress(), jwtTokenHelper.JwtOption);
        if (newRefreshToken.IsError)
            return newRefreshToken.Error;

        appDbContext.RefreshTokens.Add(newRefreshToken.Value);
        await appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        var claims = UserClaimGenerator.GetUserClaims(user.Id, user.Username, userRoles);
        return new GetNewTokenResponse(
            new TokenInfoModel(
                jwtTokenHelper.GenerateAccessToken(claims),
                jwtTokenHelper.JwtOption.AccessToken.ExpireInMinutes,
                newRefreshToken.Value.EncryptedToken(securityHelper)));
    }
}
