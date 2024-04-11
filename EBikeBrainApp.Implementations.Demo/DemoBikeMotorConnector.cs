using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;
using EBikeBrainApp.Utils;
using LanguageExt.UnitsOfMeasure;

namespace EBikeBrainApp.Implementations.Demo;

public class DemoBikeMotorConnector : IBikeMotorConnector
{
    private readonly Busy busy = new();

    public async Task<IBikeMotor> ConnectDevice(DeviceId deviceId, CancellationToken cancellationToken = default)
    {
        await busy.Run(() => Task.Delay(1.Seconds(), cancellationToken), cancellationToken);
        return new DemoBikeMotor();
    }

    public IObservable<bool> IsBusy => busy.IsBusy;
}
