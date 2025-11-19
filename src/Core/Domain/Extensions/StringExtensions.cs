namespace Core.Domain.Extensions;

public static class StringExtensions
{
    public static string? ToBase64UrlSafeString(this string? input)
    {
        if (string.IsNullOrEmpty(input))
            return null;

        // Convert string to bytes
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);

        // Convert bytes to Base64 string
        var base64 = Convert.ToBase64String(bytes);

        // Make the Base64 string URL-safe
        return base64
            .Replace('+', '-') // Replace '+' with '-'
            .Replace('/', '_') // Replace '/' with '_'
            .TrimEnd('=');     // Remove padding '=' characters
    }

    public static string? FromBase64UrlSafeString(this string? base64UrlSafe)
    {
        if (string.IsNullOrEmpty(base64UrlSafe))
            return null;

        // Make the Base64 URL-safe string standard Base64
        var base64 = base64UrlSafe
            .Replace('-', '+') // Replace '-' with '+'
            .Replace('_', '/'); // Replace '_' with '/'

        // Add padding if necessary
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        // Convert Base64 string to bytes
        var bytes = Convert.FromBase64String(base64);

        // Convert bytes to string
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
