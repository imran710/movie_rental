using Core.Features.PermissionManagement.Common.Model;
using Core.Features.PermissionManagement.Roles;

namespace Core.Features.PermissionManagement.Common.Mapper;

public static class RoleMapper
{
    public static RoleModel MapToModel(this Role entity)
    {
        return new RoleModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Type = entity.Type,
            IsSystemManaged = entity.IsSystemManaged,
        };
    }

    public static RoleShortModel MapToShortModel(this Role entity)
    {
        return new RoleShortModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Type = entity.Type,
        };
    }
}
