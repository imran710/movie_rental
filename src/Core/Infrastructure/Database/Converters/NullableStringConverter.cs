using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Database.Converters;

public class NullableStringConverter() : ValueConverter<string, string?>(
    v => string.IsNullOrEmpty(v) ? null : v,
    v => v ?? string.Empty);
