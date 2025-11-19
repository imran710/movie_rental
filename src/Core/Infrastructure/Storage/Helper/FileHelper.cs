using Core.Domain.Extensions;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Core.Infrastructure.Storage.Helper;

public class FileHelper(IWebHostEnvironment webHostEnvironment)
{
    public const string BaseFolder = "data/storage";

    private static readonly Dictionary<SupportedFileType, string[]> SupportedExtensions = new()
    {
        { SupportedFileType.Image, new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" } },
        { SupportedFileType.Document, new[] { ".pdf", ".doc", ".docx", ".txt" } },
        { SupportedFileType.Video, new[] { ".mp4", ".avi", ".mkv", ".mov" } },
        { SupportedFileType.Audio, new[] { ".mp3", ".wav", ".ogg", ".aac" } }
    };

    /// <summary>
    /// Validates if the file extension matches the supported file type.
    /// </summary>
    /// <param name="file">The file to validate.</param>
    /// <param name="fileType">The expected file type.</param>
    /// <returns>True if the file type is valid; otherwise, false.</returns>
    public static bool IsValidFileType(IFormFile file, SupportedFileType fileType)
    {
        if (file == null || file.Length == 0)
            return false;

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return SupportedExtensions[fileType].Contains(fileExtension);
    }

    /// <summary>
    /// Validates if the file extension matches any of the supported file types.
    /// </summary>
    /// <param name="file">The file to validate.</param>
    /// <param name="fileTypes">The expected file types.</param>
    /// <returns>True if the file type is valid; otherwise, false.</returns>
    public static bool IsValidFileType(IFormFile file, params SupportedFileType[] fileTypes)
    {
        if (file == null || file.Length == 0)
            return false;

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return fileTypes.Any(type => SupportedExtensions[type].Contains(fileExtension));
    }

    /// <summary>
    /// Saves a file to the specified subdirectory within the public folder.
    /// </summary>
    /// <param name="file">The file to save.</param>
    /// <param name="subDirectory">The subdirectory to save the file in.</param>
    /// <returns>The relative file path if successful; otherwise, null.</returns>
    public async Task<string?> SaveFileAsync(IFormFile file, string subDirectory)
    {
        if (file == null || file.Length == 0)
            return null;

        // Create the full directory path
        var directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, BaseFolder, subDirectory);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Generate a unique file name
        var fileName = $"{Guid.CreateVersion7()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(directoryPath, fileName);

        // Save the file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return the relative path
        return (Path.Combine(BaseFolder, subDirectory, fileName).Replace("\\", "/")).ToBase64UrlSafeString();
    }

    public (bool IsExists, string FullPath) ExistsFile(string relativeFilePath)
    {
        var filePath = Path.Combine(webHostEnvironment.ContentRootPath, relativeFilePath);
        return (File.Exists(filePath), filePath);
    }
}
