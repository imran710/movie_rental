namespace Core.Infrastructure.Storage.FileSystem;

public class FileSystemStorage(string baseDirectory) : IStorage
{
    public async Task<Result<string>> UploadAsync(string filePath, Stream fileStream, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return Error.Unexpected("File path cannot be null or empty");
        }
        if (!fileStream.CanRead)
        {
            return Error.Unexpected("Invalid file stream provided");
        }
        
        var fullPath = Path.GetFullPath(Path.Combine(baseDirectory, filePath));
        if (!fullPath.StartsWith(baseDirectory, StringComparison.OrdinalIgnoreCase))
        {
            return Error.Unexpected("Access to path outside base directory is not allowed");
        }
        
        var directory = Path.GetDirectoryName(fullPath);
        if (string.IsNullOrEmpty(directory))
        {
            throw new NullReferenceException(directory);
        }
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        const int bufferSize = 81920;
        await using var destinationStream = new FileStream(
            fullPath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize,
            FileOptions.Asynchronous);

        try
        {
            await fileStream.CopyToAsync(destinationStream, token)
                .ConfigureAwait(false);

            return Result.Success(fullPath);
        }
        catch (Exception ex) when (ex is IOException or OutOfMemoryException)
        {
            try
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch
            {
                // TODO: Do something
            }
            return Error.Unexpected($"Failed to write file: {ex.Message}");
        }
    }

    public Task<Result<Stream>> DownloadAsync(string filePath, CancellationToken _ = default)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return Task.FromResult(Result.Error<Stream>(Error.Unexpected("File path cannot be null or empty")));
        }

        var fullPath = Path.GetFullPath(Path.Combine(baseDirectory, filePath));
        if (!fullPath.StartsWith(baseDirectory, StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(Result.Error<Stream>(Error.Unexpected("Access to file path outside base directory is not allowed")));
        }
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        return Task.FromResult(Result.Success<Stream>(File.OpenRead(fullPath)));
    }
    
    public Task<Result<Success>> DeleteAsync(string filePath, CancellationToken _ = default)
    {
        var fullPath = Path.Combine(baseDirectory, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.FromResult(Result.Success(SuccessBuilder.Default));
    }
}