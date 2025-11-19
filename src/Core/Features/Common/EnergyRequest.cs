using System.ComponentModel;

namespace Core.Features.Common;

public record EnergyRequest(
    [property: Description("Value of the Energy")] double Value,
    [property: Description("Unit of Mass (Kilocalorie)")] EnergyUnitEnum Unit);

public enum EnergyUnitEnum
{
    Kilocalorie,
    Calorie,
}

