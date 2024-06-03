using System;
using EBikeBrainApp.Application.Abstractions;

namespace EBikeBrainApp.Avalonia.XPlat;

internal class Clock : IClock
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
