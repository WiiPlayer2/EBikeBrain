using System;
using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Application.Abstractions.Events;
using ObservableExtensions = Reactive.Bindings.TinyLinq.ObservableExtensions;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

public class ClockCardViewModel(IEventStream<ClockTime> clockTime) : CardViewModel
{
    public IObservable<string> Time => ObservableExtensions.Select(clockTime, x => $"{x.Value:HH:mm}")
        .StartWith("--:--");
}
