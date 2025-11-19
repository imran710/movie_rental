using Core.Features.Rentals.Entity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.Rentals.Configure;
public class ConfigureMovieEntity : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(r => r.Id);

        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());
    }
}
