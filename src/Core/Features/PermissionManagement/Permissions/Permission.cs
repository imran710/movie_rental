using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Features.PermissionManagement.RolePermissions;

namespace Core.Features.PermissionManagement.Permissions;

public class Permission : IEntity, IDeletableEntity
{
    public long Id { get; set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public required string Code { get; init; }
    public required bool IsSystemManaged { get; init; }

    public DeletionInfo DeletionInfo { get; private set; } = DeletionInfo.NoDeleted();

    public ICollection<RolePermission> RolePermissions { get; private set; } = [];

    public static Result<Permission> CreatePermission(Value<string> name, Value<string?> description, Value<string>? code = null)
    {
        var validationError = new ValidationErrors();
        if (name.Data is null)
        {
            validationError.Add(name, ValidationError.Create("Name is required", "Error.Permission.NameRequired"));
        }
        if (description.Data is null)
        {
            validationError.Add(description, ValidationError.Create("Description is required", "Error.Permission.DescriptionRequired"));
        }
        if (code.HasValue && code.Value.Data is null)
        {
            validationError.Add(code.Value, ValidationError.Create("Code is required", "Error.Permission.CodeRequired"));
        }

        return validationError.Count switch
        {
            0 => new Permission
            {
                Name = name.Data ?? string.Empty,
                Description = description.Data,
                Code = code?.Data ?? string.Empty,
                IsSystemManaged = false,
            },
            _ => Error.Validation(validationError),
        };
    }

    public static Permission CreateSystemPermission(string code, string name = "")
    {
        return new Permission
        {
            Name = name,
            Code = code,
            IsSystemManaged = true,
            Description = null,
        };
    }

    public void Edit(Permission permission)
    {
        Name = permission.Name;
        Description = permission.Description;
    }
}
