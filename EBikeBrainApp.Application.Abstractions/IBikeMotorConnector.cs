namespace EBikeBrainApp.Application.Abstractions;

public interface IBikeMotorConnector
{
    Task<IBikeMotor> ConnectDevice(DeviceId deviceId, CancellationToken cancellationToken = default);
}
