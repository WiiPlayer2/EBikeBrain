using System.Reactive.Linq;
using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

[Obsolete]
public class ConnectionService<RT>(
    IBikeMotorConnector bikeMotorConnector)
    where RT : struct, HasCancel<RT>
{
    public IObservable<bool> CanConnect { get; } = bikeMotorConnector.IsBusy.Select(x => !x);

    public IObservable<bool> CanDisconnect { get; } = Observable.Return(false);
}
