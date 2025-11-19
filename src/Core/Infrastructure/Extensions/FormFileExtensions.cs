using Microsoft.AspNetCore.Http;

namespace Core.Infrastructure.Extensions;

public static class FormFileExtensions
{
    public static async Task<byte[]> ReadFileAsBytesAsync(this IFormFile file, CancellationToken cancellationToken = default)
    {
        using var stream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();

        await stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}
