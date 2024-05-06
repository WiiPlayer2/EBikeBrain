using EBikeBrainApp.Application.Abstractions.Commands;
using EBikeBrainApp.Application.Abstractions.Events;

namespace EBikeBrainApp.Application;

public class ConnectDeviceHandler(
    IBikeMotorConnector bikeMotorConnector,
    IEventStream<BikeMotorConnected> connectedStream)
    : ICommandHandler<ConnectDevice>
{
    public async Task ExecuteAsync(ConnectDevice command)
    {
        var bikeMotor = await bikeMotorConnector.ConnectDevice(command.Device.Id);
        connectedStream.Publish(new BikeMotorConnected(bikeMotor));
    }
}
