using Vogen;
using Validation = Vogen.Validation;

namespace EBikeBrainApp.Domain;

[ValueObject<Length>]
public readonly partial struct WheelDiameter;

[ValueObject<ElectricPotential>]
public readonly partial struct MotorVoltage;

[ValueObject<double>]
public readonly partial struct Percentage
{
    private static Validation Validate(double input)
    {
        var isValid = input is >= 0 and <= 100;
        return isValid ? Validation.Ok : Validation.Invalid("Value must be between 0-100 inclusively");
    }
}
