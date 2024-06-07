using System;
using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain.Events;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

public class DistanceCardViewModel(IEventStream<BikeDistanceCycled> distanceCycledStream) : CardViewModel
{
    public IObservable<string> Value => distanceCycledStream
        .Select(x => $"{x.Value.Kilometers:0.00}")
        .StartWith("---");
}
