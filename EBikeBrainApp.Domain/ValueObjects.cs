using Vogen;

namespace EBikeBrainApp.Domain;

[ValueObject<Length>]
public readonly partial struct WheelDiameter;

[ValueObject<ElectricPotential>]
public readonly partial struct MotorVoltage;
