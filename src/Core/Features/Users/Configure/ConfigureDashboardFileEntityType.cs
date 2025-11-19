using Core.Features.Users.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.Users.Configure;

public class ConfigureDashboardFileEntityType : IEntityTypeConfiguration<DashboardFile>
{
    public void Configure(EntityTypeBuilder<DashboardFile> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(x => x.Id);
         

        builder.ComplexProperty(u => u.CreationInfo, options => options.AddCreationInfoConfig());
        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());
    }
}
