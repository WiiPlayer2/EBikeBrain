using System;
using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain.Events;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

public class PowerCardViewModel(IEventStream<BikeMotorPower> powerStream) : CardViewModel
{
    public IObservable<string> Value => powerStream
        .Select(x => x.Value.Watts.ToString("0.0"))
        .StartWith("---");
}
