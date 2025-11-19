using Core.Features.Auth.RefreshTokens;
using Core.Infrastructure.Database.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.Auth.Configure;

public class ConfigureRefreshTokenEntity : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Token);

        builder.ComplexProperty(u => u.CreationTime, options => options.AddCreationTimeConfig());
    }
}
