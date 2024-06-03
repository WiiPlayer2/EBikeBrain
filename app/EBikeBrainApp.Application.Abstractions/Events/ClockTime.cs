using Vogen;

namespace EBikeBrainApp.Application.Abstractions.Events;

[ValueObject<DateTimeOffset>]
public partial record ClockTime;
