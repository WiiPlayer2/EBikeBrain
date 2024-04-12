using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;

namespace EBikeBrainApp.Implementations.Android;

public class AndroidBikeMotorConnector : IBikeMotorConnector
{
    public Task<IBikeMotor> ConnectDevice(DeviceId deviceId, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public IObservable<bool> IsBusy { get; }
}
