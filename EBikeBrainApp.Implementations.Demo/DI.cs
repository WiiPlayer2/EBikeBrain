using EBikeBrainApp.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EBikeBrainApp.Implementations.Demo;

public static class DI
{
    public static void AddDemoImplementations(this IServiceCollection services)
    {
        services.AddSingleton<IBikeMotor, DemoBikeMotor>();
    }
}
