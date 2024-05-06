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
        services.AddSingleton<IEventBus>(sp => sp.GetRequiredService<EventBus>());
        services.AddSingleton(typeof(IEventStream<>), typeof(EventStream<>));
        services.AddSingleton(typeof(ICommandPublisher<>), typeof(CommandPublisher<>));
    }

    public static void AddProjection<T1, T2, TOut, TProjection>(this IServiceCollection services)
        where T1 : notnull
        where T2 : notnull
        where TOut : notnull
        where TProjection : IEventProjection<T1, T2, TOut> =>
        services.AddSingleton<IEventProjector, EventProjector<T1, T2, TOut, TProjection>>();

    public static void UseProjections(this IServiceProvider services) =>
        services.GetServices<IEventProjector>();
}
