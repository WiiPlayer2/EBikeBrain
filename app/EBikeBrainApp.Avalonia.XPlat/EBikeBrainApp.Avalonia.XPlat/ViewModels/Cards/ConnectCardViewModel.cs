using System;
using System.Reactive.Disposables;
using EBikeBrainApp.Application;
using LanguageExt.Sys.Live;
using Reactive.Bindings;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

public class ConnectCardViewModel : CardViewModel, IDisposable
{
    private readonly CompositeDisposable subscriptions;

    public ConnectCardViewModel(DisplayService<Runtime> displayService)
    {
        ConnectCommand = new ReactiveCommand<object?>(displayService.CanConnectBike);

        subscriptions = new CompositeDisposable(
            ConnectCommand.Subscribe(_ => displayService.Connect()));
    }

    public ReactiveCommand<object?> ConnectCommand { get; }

    public void Dispose()
    {
        subscriptions.Dispose();
    }
}
