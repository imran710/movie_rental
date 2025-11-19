using DeviceDetectorNET.Results;
using DeviceDetectorNET.Results.Client;

namespace Core.Domain.Common;

public record ClientDeviceInfo
{
    public required ParseResult<ClientMatchResult> ClientInfo { get; set; }
    public required ParseResult<OsMatchResult> Os { get; set; }
    public required string DeviceName { get; set; }
    public required string BrandName { get; set; }
    public required string Model { get; set; }
}

public record CurrentUserInfo
{
    public required long UserId { get; init; }
    public required string Username { get; init; }
}
