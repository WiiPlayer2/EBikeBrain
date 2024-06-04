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
        speedCardViewModel.With(0, 0, 4, 2),
        batteryCardViewModel.With(0, 4, 1, 1),
        pasCardViewModel.With(1, 4, 1, 1),
        powerCardViewModel.With(2, 4, 1, 1),
        clockCardViewModel.With(3, 4, 1, 1),
        logCardViewModel.With(0, 5, 3, 1),
        connectCardViewModel.With(3, 5, 1, 1),
    ];
}
