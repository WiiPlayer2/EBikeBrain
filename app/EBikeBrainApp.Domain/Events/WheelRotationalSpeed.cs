using Vogen;

namespace EBikeBrainApp.Domain.Events;

[ValueObject<RotationalSpeed>]
public partial record WheelRotationalSpeed;

[ValueObject<Speed>]
public partial record BikeSpeed;
