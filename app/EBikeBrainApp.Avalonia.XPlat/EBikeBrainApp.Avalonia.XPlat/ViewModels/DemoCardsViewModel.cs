using System;
using System.Collections.Generic;
using EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels;

public class DemoCardsViewModel(
    SpeedCardViewModel speedCardViewModel,
    ConnectCardViewModel connectCardViewModel,
    LogCardViewModel logCardViewModel,
    ClockCardViewModel clockCardViewModel,
    BatteryCardViewModel batteryCardViewModel,
    PasCardViewModel pasCardViewModel,
    PowerCardViewModel powerCardViewModel)
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
        batteryCardViewModel,
        pasCardViewModel,
        powerCardViewModel,
        clockCardViewModel,
        logCardViewModel,
        null,
        null,
        connectCardViewModel,
    ];
}
