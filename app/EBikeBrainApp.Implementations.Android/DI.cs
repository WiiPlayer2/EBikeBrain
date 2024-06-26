using Android.Bluetooth;
using EBikeBrainApp.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EBikeBrainApp.Implementations.Android;

public static class DI
{
    public static void AddAndroidServices(this IServiceCollection services)
    {
        services.AddSingleton(BluetoothAdapter.DefaultAdapter ?? throw new InvalidOperationException("Bluetooth is required."));
        services.AddSingleton<IDeviceProvider, AndroidDeviceProvider>();
        services.AddSingleton<IBikeMotorConnector, AndroidBikeMotorConnector>();
        services.AddSingleton(typeof(IConfigurationStore<>), typeof(AndroidConfigurationStore<>));
    }
}
