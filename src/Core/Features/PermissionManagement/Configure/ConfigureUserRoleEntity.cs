using Core.Features.PermissionManagement.UserRoles;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.PermissionManagement.Configure;

public class ConfigureUserRoleEntity : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);

        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());
    }
}
