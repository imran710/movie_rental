using Core.Features.PermissionManagement.Roles;
using Core.Infrastructure.Database.Converters;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.PermissionManagement.Configure;

public class ConfigureRoleEntity : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.Property(r => r.IsSystemManaged).IsRequired();
        builder.Property(r => r.Type)
            .IsRequired()
            .HasConversion<EnumToStringConverter<RoleType>>()
            .HasDefaultValue(RoleType.User);

        builder.HasIndex(r => r.Name).IsUnique();

        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());
        builder.ComplexProperty(u => u.UpdateInfo, options => options.AddUpdatableConfig());
    }
}
