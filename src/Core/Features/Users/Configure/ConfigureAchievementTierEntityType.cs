using Core.Features.Users.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.Users.Configure;
public class ConfigureAchievementTierEntityType : IEntityTypeConfiguration<AchievementTier>
{
    public void Configure(EntityTypeBuilder<AchievementTier> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

        builder.Property(x => x.MinPoints)
            .IsRequired();

        builder.Property(x => x.MaxPoints)
            .IsRequired();


        builder.ComplexProperty(u => u.CreationInfo, options => options.AddCreationInfoConfig());
        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());

        builder.HasIndex(x => new { x.MinPoints, x.MaxPoints })
        .HasDatabaseName("IX_AchievementTier_MinMaxPoints");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_AchievementTier_Name");
    }
}
