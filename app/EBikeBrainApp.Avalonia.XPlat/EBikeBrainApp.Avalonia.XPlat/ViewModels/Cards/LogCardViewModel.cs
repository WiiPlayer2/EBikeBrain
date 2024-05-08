using System;
using System.Reactive.Linq;
using EBikeBrainApp.Application;
using LanguageExt.Sys.Live;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

public class LogCardViewModel(DisplayService<Runtime> displayService) : CardViewModel
{
    public IObservable<string> Text { get; } = displayService.LogEntries
        .Select(x => string.Join("\n", x.Reverse()));
}
