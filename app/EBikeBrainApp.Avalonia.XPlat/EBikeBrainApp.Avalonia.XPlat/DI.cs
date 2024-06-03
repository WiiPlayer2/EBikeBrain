using System;
using Avalonia.Controls;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Avalonia.XPlat.DataTemplates;
using EBikeBrainApp.Avalonia.XPlat.ViewModels;
using EBikeBrainApp.Avalonia.XPlat.ViewModels.Cards;
using EBikeBrainApp.Avalonia.XPlat.Views.Cards;
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

        services.AddCard<ConnectCard, ConnectCardViewModel>();
        services.AddCard<LogCard, LogCardViewModel>();
        services.AddCard<SpeedCard, SpeedCardViewModel>();
        services.AddCard<ClockCard, ClockCardViewModel>();
        services.AddCard<BatteryCard, BatteryCardViewModel>();
        services.AddCard<PasCard, PasCardViewModel>();
        services.AddCard<PowerCard, PowerCardViewModel>();

        services.AddSingleton<IMainThreadDispatcher, AvaloniaMainThreadDispatcher>();

        services.AddSingleton<IClock, Clock>();
    }

    public static void AddPlatformSpecificServices(this IServiceCollection services) =>
        PlatformSpecificLocator.RegisterServices(services);

    private static void AddCard<TCardView, TCardViewModel>(this IServiceCollection services)
        where TCardView : Control, new()
        where TCardViewModel : CardViewModel
    {
        CardTemplateProvider.AddCard<TCardView, TCardViewModel>();
        services.AddSingleton<TCardViewModel>();
    }
}
