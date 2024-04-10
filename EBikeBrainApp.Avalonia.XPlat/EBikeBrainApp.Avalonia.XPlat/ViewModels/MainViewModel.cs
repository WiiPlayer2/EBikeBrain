using System;
using EBikeBrainApp.Application;
using LanguageExt.Sys.Live;
using Reactive.Bindings;
using Reactive.Bindings.TinyLinq;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly DisplayService<Runtime> displayService;

    public MainViewModel(DisplayService<Runtime> displayService)
    {
        this.displayService = displayService;

        Speed = displayService.Speed
            .Select(x => x.Match(x => x.KilometersPerHour.ToString("0.0"), _ => "---"))
            .ToReadOnlyReactiveProperty();
    }

    public ReadOnlyReactiveProperty<string> Speed { get; }
}
