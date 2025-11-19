using System.Runtime.CompilerServices;

namespace Library.Values;

public readonly record struct Value<T>(T Data, string Property)
{
    public T Data { get; init; } = Data;
    public string Property { get; init; } = Property;
}

public record Value
{
    public static Value<T> New<T>(T data, [CallerArgumentExpression(nameof(data))] string propertyName = "")
    {
        return new Value<T>(data, RemoveFirstSegment(propertyName));
    }

    private static string RemoveFirstSegment(string input)
    {
        var firstIndex = input.IndexOf('.');
        if (firstIndex == -1)
            return input;

        return input[(firstIndex + 1)..];
    }
}
