using System;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Avalonia.XPlat.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace EBikeBrainApp.Avalonia.XPlat;

internal static class DI
{
    public static void AddAvalonia(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<DisplayViewModel>();
        services.AddSingleton<SettingsViewModel>();

        services.AddSingleton<IMainThreadDispatcher, AvaloniaMainThreadDispatcher>();
    }

    public static void AddPlatformSpecificServices(this IServiceCollection services) =>
        PlatformSpecificLocator.RegisterServices(services);
}
