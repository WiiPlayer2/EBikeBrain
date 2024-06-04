using System;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;

public abstract class CardViewModel
{
    public int Height { get; set; } = 1;

    public int Width { get; set; } = 1;

    public int X { get; set; }

    public int Y { get; set; }

    // HACK bad design, pls do better me
    public CardViewModel With(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        return this;
    }
}
