using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Features.PermissionManagement.RolePermissions;
using Core.Features.PermissionManagement.UserRoles;

namespace Core.Features.PermissionManagement.Roles;

public class Role : IEntity, IUpdatableEntity, IDeletableEntity
{
    public long Id { get; init; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public RoleType Type { get; private set; }
    public required bool IsSystemManaged { get; init; }

    public UpdateInfo UpdateInfo { get; private set; } = UpdateInfo.NotUpdated;
    public DeletionInfo DeletionInfo { get; private set; } = DeletionInfo.NoDeleted();

    public ICollection<RolePermission> RolePermissions { get; private set; } = [];
    public ICollection<UserRole> UserRoles { get; private set; } = [];

    public static Result<Role> Create(Value<string> name, Value<string> description)
    {
        var validationErrors = new ValidationErrors();

        if (name.Data.Length == 0)
            validationErrors.Add(name, ValidationError.Create("Name cannot be empty", "Role.Name.Empty"));
        if (description.Data.Length == 0)
            validationErrors.Add(description, ValidationError.Create("Description cannot be empty", "Role.Description.Empty"));

        if (validationErrors.HasErrors)
            return Error.Validation(validationErrors);

        return new Role
        {
            Name = name.Data,
            Description = description.Data,
            IsSystemManaged = false,
        };
    }

    public static Role CreateSystemRole(string name, string description)
    {
        return new Role
        {
            Name = name,
            Description = description,
            IsSystemManaged = true,
        };
    }

    public void Edit(Role role, long userId)
    {
        Name = role.Name;
        Description = role.Description;
        UpdateInfo.MarkAsUpdated(userId);
    }
}
