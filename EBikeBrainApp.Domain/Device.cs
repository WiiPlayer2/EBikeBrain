using Vogen;

namespace EBikeBrainApp.Domain;

public record Device(
    string Name,
    DeviceId Id
);

[ValueObject<string>]
public readonly partial struct DeviceId;
