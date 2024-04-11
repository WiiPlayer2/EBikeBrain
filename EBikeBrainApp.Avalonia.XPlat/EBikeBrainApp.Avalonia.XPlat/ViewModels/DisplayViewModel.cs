using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using EBikeBrainApp.Application;
using LanguageExt.Sys.Live;
using Reactive.Bindings;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels;

public class DisplayViewModel : ViewModelBase, IDisposable
{
    private readonly CompositeDisposable subscriptions;

    public DisplayViewModel(DisplayService<Runtime> displayService)
    {
        Speed = displayService.Speed
            .Select(x => x.Match(x => x.KilometersPerHour.ToString("0.0"), "---"))
            .ToReadOnlyReactiveProperty();

        ConnectCommand = new ReactiveCommand<object?>(displayService.CanConnectBike);
        DisconnectCommand = new ReactiveCommand<object?>(displayService.CanDisconnectBike);

        subscriptions = new CompositeDisposable(
            ConnectCommand.Subscribe(_ => displayService.Connect()));
    }

    public ReactiveCommand<object?> ConnectCommand { get; }

    public ReactiveCommand<object?> DisconnectCommand { get; }

    public ReadOnlyReactiveProperty<string> Speed { get; }

    public void Dispose()
    {
        subscriptions.Dispose();
    }
}
