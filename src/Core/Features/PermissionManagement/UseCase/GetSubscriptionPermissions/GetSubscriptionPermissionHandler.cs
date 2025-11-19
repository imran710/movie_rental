using Core.Domain.Common;
using Core.Features.PermissionManagement.Common.System;

namespace Core.Features.PermissionManagement.UseCase.GetSubscriptionPermissions;

public class GetSubscriptionPermissionHandler() : BaseHandler<GetSubscriptionPermissionRequest, GetSubscriptionPermissionResponse>
{
    protected override async Task<Result<GetSubscriptionPermissionResponse>> Handle(GetSubscriptionPermissionRequest request, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        var permissions = SubscriptionPermissions.AllPermissions;

        return new GetSubscriptionPermissionResponse { Permissions = permissions };
    }
}
