using Core.Infrastructure.Storage.Aws;
using Core.Infrastructure.Storage.FileSystem;

namespace Core.Infrastructure.Storage.Common;

public record StorageOption
{
    public const string SectionName = "Storage";

    public required AwsS3Option S3 { get; init; }
    public required FileSystemOption FileSystem { get; init; }
}
