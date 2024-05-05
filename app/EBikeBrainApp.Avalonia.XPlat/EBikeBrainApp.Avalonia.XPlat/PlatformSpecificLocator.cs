using System;

namespace EBikeBrainApp.Avalonia.XPlat;

public static class PlatformSpecificLocator
{
    public static PlatformSpecificServiceRegistrationDelegate RegisterServices { get; set; } = _ => { };
}
