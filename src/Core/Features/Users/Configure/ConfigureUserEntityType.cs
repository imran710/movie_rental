using Core.Features.Users.Entity;
using Core.Features.Users.ValueObject.ContactInfo;
using Core.Features.Users.ValueObject.Emails;
using Core.Features.Users.ValueObject.PersonalName;
using Core.Features.Users.ValueObject.Username;
using Core.Infrastructure.Database.Converters;
using Core.Infrastructure.Database.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.Users.Configure;

public class ConfigureUserEntityType : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.AddDeletionQueryFilter();
        builder.HasKey(x => x.Id);

        builder.ComplexProperty(u => u.PersonalName, options =>
        {
            options.Property(p => p.FirstName).HasMaxLength(UserPersonalName.MaxLength).HasConversion<NullableStringConverter>().HasColumnName("FirstName");
            options.Property(p => p.LastName).HasMaxLength(UserPersonalName.MaxLength).HasConversion<NullableStringConverter>().HasColumnName("LastName");
        });

        builder.ComplexProperty(u => u.Username, options =>
        {
            options.Property(p => p.Username).HasMaxLength(UserUsername.MaxLength).IsRequired().HasColumnName("Username");
            options.Property(p => p.UsernameNormalized).HasMaxLength(UserUsername.MaxLength).IsRequired().HasColumnName("UsernameNormalized");
        });

        builder.ComplexProperty(u => u.Email, options =>
        {
            options.Property(p => p.Email).HasMaxLength(UserEmail.MaxLength).HasColumnName("Email");
            options.Property(p => p.EmailNormalized).HasMaxLength(UserEmail.MaxLength).HasColumnName("EmailNormalized");
            options.Property(p => p.EmailConfirmed).IsRequired().HasColumnName("EmailConfirmed");
        });

        builder.ComplexProperty(
            u => u.Password,
            options => options.Property(p => p.PasswordHash)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasConversion<NullableStringConverter>()
                .HasColumnName("Password"));
        builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();

        builder.ComplexProperty(u => u.ContactInfo, options =>
        {
            options.Property(p => p.RegionCode).IsRequired(false).HasConversion<NullableStringConverter>().HasMaxLength(10).HasColumnName("RegionCode");
            options.Property(p => p.PhoneNumber).IsRequired(false).HasConversion<NullableStringConverter>().HasMaxLength(UserContactInfo.MaxLength).HasColumnName("PhoneNumber");
            options.Property(p => p.PhoneNumberConfirmed).IsRequired().HasColumnName("PhoneNumberConfirmed");
        });

        builder.ComplexProperty(u => u.DeletionInfo, options => options.AddDeletableConfig());
    }
}

