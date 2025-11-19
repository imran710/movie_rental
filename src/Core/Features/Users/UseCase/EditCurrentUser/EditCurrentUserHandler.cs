using Core.Domain.Common;
using Core.Domain.Extensions;
using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.PersonalName;
using Core.Features.Users.ValueObject.Username;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Storage.Helper;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Users.UseCase.EditCurrentUser;

public class EditCurrentUserHandler(AppDbContext appDbContext, FileHelper fileHelper) : BaseHandler<EditCurrentUserRequest, EditCurrentUserResponse>
{
    protected override async Task<Result<EditCurrentUserResponse>> Handle(EditCurrentUserRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var user = await appDbContext.Users.FindById(currentUser.UserId).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        if (user is null)
            return Error.NotFound();

        if (request.UserProfileImage is not null)
        {
            if (!FileHelper.IsValidFileType(request.UserProfileImage, SupportedFileType.Image))
                return Error.Failure("Unsupported image file");

            user.UserProfileImageUrl = await fileHelper.SaveFileAsync(request.UserProfileImage, "UserProfile").ConfigureAwait(false);
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            var email = UserEmail.Create(Value.New(request.Email));
            if (email.IsError)
                return email.Error;

            if (email.Value != user.Email)
            {
                var emailCount = await appDbContext.Users.FindByEmail(email.Value).CountAsync(cancellationToken).ConfigureAwait(false);
                if (emailCount > 0)
                    return Error.Failure("Account created with this Email already");

                user.Email.SetEmail(email.Value);
            }
        }

        if (!string.IsNullOrEmpty(request.Username))
        {
            var username = UserUsername.Create(Value.New(request.Username));
            if (username.IsError)
                return username.Error;

            if (username.Value != user.Username)
            {
                var emailCount = await appDbContext.Users.FindByUsername(username.Value).CountAsync(cancellationToken).ConfigureAwait(false);
                if (emailCount > 0)
                    return Error.Failure("Account created with this username already");

                user.Username.SetUsername(username.Value);
            }
        }

        if (request.PersonalName is not null || !string.IsNullOrEmpty(request.PersonalName?.FirstName) || !string.IsNullOrEmpty(request.PersonalName?.LastName))
        {
            var personalName = request.PersonalName.MapToUserPersonalName();
            if (personalName.IsError)
                return personalName.Error;

            if (personalName.Value != user.PersonalName)
            {
                user.SetPersonalName(personalName.Value);
            }
        }

        await appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new EditCurrentUserResponse(user.UserProfileImageUrl);
    }
}
