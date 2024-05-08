using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;
using EBikeBrainApp.Protocols.Bafang;
using EBikeBrainApp.Utils;
using LanguageExt.UnitsOfMeasure;

namespace EBikeBrainApp.Implementations.Demo;

public class DemoBikeMotorConnector : IBikeMotorConnector
{
    private readonly Busy busy = new();

    public async Task<IBikeMotor> ConnectDevice(DeviceId deviceId, CancellationToken cancellationToken = default)
    {
        await busy.Run(() => Task.Delay(1.Seconds(), cancellationToken), cancellationToken);

        throw new Exception();
        var stream = typeof(DemoBikeMotorConnector).Assembly.GetManifestResourceStream("EBikeBrainApp.Implementations.Demo.demo.log") ?? throw new InvalidOperationException();
        return new ProtocolInterceptorBikeMotor(new SlowStream(stream, 1.Milliseconds()));
    }

    public IObservable<bool> IsBusy => busy.IsBusy;
}
