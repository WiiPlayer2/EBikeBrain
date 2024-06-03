using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

namespace EBikeBrainApp.Avalonia.XPlat.DataTemplates;

public static class CardTemplateProvider
{
    private static readonly Dictionary<Type, Func<Control>> cardCreators = new();

    public static FuncDataTemplate<CardViewModel?> CardDataTemplate { get; } = new(BuildCard);

    public static void AddCard<TCardView, TCardViewModel>()
        where TCardView : Control, new()
        where TCardViewModel : CardViewModel
        => cardCreators.Add(typeof(TCardViewModel), () => new TCardView());

    private static Control? BuildCard(CardViewModel? vm, INameScope nameScope) =>
        vm is null
            ? default
            : cardCreators.TryGetValue(vm.GetType(), out var createCard)
                ? createCard()
                : default;
}
