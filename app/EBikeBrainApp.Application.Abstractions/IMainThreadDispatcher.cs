namespace EBikeBrainApp.Application.Abstractions;

public interface IMainThreadDispatcher
{
    Task<T> Invoke<T>(Func<Task<T>> fn);
}
