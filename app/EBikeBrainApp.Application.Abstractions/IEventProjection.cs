namespace EBikeBrainApp.Application.Abstractions;

public interface IEventProjection<T1, T2, TOut>
{
    public static abstract TOut Project(T1 arg1, T2 arg2);
}
