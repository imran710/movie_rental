using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using UnitsNet;
using UnitsNet.Units;

namespace Core.Infrastructure.Database.Converters;

public class EnergyToDoubleConverter : ValueConverter<Energy?, double?>
{
    public EnergyToDoubleConverter()
        : base(
            v => v.HasValue ? v.Value.Calories : null,
            v => v.HasValue ? new Energy(v.Value, EnergyUnit.Calorie) : null)
    {
    }
}
