using System.Diagnostics.Contracts;
using UnitsNet;

namespace EBikeBrainApp.Domain;

public static class Calculations
{
    [Pure]
    public static Speed ToLinearSpeed(this RotationalSpeed rotationalSpeed, Length wheelDiameter) =>
        Speed.FromMetersPerSecond(wheelDiameter.Meters * (Math.PI / 60.0) * rotationalSpeed.RevolutionsPerMinute);
}
