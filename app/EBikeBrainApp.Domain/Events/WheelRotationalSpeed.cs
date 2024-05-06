using Vogen;

namespace EBikeBrainApp.Domain.Events;

[ValueObject<RotationalSpeed>]
public partial record WheelRotationalSpeed;

[ValueObject<Speed>]
public partial record BikeSpeed;

[ValueObject<ElectricCurrent>]
public partial record BikeMotorCurrent;

[ValueObject<Percentage>]
public partial record BikeMotorBatteryPercentage;

[ValueObject<PasLevel>]
public partial record BikeMotorPasLevel;

[ValueObject<Power>]
public partial record BikeMotorPower;
