﻿using System;
using Avalonia;
using Avalonia.ReactiveUI;
using EBikeBrainApp.Implementations.Demo;

namespace EBikeBrainApp.Avalonia.XPlat.Desktop;

internal sealed class Program
{
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        PlatformSpecificLocator.RegisterServices = DI.AddDemoImplementations;

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }
}
