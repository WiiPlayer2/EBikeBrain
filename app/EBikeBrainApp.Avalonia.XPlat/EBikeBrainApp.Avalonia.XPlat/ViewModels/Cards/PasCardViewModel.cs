using System;
using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain.Events;
using Pas = EBikeBrainApp.Domain.PasLevel;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

public class PasCardViewModel(IEventStream<BikeMotorPasLevel> pasLevelStream) : CardViewModel
{
    public IObservable<string> Value => pasLevelStream
        .Select(x => x.Value switch
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
        })
        .StartWith("PAS -");
}
