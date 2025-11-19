using Core.Features.Users.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.Users.Configure;
public class ConfigureUserAchievementEntityType : IEntityTypeConfiguration<UserAchievement>
{
    public void Configure(EntityTypeBuilder<UserAchievement> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
         .WithMany()
         .HasForeignKey(x => x.UserId)
         .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId)
        .HasDatabaseName("IX_UserAchievement_UserId");
        builder.HasIndex(x => x.PointsEarned)
            .HasDatabaseName("IX_UserAchievement_PointsEarned");
        builder.ComplexProperty(u => u.CreationInfo, options => options.AddCreationInfoConfig());
        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());
    }
}