using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EBikeBrainApp.Avalonia.XPlat.ViewModels;
using EBikeBrainApp.Avalonia.XPlat.Views;
using EBikeBrainApp.DependencyInjection;
using EBikeBrainApp.Implementations.Demo;
using LanguageExt.Sys.Live;
using Microsoft.Extensions.DependencyInjection;
using AvaloniaApp = Avalonia.Application;

namespace EBikeBrainApp.Avalonia.XPlat;

public class App : AvaloniaApp
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var serviceProvider = BuildServiceProvider();
        serviceProvider.UseProjections();
        var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainViewModel,
                };
                break;

            case ISingleViewApplicationLifetime singleViewPlatform:
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = mainViewModel,
                };
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddAvalonia();
        services.AddPlatformSpecificServices();
        services.AddDemoImplementations();
        services.AddEBikeBrainApp<Runtime>();
        services.AddEventing();

        // services.AddProjection<BikeConfiguration, RotationalSpeed, Speed, SpeedProjection>();

        return services.BuildServiceProvider();
    }
}
