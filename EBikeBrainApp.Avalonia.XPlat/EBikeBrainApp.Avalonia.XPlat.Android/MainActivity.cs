using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using EBikeBrainApp.Implementations.Android;
using Microsoft.Maui.ApplicationModel;

namespace EBikeBrainApp.Avalonia.XPlat.Android;

[Activity(
    Label = "EBikeBrainApp.Avalonia.XPlat.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        PlatformSpecificLocator.RegisterServices = DI.AddAndroidServices;

        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        ActivityStateManager.Default.Init(Application!);
    }
}
