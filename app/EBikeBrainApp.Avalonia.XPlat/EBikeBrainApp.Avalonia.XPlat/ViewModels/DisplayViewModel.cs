using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using EBikeBrainApp.Application;
using LanguageExt.Sys.Live;
using Reactive.Bindings;
using Pas = EBikeBrainApp.Domain.PasLevel;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels;

public class DisplayViewModel : ViewModelBase, IDisposable
{
    private readonly CompositeDisposable subscriptions;

    public DisplayViewModel(DisplayService<Runtime> displayService)
    {
        Speed = displayService.Speed
            .Select(x => x.Value.KilometersPerHour.ToString("0.0"))
            .StartWith("---");
        PasLevel = displayService.PasLevel.StartWith(None)
            .Select(x => x.Match(x => x switch
            {
                Pas.Level0 => "PAS 0",
                Pas.Level1 => "PAS 1",
                Pas.Level2 => "PAS 2",
                Pas.Level3 => "PAS 3",
                Pas.Level4 => "PAS 4",
                Pas.Level5 => "PAS 5",
                Pas.Level6 => "PAS 6",
                Pas.Level7 => "PAS 7",
                Pas.Level8 => "PAS 8",
                Pas.Level9 => "PAS 9",
                Pas.Unknown => "PAS ?",
                _ => x.ToString(),
            }, () => "PAS -"));
        RotationsPerMinute = displayService.RotationalSpeed
            .Select(x => x.Value.RevolutionsPerMinute.ToString("0"))
            .StartWith("---");
        Power = displayService.Power
            .Select(x => x.Watts.ToString("0.0"))
            .StartWith("---");
        Battery = displayService.Battery
            .Select(x => $"{x}%")
            .StartWith("---");
        LogOutput = displayService.LogEntries
            .Select(x => string.Join("\n", x.Reverse()));

        ConnectCommand = new ReactiveCommand<object?>(displayService.CanConnectBike);
        DisconnectCommand = new ReactiveCommand<object?>(displayService.CanDisconnectBike);

        subscriptions = new CompositeDisposable(
            ConnectCommand.Subscribe(_ => displayService.Connect()));
    }

    public IObservable<string> Battery { get; }

    public ReactiveCommand<object?> ConnectCommand { get; }

    public ReactiveCommand<object?> DisconnectCommand { get; }

    public IObservable<string> LogOutput { get; }

    public IObservable<string> PasLevel { get; }

    public IObservable<string> Power { get; }

    public IObservable<string> RotationsPerMinute { get; }

    public IObservable<string> Speed { get; }

    public void Dispose()
    {
        subscriptions.Dispose();
    }
}
