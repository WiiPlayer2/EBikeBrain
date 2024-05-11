using EBikeBrain.Implementations.Eventing;
using EBikeBrainApp.Application;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Application.Abstractions.Commands;
using EBikeBrainApp.Application.Commanding;
using EBikeBrainApp.Application.Eventing;
using LanguageExt.Effects.Traits;
using Microsoft.Extensions.DependencyInjection;

namespace EBikeBrainApp.DependencyInjection;

public static class DI
{
    public static void AddCommandHandler<TCommand, THandler>(this IServiceCollection services)
        where THandler : class, ICommandHandler<TCommand> =>
        services.AddSingleton<ICommandHandler<TCommand>, THandler>();

    public static void AddEBikeBrainApp<RT>(this IServiceCollection services)
        where RT : struct, HasCancel<RT>
    {
        services.AddSingleton<DisplayService<RT>>();
        services.AddSingleton<ConnectionService<RT>>();
        services.AddSingleton<ConfigurationService>();
        services.AddSingleton<LogService>();

        services.AddCommandHandler<ConnectDevice, ConnectDeviceHandler>();

        services.AddEventBusInitializer<BikeMotorBusInitializer>();
        services.AddEventBusInitializer<BikeMotorCalculations>();
        services.AddEventBusInitializer<EventTraceLogging>();
    }

    public static void AddEventBusInitializer<TInitializer>(this IServiceCollection services)
        where TInitializer : class, IEventBusInitializer =>
        services.AddSingleton<IEventBusInitializer, TInitializer>();

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

    public static void UseEventing(this IServiceProvider services)
    {
        var bus = services.GetRequiredService<IEventBus>();
        foreach (var eventBusInitializer in services.GetServices<IEventBusInitializer>()) eventBusInitializer.Initialize(bus);
    }

    public static void UseProjections(this IServiceProvider services) =>
        services.GetServices<IEventProjector>();
}
