using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;
using LanguageExt.UnitsOfMeasure;

namespace EBikeBrainApp.Implementations.Demo;

public class DemoBikeMotorConnector : IBikeMotorConnector
{
    public async Task<IBikeMotor> ConnectDevice(DeviceId deviceId, CancellationToken cancellationToken = default)
    {
        await Task.Delay(1.Seconds(), cancellationToken);
        return new DemoBikeMotor();
    }
}
