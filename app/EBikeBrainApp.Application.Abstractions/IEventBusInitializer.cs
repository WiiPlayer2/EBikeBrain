namespace EBikeBrainApp.Application.Abstractions;

public interface IEventBusInitializer
{
    void Initialize(IEventBus bus);
}
