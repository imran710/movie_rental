using Core.Domain.Common;
using Core.Domain.Helper;
using Core.Domain.Helper.Enums;
using Core.Features.Auth.UseCase.EmailLogin;
using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.Password;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Users.UseCase.EditUserPassword;

public class EditUserPasswordHandler(AppDbContext appDbContext, SecurityHelper securityHelper) : BaseHandler<EditUserPasswordRequest, EditUserPasswordResponse>
{
    protected override async Task<Result<EditUserPasswordResponse>> Handle(EditUserPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var user = await appDbContext.Users.FindById(currentUser.UserId).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (user is null)
            return Error.NotFound();
        if (securityHelper.VerifyHashedPassword(user.Password.PasswordHash, request.currentPassword) != UserPasswordVerificationResult.Success)
            return EmailLoginErrors.InvalidCredentials;

        var passwordResult = UserPassword.Create(Value.New(request.password), securityHelper);
        User.EditPassword(user, passwordResult.Value);

        await appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new EditUserPasswordResponse(user.Id);
    }
}