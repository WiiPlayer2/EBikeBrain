using EBikeBrain.Implementations.Eventing;
using EBikeBrainApp.Application;
using EBikeBrainApp.Application.Abstractions;
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
        services.AddSingleton<ConfigurationService>();
        services.AddSingleton<LogService>();
    }

    public static void AddEventing(this IServiceCollection services)
    {
        services.AddSingleton<EventBus>();
        services.AddSingleton(typeof(IEventStream<>), typeof(EventStream<>));
        services.AddSingleton(typeof(ICommandPublisher<>), typeof(CommandPublisher<>));
    }
}
