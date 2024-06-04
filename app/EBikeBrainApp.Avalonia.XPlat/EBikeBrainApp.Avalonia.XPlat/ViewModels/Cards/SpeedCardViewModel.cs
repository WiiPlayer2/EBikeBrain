using System;
using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain.Events;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

public class SpeedCardViewModel(IEventStream<BikeSpeed> stream) : CardViewModel
{
    public IObservable<string> Value { get; } = stream
        .Select(x => x.Value.KilometersPerHour.ToString("0.0"))
        .StartWith("---");
}
