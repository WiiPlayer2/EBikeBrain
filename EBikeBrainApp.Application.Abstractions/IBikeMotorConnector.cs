namespace EBikeBrainApp.Application.Abstractions;

public interface IBikeMotorConnector
{
    IObservable<bool> IsBusy { get; }

    Task<IBikeMotor> ConnectDevice(DeviceId deviceId, CancellationToken cancellationToken = default);
}
