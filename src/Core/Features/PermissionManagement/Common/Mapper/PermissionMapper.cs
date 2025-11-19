using Core.Features.PermissionManagement.Common.Model;
using Core.Features.PermissionManagement.Permissions;

namespace Core.Features.PermissionManagement.Common.Mapper;

public static class PermissionMapper
{
    public static PermissionModel ToModel(this Permission permission)
    {
        return new PermissionModel
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description,
            IsSystemManaged = permission.IsSystemManaged,
        };
    }
}


