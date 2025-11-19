using System.Net.Mime;

using Api.Presentation.Common;

using Core.Domain.Extensions;
using Core.Infrastructure.Storage.Helper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Api.Controllers;

public class FileApi : IEndpoint
{
    private const string ContentDisposition = "Content-Disposition";
    private const string OctetStream = "application/octet-stream";
    private static readonly FileExtensionContentTypeProvider Provider = new();

    public void MapRoutes(IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/file").WithTags("File").AllowAnonymous();

        group
            .MapGet("/get/{url}", GetFile)
            .WithSummary("Get file");
    }

    private static IResult GetFile(
        [FromRoute] string url,
        [FromQuery] bool download,
        [FromQuery] string? name,
        [FromServices] FileHelper fileHelper,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var u = url.FromBase64UrlSafeString();
            if (string.IsNullOrEmpty(u))
                return Results.BadRequest("Invalid url");

            var (isExists, fullFullPath) = fileHelper.ExistsFile(u);
            if (!isExists)
                return Results.NotFound();

            var extension = Path.GetExtension(fullFullPath);
            var filename = Path.GetFileName(fullFullPath);
            Provider.TryGetContentType(extension, out string? contentType);

            contentType = download
                ? OctetStream
                : string.IsNullOrEmpty(contentType) ? OctetStream : contentType;

            var contentDisposition = new ContentDisposition
            {
                FileName = name ?? filename,
                Inline = download == false
            };
            httpContext.Response.Headers.Append(ContentDisposition, contentDisposition.ToString());

            var fileInfo = new FileInfo(fullFullPath);
            var fileLength = fileInfo.Length;

            if (httpContext.Request.Method == HttpMethod.Head.Method)
            {
                httpContext.Response.Headers.Append("Accept-Ranges", "bytes");
                httpContext.Response.Headers.Append("Content-Length", fileLength.ToString());
                return Results.Ok();
            }

            var rangeHeader = httpContext.Request.Headers.Range.FirstOrDefault();

            if (!string.IsNullOrEmpty(rangeHeader))
            {
                // Parse range header
                var range = rangeHeader.Replace("bytes=", "").Split('-');

                // Check if format is valid
                if (range.Length != 2)
                {
                    return Results.BadRequest("Invalid range format");
                }

                // Parse start position
                if (!long.TryParse(range[0], out long start))
                {
                    return Results.BadRequest("Invalid start position");
                }

                // Parse end position - if empty, set to end of file
                long end;
                if (string.IsNullOrEmpty(range[1]))
                {
                    end = fileLength - 1;
                }
                else if (!long.TryParse(range[1], out end))
                {
                    return Results.BadRequest("Invalid end position");
                }

                // Ensure end isn't beyond file length
                if (end >= fileLength)
                {
                    end = fileLength - 1;
                }

                // Validate range
                if (start < 0 || start >= fileLength || start > end)
                {
                    httpContext.Response.Headers.Append("Content-Range", $"bytes */{fileLength}");
                    return Results.StatusCode(416); // Range Not Satisfiable
                }

                // Calculate length of range
                long rangeLength = end - start + 1;

                // Set response headers for partial content
                httpContext.Response.StatusCode = 206; // Partial Content
                httpContext.Response.Headers.Append("Accept-Ranges", "bytes");
                httpContext.Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{fileLength}");
                httpContext.Response.Headers.Append("Content-Length", rangeLength.ToString());

                // Open file stream with range
                var fileStream = File.OpenRead(fullFullPath);
                fileStream.Seek(start, SeekOrigin.Begin);

                return Results.File(fileStream, contentType, enableRangeProcessing: true);
            }

            // Full file download
            httpContext.Response.Headers.Append("Accept-Ranges", "bytes");
            httpContext.Response.Headers.Append("Content-Length", fileLength.ToString());

            var fullFileStream = File.OpenRead(fullFullPath);
            return Results.File(fullFileStream, contentType);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
