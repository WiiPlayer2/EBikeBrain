using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class DisplayService<RT> where RT : struct, HasCancel<RT>
{
    public IObservable<Option<Aff<RT, Unit>>> ConnectBikeCommand { get; }

    public IObservable<Option<Aff<RT, Unit>>> DecreasePasLevelCommand { get; }

    public IObservable<Option<Aff<RT, Unit>>> DisconnectBikeCommand { get; }

    public IObservable<Option<Aff<RT, Unit>>> IncreasePasLevelCommand { get; }

    public IObservable<Fin<PasLevel>> PasLevel { get; }

    public IObservable<Fin<Speed>> Speed { get; }
}
