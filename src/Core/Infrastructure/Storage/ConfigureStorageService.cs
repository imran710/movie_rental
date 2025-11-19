using Core.Infrastructure.ServiceInjector;
using Core.Infrastructure.Storage.Aws;
using Core.Infrastructure.Storage.Common;
using Core.Infrastructure.Storage.FileSystem;
using Core.Infrastructure.Storage.Helper;

using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Storage;

public class ConfigureStorageService : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddOptions<StorageOption>().BindConfiguration(StorageOption.SectionName);
        services.AddOptions<AwsS3Option>().BindConfiguration(AwsS3Option.SectionName);
        services.AddOptions<FileSystemOption>().BindConfiguration(FileSystemOption.SectionName);
        services.AddSingleton<FileHelper>();
    }
}
