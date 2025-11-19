using System.ComponentModel;

namespace Core.Features.Common;

public record MassRequest(
    [property: Description("Value of the Mass")] double Value,
    [property: Description("Unit of Mass (Kilogram,Pound,Gram)")] MassUnitEnum Unit);

public enum MassUnitEnum
{
    Kilogram,
    Pound,
    Gram,
}

