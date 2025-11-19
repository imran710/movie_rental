using Core.Features.PermissionManagement.RolePermissions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.PermissionManagement.Configure;

public class ConfigureRolePermissionEntity : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasOne(r => r.Role).WithMany(r => r.RolePermissions).HasForeignKey(r => r.RoleId);
        builder.HasOne(r => r.Permission).WithMany(r => r.RolePermissions).HasForeignKey(r => r.PermissionId);

        builder.HasIndex(r => new { r.RoleId, r.PermissionId }).IsUnique();
        builder.ComplexProperty(r => r.DeletionInfo, options => options.AddDeletableConfig());
    }
}
