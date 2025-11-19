using Core.Infrastructure.Storage.Common;

namespace Core.Infrastructure.Storage.FileSystem;

public record FileSystemOption
{
    public const string SectionName = $"{StorageOption.SectionName}.{nameof(StorageOption.FileSystem)}";

    public required string RootPath { get; init; }
}
