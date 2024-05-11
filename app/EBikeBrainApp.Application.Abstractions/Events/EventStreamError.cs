namespace EBikeBrainApp.Application.Abstractions.Events;

public record EventStreamError(Type StreamType, Exception Exception);

public record EventStreamError<T>(Exception Exception) : EventStreamError(typeof(T), Exception);

public record EventStreamCompleted(Type StreamType);

public record EventStreamCompleted<T>() : EventStreamCompleted(typeof(T));
