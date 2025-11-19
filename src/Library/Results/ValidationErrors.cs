using System.Collections;
using System.Diagnostics.CodeAnalysis;

using Library.Values;

namespace Library.Results;

public class ValidationErrors : IReadOnlyDictionary<string, ValidationError[]>
{
    private readonly Dictionary<string, ValidationError[]> _errors;

    public ValidationErrors()
    {
        _errors = [];
    }

    public ValidationErrors(IDictionary<string, ValidationError[]> errors)
    {
        _errors = new Dictionary<string, ValidationError[]>(errors ?? throw new ArgumentNullException(nameof(errors)));
    }

    public void Add<T>(Value<T> value, params ValidationError[] errors)
    {
        ArgumentException.ThrowIfNullOrEmpty(value.Property);

        if (!_errors.TryGetValue(value.Property, out var existingErrors))
        {
            _errors[value.Property] = errors;
            return;
        }

        _errors[value.Property] = [.. existingErrors, .. errors];
    }

    public void Add(string property, params ValidationError[] errors)
    {
        ArgumentException.ThrowIfNullOrEmpty(property);

        if (!_errors.TryGetValue(property, out var existingErrors))
        {
            _errors[property] = errors;
            return;
        }

        _errors[property] = [.. existingErrors, .. errors];
    }

    public ValidationErrors Merge(ValidationErrors otherErrors)
    {
        if (otherErrors == null) return this;

        var mergedErrors = new ValidationErrors(new Dictionary<string, ValidationError[]>(_errors));

        foreach (var kvp in otherErrors)
        {
            mergedErrors.Add(kvp.Key, kvp.Value);
        }

        return mergedErrors;
    }

    public void Clear(string propertyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName);

        _errors.Remove(propertyName);
    }

    public bool HasErrors => _errors.Count > 0;
    public int ErrorCount => _errors.Values.Sum(errors => errors.Length);

    public ValidationError[] this[string key] => _errors[key];

    public IEnumerable<string> Keys => _errors.Keys;

    public IEnumerable<ValidationError[]> Values => _errors.Values;

    public int Count => _errors.Count;

    public bool ContainsKey(string key) =>_errors.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, ValidationError[]>> GetEnumerator() => _errors.GetEnumerator();

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out ValidationError[] value) => _errors.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
