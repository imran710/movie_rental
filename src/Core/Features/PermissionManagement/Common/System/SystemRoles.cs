using Core.Features.PermissionManagement.Roles;

namespace Core.Features.PermissionManagement.Common.System;

public static class SystemRoles
{
    public static readonly Role Admin = Role.CreateSystemRole("Admin", "Administrator");
    public static readonly Role Customer = Role.CreateSystemRole("Customer", "Registered customer");
    public static readonly Role Developer = Role.CreateSystemRole("Developer", "Developer");

    public static readonly IReadOnlyList<Role> AllRoles =
    [
        Admin,
        Customer,
        Developer
    ];
}
