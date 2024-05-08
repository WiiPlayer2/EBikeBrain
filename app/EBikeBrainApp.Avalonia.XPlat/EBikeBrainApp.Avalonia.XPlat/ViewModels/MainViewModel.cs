using System;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels;

public class MainViewModel(
    DisplayViewModel displayViewModel,
    SettingsViewModel settingsViewModel,
    DemoCardsViewModel demoCardsViewModel
) : ViewModelBase
{
    public DemoCardsViewModel DemoCards => demoCardsViewModel;

    public DisplayViewModel Display => displayViewModel;

    public SettingsViewModel Settings => settingsViewModel;
}
