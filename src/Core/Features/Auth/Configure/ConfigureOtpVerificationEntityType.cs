using Core.Features.Auth.OtpVerifications;
using Core.Infrastructure.Database.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.Auth.Configure;

public class ConfigureOtpVerificationEntityType : IEntityTypeConfiguration<OtpVerification>
{
    public void Configure(EntityTypeBuilder<OtpVerification> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Verifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Otp).HasMaxLength(OtpVerification.MaxLength);
        builder.Property(x => x.Purpose).HasMaxLength(100);

        builder.ComplexProperty(u => u.CreationTime, options => options.AddCreationTimeConfig());
        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());
    }
}
