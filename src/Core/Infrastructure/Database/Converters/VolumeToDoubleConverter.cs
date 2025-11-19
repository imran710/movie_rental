using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using UnitsNet;
using UnitsNet.Units;

namespace Core.Infrastructure.Database.Converters;

public class VolumeToDoubleConverter : ValueConverter<Volume, double>
{
    public VolumeToDoubleConverter()
        : base(
            v => v.Liters,
            v => new Volume(v, VolumeUnit.Liter))
    {
    }
}
