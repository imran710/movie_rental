using Core.Domain.Common;
using Core.Domain.Helper;
using Core.Features.Common.Service;
using Core.Features.PermissionManagement.Common.System;
using Core.Features.PermissionManagement.Roles;
using Core.Features.PermissionManagement.UserRoles;
using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.Password;
using Core.Features.Users.ValueObject.PersonalName;
using Core.Features.Users.ValueObject.Username;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Core.Features.Users.UseCase.CreateAdminUser;

public class CreateAdminUserHandler(AppDbContext appDbContext, SecurityHelper securityHelper, PermissionManagerService permissionManagerService) : BaseHandler<CreateAdminUserRequest, CreateAdminUserResponse>
{
    protected override async Task<Result<CreateAdminUserResponse>> Handle(CreateAdminUserRequest request, CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        await permissionManagerService.HasPermissionAsync(currentUser.UserId, RoleType.Admin, SystemPermissions.User.CreateUser);

        // Validate
        var userPersonalName = UserPersonalName.Create(
            Value.New(request.PersonalName.FirstName),
            Value.New(request.PersonalName.LastName));
        var email = UserEmail.Create(Value.New(request.Email));
        var password = UserPassword.Create(Value.New(request.Password), securityHelper);

        var result = userPersonalName.Merge(email, password);
        if (result.IsError)
            return result.Error;

        // Initialize
        var user = User.RegisterWithEmail(
            email.Value,
            UserUsername.GenerateUnique(UserPersonalName.DefaultName),
            password.Value,
            userPersonalName.Value);
        if (user.IsError)
            return user.Error;

        // Get user if exists
        var existingUser = await appDbContext.Users.FindByEmail(email.Value).FirstOrDefaultAsync(cancellationToken);
        if (existingUser is not null)
        {
            return Error.Failure("User already exists");
        }

        var role = await appDbContext.Roles.GetRoleById(request.RoleId).FirstOrDefaultAsync(cancellationToken);
        if (role is null)
        {
            return Error.Unexpected("No role exists");
        }
        var userRole = UserRole.Create(user.Value, role);

        appDbContext.Users.Add(user.Value);
        appDbContext.UserRoles.Add(userRole);

        await appDbContext.SaveChangesAsync(cancellationToken);

        return new CreateAdminUserResponse(user.Value.Id);
    }
}

