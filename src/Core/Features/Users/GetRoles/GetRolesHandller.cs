using Core.Domain.Common;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Users.GetRoles;
public class GetRolesHandller(AppDbContext appDbContext) : BaseHandler<GetRolesRequest, GetRolesResponse>
{
    protected override async Task<Result<GetRolesResponse>> Handle(GetRolesRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var roles = await appDbContext.Roles
                                    .ToListAsync(cancellationToken);

        if (roles.Count == 0)
            return Error.NotFound();

        return new GetRolesResponse(roles);
    }
}