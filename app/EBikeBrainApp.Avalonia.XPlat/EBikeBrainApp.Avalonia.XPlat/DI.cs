using System;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Avalonia.XPlat.ViewModels;
using EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;
using Microsoft.Extensions.DependencyInjection;

namespace EBikeBrainApp.Avalonia.XPlat;

internal static class DI
{
    public static void AddAvalonia(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<DisplayViewModel>();
        services.AddSingleton<SettingsViewModel>();

        services.AddSingleton<DemoCardsViewModel>();

        services.AddSingleton<ConnectCardViewModel>();
        services.AddSingleton<LogCardViewModel>();
        services.AddSingleton<SpeedCardViewModel>();
        services.AddSingleton<ClockCardViewModel>();

        services.AddSingleton<IMainThreadDispatcher, AvaloniaMainThreadDispatcher>();

        services.AddSingleton<IClock, Clock>();
    }

    public static void AddPlatformSpecificServices(this IServiceCollection services) =>
        PlatformSpecificLocator.RegisterServices(services);
}
