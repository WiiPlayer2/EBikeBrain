using System;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels;

public class MainViewModel(
    DisplayViewModel displayViewModel,
    SettingsViewModel settingsViewModel
) : ViewModelBase
{
    public DisplayViewModel Display => displayViewModel;

    public SettingsViewModel Settings => settingsViewModel;
}
