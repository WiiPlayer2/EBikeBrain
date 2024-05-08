using System;
using System.Collections.Generic;
using EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels;

public class DemoCardsViewModel(
    SpeedCardViewModel speedCardViewModel,
    ConnectCardViewModel connectCardViewModel,
    LogCardViewModel logCardViewModel)
{
    public IReadOnlyList<CardViewModel?> Cards { get; } =
    [
        speedCardViewModel,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        logCardViewModel,
        null,
        null,
        connectCardViewModel,
    ];
}
