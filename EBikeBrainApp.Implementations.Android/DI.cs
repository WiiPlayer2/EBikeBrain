using EBikeBrainApp.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EBikeBrainApp.Implementations.Android;

public static class DI
{
    public static void AddAndroidServices(this IServiceCollection services)
    {
        services.AddSingleton<IDeviceProvider, AndroidDeviceProvider>();
    }
}
