using EBikeBrainApp.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EBikeBrainApp.Implementations.Demo;

public static class DI
{
    public static void AddDemoImplementations(this IServiceCollection services)
    {
        services.TryAddSingleton<IBikeMotorConnector, DemoBikeMotorConnector>();
        services.TryAddSingleton<IDeviceProvider, DemoDeviceProvider>();
        services.TryAddSingleton<IConfigurationStore, DemoConfigurationStore>();
    }
}
