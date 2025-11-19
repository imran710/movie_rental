using System.Text;

namespace Core.Infrastructure.Extensions;

public static class EnumExtensions
{
    public static string ToAttributeString<T>(string? name = null) where T : Enum
    {
        var type = typeof(T);
        var values = Enum.GetValues(type);

        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"[{name ?? type.Name}(");

        var enumValues = values.Cast<T>()
            .Select(value => $"{value}={(int)(object)value}")
            .ToList();

        stringBuilder.Append(string.Join(",", enumValues));
        stringBuilder.Append(")]");

        return stringBuilder.ToString();
    }
}
