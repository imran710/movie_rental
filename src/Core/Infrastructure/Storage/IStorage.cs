using Library.Results;

namespace Core.Infrastructure.Storage;

public interface IStorage
{
    Task<Result<string>> UploadAsync(string filePath, Stream fileStream, CancellationToken cancellationToken = default);
    Task<Result<Stream>> DownloadAsync(string filePath, CancellationToken cancellationToken = default);
    Task<Result<Success>> DeleteAsync(string filePath, CancellationToken cancellationToken = default);
}
