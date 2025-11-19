using Core.Features.PermissionManagement.Permissions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.PermissionManagement.Configure;

public class ConfigurePermissionEntity : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.Property(r => r.Code).IsRequired();
        builder.Property(r => r.IsSystemManaged).IsRequired();

        builder.HasIndex(p => p.Code).IsUnique();

        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());
    }
}
