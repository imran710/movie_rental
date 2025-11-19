using Core.Domain.Common;

using DeviceDetectorNET;
using DeviceDetectorNET.Cache;

using Microsoft.AspNetCore.Http;

using System.Runtime.CompilerServices;

namespace Core.Infrastructure.Extensions;

public static partial class UserAgentExtensions
{
    private static readonly DictionaryCache DictionaryCache = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)] // TODO: Check if this is the best way to do this
    public static ClientDeviceInfo GetDeviceInformation(this HttpContext httpContext)
    {
        // Get user agent once to avoid multiple header lookups
        var userAgent = httpContext.Request.Headers.UserAgent.ToString();

        // Initialize and configure detector in one go
        var deviceDetector = new DeviceDetector(userAgent);
        deviceDetector.SetCache(DictionaryCache);
        deviceDetector.SkipBotDetection();
        deviceDetector.Parse();

        // Get all info at once to avoid multiple internal parsing
        return new ClientDeviceInfo
        {
            ClientInfo = deviceDetector.GetClient(),
            Os = deviceDetector.GetOs(),
            DeviceName = deviceDetector.GetDeviceName(),
            BrandName = deviceDetector.GetBrandName(),
            Model = deviceDetector.GetModel(),
        };
    }
}
