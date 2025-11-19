using System.Net;

using Amazon.S3;
using Amazon.S3.Model;

using Core.Infrastructure.Storage.Common;

using Library.Results;

namespace Core.Infrastructure.Storage.Aws;

public class AwsS3Storage(IAmazonS3 s3Client, string bucketName) : IStorage
{
    public async Task<Result<string>> UploadAsync(string filePath, Stream fileStream, CancellationToken cancellationToken = default)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = filePath,
            InputStream = fileStream,
            UseChunkEncoding = false,
        };

        var response = await s3Client.PutObjectAsync(putRequest, cancellationToken).ConfigureAwait(false);
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new Exception("Failed to upload to S3");
        }

        return $"s3://{bucketName}/{filePath}";
    }

    public async Task<Result<Stream>> DownloadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = filePath
        };

        var response = await s3Client.GetObjectAsync(getRequest, cancellationToken);
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            return StorageErrors.FileNotFound;
        }

        return response.ResponseStream;
    }

    public async Task<Result<Success>> DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = bucketName,
            Key = filePath
        };

        var response = await s3Client.DeleteObjectAsync(deleteRequest, cancellationToken);
        if (response.HttpStatusCode != HttpStatusCode.NoContent)
        {
            return StorageErrors.FailedToDelete;
        }

        return SuccessBuilder.Default;
    }
}
