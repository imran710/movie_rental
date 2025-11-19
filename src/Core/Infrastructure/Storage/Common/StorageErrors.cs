using Library.Results;

namespace Core.Infrastructure.Storage.Common;

public static class StorageErrors
{
    public static readonly Error FileNotFound = Error.Unexpected(
        "The requested file could not be found. Please check the file path and try again",
        code: "Error.Storage.FileNotFound");
    public static readonly Error FailedToDelete = Error.Unexpected(
        "Unable to delete the file. Ensure the file exists and try again",
        code: "Error.Storage.FailedToDelete");
}
