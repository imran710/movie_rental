using System.ComponentModel;

namespace Core.Features.Common;

public record LengthRequest(
    [property: Description("Value of the Length")] double Value,
    [property: Description("Unit of Length (Centimeter,Foot,Inch)")] LengthUnitEnum Unit);

public enum LengthUnitEnum
{
    Meter,
    Centimeter,
    Foot,
    Inch,
}

