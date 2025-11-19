namespace Core.Domain.Parser;

public static class IncludeRequestParser
{
    public static TEnum[] ParseIncludeString<TEnum>(this string? include) where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(include))
        {
            return [];
        }

        var includes = include.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var result = new List<TEnum>();

        foreach (var item in includes)
        {
            if (!Enum.TryParse<TEnum>(item, out var enumValue))
            {
                throw new ArgumentException($"Invalid include value: {item}. Valid values are: {string.Join(", ", Enum.GetNames<TEnum>())}");
            }
            result.Add(enumValue);
        }

        return [.. result];
    }
}

