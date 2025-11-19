using System.Net;
using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Core.Infrastructure.Extensions;

public static class ClientIpAddressExtensions
{
    // Predefined header names as static readonly for performance
    private static readonly string[] ProxyHeaders =
    [
        "X-Forwarded-For",     // Standard proxy header
        "X-Real-IP",           // Nginx proxy
        "CF-Connecting-IP",    // Cloudflare
        "X-Azure-ClientIP",    // Azure
        "True-Client-IP"       // Additional proxy header
    ];

    /// <summary>
    /// Efficiently retrieves the client IP address
    /// </summary>
    public static IPAddress GetClientIpAddress(
        this HttpContext context,
        bool allowPrivateIPs = false)
    {
        // First, check proxy headers
        foreach (var headerName in ProxyHeaders)
        {
            var headerValue = context.Request.Headers[headerName];
            var ip = TryExtractIpFromHeader(headerValue, allowPrivateIPs);

            if (ip is not null)
                return ip;
        }

        // Fallback to RemoteIpAddress
        return context.Connection.RemoteIpAddress ?? throw new Exception("Can not get client ip address");
    }

    /// <summary>
    /// Efficiently extracts and validates IP from header
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPAddress? TryExtractIpFromHeader(
        StringValues headerValue,
        bool allowPrivateIPs)
    {
        // Quick null/empty check
        if (headerValue.Count == 0)
            return null;

        // Get first non-empty value
        ReadOnlySpan<char> ipSpan = headerValue.First().AsSpan().Trim();

        // Handle comma-separated multiple IPs (take first)
        var firstComma = ipSpan.IndexOf(',');
        if (firstComma > 0)
            ipSpan = ipSpan[..firstComma].Trim();

        // Validate IP
        if (IPAddress.TryParse(ipSpan, out var parsedIp))
        {
            // Check for private/localhost IPs if not allowed
            if (!allowPrivateIPs && IsPrivateOrLocalhost(parsedIp))
                return null;

            return parsedIp;
        }

        return null;
    }

    /// <summary>
    /// Efficiently checks if IP is private or localhost
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsPrivateOrLocalhost(IPAddress ip)
    {
        // Use span-based comparison for performance
        Span<byte> ipBytes = stackalloc byte[16];
        ip.TryWriteBytes(ipBytes, out _);

        // IPv4 private ranges
        return ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
            ? IsPrivateIPv4(ipBytes)
            : IsPrivateIPv6(ipBytes);
    }

    /// <summary>
    /// Checks IPv4 private ranges
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsPrivateIPv4(ReadOnlySpan<byte> ipBytes)
    {
        // 10.0.0.0/8
        if (ipBytes[0] == 10)
            return true;

        // 172.16.0.0/12
        if (ipBytes[0] == 172 && ipBytes[1] >= 16 && ipBytes[1] <= 31)
            return true;

        // 192.168.0.0/16
        if (ipBytes[0] == 192 && ipBytes[1] == 168)
            return true;

        // 127.0.0.0/8 (localhost)
        if (ipBytes[0] == 127)
            return true;

        return false;
    }

    /// <summary>
    /// Checks IPv6 private ranges
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsPrivateIPv6(ReadOnlySpan<byte> ipBytes)
    {
        // ::1/128 (localhost)
        if (ipBytes[15] == 1 &&
            ipBytes[0..15].SequenceEqual(stackalloc byte[15]))
            return true;

        // fc00::/7 (unique local address)
        if ((ipBytes[0] & 0xFE) == 0xFC)
            return true;

        return false;
    }
}
