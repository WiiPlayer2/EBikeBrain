using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

namespace EBikeBrainApp.Avalonia.XPlat.Views.Controls;

public partial class CardGrid : UserControl
{
    public static readonly StyledProperty<IEnumerable<CardViewModel?>?> CardsProperty =
        AvaloniaProperty.Register<CardGrid, IEnumerable<CardViewModel?>?>(nameof(Cards));

    public CardGrid()
    {
        InitializeComponent();
    }

    public IEnumerable<CardViewModel?>? Cards { get; set; }
}
