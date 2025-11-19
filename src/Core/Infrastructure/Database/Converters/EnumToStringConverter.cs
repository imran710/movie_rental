using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Infrastructure.Database.Converters;

public class EnumToStringConverter<TEnum> : ValueConverter<TEnum, string> where TEnum : struct, Enum
{
  public EnumToStringConverter()
      : base(
          v => v.ToString(),
          v => Enum.Parse<TEnum>(v))
  {
  }
}
