using Core.Domain.Common;
using Core.Features.Users.Entity;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Users.UseCase.DeleteAccount;

public class DeleteAccountHander(AppDbContext appDbContext) : BaseHandler<DeleteAccountRequest, DeleteAccountResponse>
{
    protected override async Task<Result<DeleteAccountResponse>> Handle(DeleteAccountRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = request.UserId ?? _httpContext.GetCurrentUser().UserId;

        var user = await appDbContext.Users.FindById(currentUser).FirstOrDefaultAsync(cancellationToken);
        if (user == null)
            return Error.NotFound("Current user information is not found");

        user.DeletionInfo.MarkAsDeleted();
        await appDbContext.SaveChangesAsync(cancellationToken);

        return new DeleteAccountResponse();
    }
}
