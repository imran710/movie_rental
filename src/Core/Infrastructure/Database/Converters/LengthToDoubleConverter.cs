using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using UnitsNet;

namespace Core.Infrastructure.Database.Converters;

public class LengthToDoubleConverter : ValueConverter<Length, double>
{
    public LengthToDoubleConverter()
        : base(
            v => v.Meters,
            v => new Length(v, UnitsNet.Units.LengthUnit.Meter))
    {
    }
}
