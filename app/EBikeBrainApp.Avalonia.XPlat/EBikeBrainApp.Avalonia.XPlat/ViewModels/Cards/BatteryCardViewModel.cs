using System;
using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain.Events;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

public class BatteryCardViewModel(IEventStream<BikeMotorBatteryPercentage> batteryStream) : CardViewModel
{
    public IObservable<string> Value => batteryStream
        .Select(x => $"{x}%")
        .StartWith("--%");
}
