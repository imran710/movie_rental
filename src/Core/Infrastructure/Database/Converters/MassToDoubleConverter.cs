using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using UnitsNet;
using UnitsNet.Units;

namespace Core.Infrastructure.Database.Converters;

public class MassToDoubleConverter : ValueConverter<Mass, double>
{
    public MassToDoubleConverter()
        : base(
            v => v.Grams,
            v => new Mass(v, MassUnit.Gram))
    {
    }
}
