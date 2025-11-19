using Core.Features.PermissionManagement.Permissions;

namespace Core.Features.PermissionManagement.UseCase.GetSubscriptionPermissions;

public class GetSubscriptionPermissionResponse
{
    public IReadOnlyList<Permission> Permissions { get; set; } = [];
}

