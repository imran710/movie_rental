using Core.Features.Rentals.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Rentals.Configure;
public class ConfigureRentalEntity : IEntityTypeConfiguration<RentalE>
{
    public void Configure(EntityTypeBuilder<RentalE> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(r => r.Id);

        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());
    }
}
