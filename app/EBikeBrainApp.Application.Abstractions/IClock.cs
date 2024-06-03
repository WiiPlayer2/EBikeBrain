namespace EBikeBrainApp.Application.Abstractions;

public interface IClock
{
    DateTimeOffset Now { get; }
}
