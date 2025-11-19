using System.Reflection.Emit;

using Core.Features.Auth.LoginActivities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.Auth.Configure;

public class ConfigureLoginActivityEntityType : IEntityTypeConfiguration<LoginActivity>
{
    public void Configure(EntityTypeBuilder<LoginActivity> builder)
    {
        builder.HasQueryFilter(la => !la.User!.DeletionInfo.IsDeleted);

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.DeviceInfo)
            .WithMany(x => x.LoginActivities)
            .HasForeignKey(x => x.DeviceInfoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(x => x.LoginActivities)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
