using Core.Infrastructure.Storage.Common;

namespace Core.Infrastructure.Storage.Aws;

public abstract record AwsS3Option
{
    public const string SectionName = $"{StorageOption.SectionName}.{nameof(StorageOption.S3)}";

    public required AwsS3Admin Admin { get; init; }

    public abstract record AwsS3Admin
    {
        public required string AccessId { get; init; }
        public required string AccessKey { get; init; }
        public required string EndpointS3 { get; init; }
        public required string EndpointIam { get; init; }
    }
}
