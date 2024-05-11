using EBikeBrainApp.Application.Abstractions.Commands;
using EBikeBrainApp.Application.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace EBikeBrainApp.Application.Commanding;

public class ConnectDeviceHandler(
    IBikeMotorConnector bikeMotorConnector,
    IEventStream<BikeMotorConnected> connectedStream,
    ILogger<ConnectDeviceHandler> logger)
    : ICommandHandler<ConnectDevice>
{
    public async Task ExecuteAsync(ConnectDevice command)
    {
        logger.LogInformation("Connecting to {device} ({deviceId})...", command.Device.Name, command.Device.Id);
        var bikeMotor = await bikeMotorConnector.ConnectDevice(command.Device.Id);
        logger.LogInformation("Connected to {device} ({deviceId}).", command.Device.Name, command.Device.Id);
        connectedStream.Publish(new BikeMotorConnected(bikeMotor));
    }
}
