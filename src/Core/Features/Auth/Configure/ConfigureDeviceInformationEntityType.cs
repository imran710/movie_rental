using Core.Features.Auth.DeviceInformations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Features.Auth.Configure;

public class ConfigureDeviceInformationEntityType : IEntityTypeConfiguration<DeviceInformation>
{
    public void Configure(EntityTypeBuilder<DeviceInformation> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
