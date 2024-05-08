using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;
using EBikeBrainApp.Avalonia.XPlat.Views.Cards;

namespace EBikeBrainApp.Avalonia.XPlat.DataTemplates;

public static class CardTemplateProvider
{
    public static FuncDataTemplate<CardViewModel> CardDataTemplate { get; } = new(BuildCard);

    private static Control? BuildCard(CardViewModel vm, INameScope nameScope) =>
        vm switch
        {
            ConnectCardViewModel => new ConnectCard(),
            LogCardViewModel => new LogCard(),
            SpeedCardViewModel => new SpeedCard(),
            _ => default,
        };
}
