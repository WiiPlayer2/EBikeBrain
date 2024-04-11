using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using EBikeBrainApp.Application.Abstractions;

namespace EBikeBrainApp.Avalonia.XPlat;

internal class AvaloniaMainThreadDispatcher : IMainThreadDispatcher
{
    public Task<T> Invoke<T>(Func<Task<T>> fn) => Dispatcher.UIThread.InvokeAsync(fn);
}
