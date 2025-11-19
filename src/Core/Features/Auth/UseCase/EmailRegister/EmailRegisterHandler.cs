using Core.Domain.Common;
using Core.Domain.Helper;
using Core.Domain.Logger;
using Core.Domain.Option;
using Core.Features.Auth.OtpVerifications;
using Core.Features.PermissionManagement.Common.System;
using Core.Features.PermissionManagement.Roles;
using Core.Features.PermissionManagement.UserRoles;
using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.Password;
using Core.Features.Users.ValueObject.PersonalName;
using Core.Features.Users.ValueObject.Username;
using Core.Infrastructure.Database;
using Core.Infrastructure.Email;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Features.Auth.UseCase.EmailRegister;

public class EmailRegisterHandler(
    AppDbContext appDbContext,
    SecurityHelper securityHelper,
    IEmail emailSender,
    IOptions<DomainOption.OtpVerificationOption> otpVerificationOptions,
    IOptionsMonitor<DomainOption.AuthOption> authOptions) : BaseHandler<EmailRegisterRequest, Success<EmailRegisterResponse>>
{
    protected override async Task<Result<Success<EmailRegisterResponse>>> Handle(EmailRegisterRequest request, CancellationToken cancellationToken = default)
    {
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
            return EmailRegisterErrors.AlreadyRegistered;
        }

        var role = await appDbContext.Roles.GetRoleByName(SystemRoles.Customer.Name).FirstOrDefaultAsync(cancellationToken);
        if (role is null)
        {
            return Error.Unexpected("No role exists");
        }
        var userRole = UserRole.Create(user.Value, role); // Assign customer role

        using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        appDbContext.Users.Add(user.Value);
        appDbContext.UserRoles.Add(userRole);

        
            await appDbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return SuccessBuilder.Success(
                "User registered successfully",
                new EmailRegisterResponse(EmailRegisterFlowType.Direct.ToString()));
    }
}
