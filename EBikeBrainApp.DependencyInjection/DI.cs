using EBikeBrainApp.Application;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;

namespace EBikeBrainApp.DependencyInjection;

public static class DI
{
    public static void AddEBikeBrainApp<RT>(this IServiceCollection services)
        where RT : struct, HasCancel<RT>
    {
        services.AddSingleton<DisplayService<RT>>();
        services.AddSingleton<ConnectionService<RT>>();
    }
}
