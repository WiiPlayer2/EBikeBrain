using System;

namespace EBikeBrainApp.Avalonia.XPlat.ViewModels;

public class MainViewModel(
    DisplayViewModel displayViewModel
) : ViewModelBase
{
    public DisplayViewModel Display => displayViewModel;
}
