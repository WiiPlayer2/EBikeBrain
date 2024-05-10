namespace EBikeBrainApp.Application.Abstractions.Events;

public record EventStreamError(Exception Exception);

public record EventStreamError<T>(Exception Exception) : EventStreamError(Exception);

public record EventStreamCompleted;

public record EventStreamCompleted<T> : EventStreamCompleted;
