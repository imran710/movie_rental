using Core.Domain.Common;
using Core.Domain.Entity;
using Core.Features.Auth.LoginActivities;

namespace Core.Features.Auth.DeviceInformations;

public class DeviceInformation : IEntity
{
    public long Id { get; }
    public long UserId { get; private set; }

    public string? FcmToken { get; private set; }

    public string? ClientType { get; private set; }
    public string? ClientName { get; private set; }
    public string? ClientVersion { get; private set; }

    public string? OsName { get; private set; }
    public string? DeviceName { get; private set; }
    public string? DeviceBrand { get; private set; }
    public string? DeviceModel { get; private set; }

    public List<LoginActivity> LoginActivities { get; } = [];

    public static DeviceInformation Create(string? fcmToken,long UserId, ClientDeviceInfo? clientDeviceInfo)
    {
        return new DeviceInformation
        {
            FcmToken = fcmToken,
            ClientType = clientDeviceInfo?.ClientInfo?.Match?.Type,
            ClientName = clientDeviceInfo?.ClientInfo?.Match?.Name,
            ClientVersion = clientDeviceInfo?.ClientInfo?.Match?.Version,
            OsName = clientDeviceInfo?.Os?.Match?.Name,
            DeviceName = clientDeviceInfo?.DeviceName,
            DeviceBrand = clientDeviceInfo?.BrandName,
            DeviceModel = clientDeviceInfo?.Model,
            UserId = UserId
        };
    }
}
