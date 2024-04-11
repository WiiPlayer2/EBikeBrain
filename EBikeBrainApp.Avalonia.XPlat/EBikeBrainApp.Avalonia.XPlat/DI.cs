using System;
using EBikeBrainApp.Avalonia.XPlat.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace EBikeBrainApp.Avalonia.XPlat;

internal static class DI
{
    public static void AddAvalonia(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<DisplayViewModel>();
    }
}
