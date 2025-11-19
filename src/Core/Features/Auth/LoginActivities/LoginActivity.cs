using System.Net;

using Core.Domain.Entity;
using Core.Domain.Helper;
using Core.Features.Auth.DeviceInformations;
using Core.Features.Users.Entity;

namespace Core.Features.Auth.LoginActivities;

public class LoginActivity : IEntity
{
    public long Id { get; }

    public long UserId { get; init; }
    public User? User { get; init; }

    public long DeviceInfoId { get; init; }
    public DeviceInformation? DeviceInfo { get; init; }

    public DateTimeOffset LoginTime { get; init; }
    public DateTimeOffset? LogoutTime { get; init; }

    public required IPAddress IPAddress { get; init; }

    public static LoginActivity Create(
        long userId,
        IPAddress ipAddress,
        DeviceInformation? deviceInformation)
    {
        return new LoginActivity
        {
            UserId = userId,
            DeviceInfo = deviceInformation,
            IPAddress = ipAddress,
            LoginTime = DateTimeHelperStatic.UtcNow,
        };
    }
}
