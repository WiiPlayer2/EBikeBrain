using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using EBikeBrainApp.Avalonia.XPlat.ViewModels;

namespace EBikeBrainApp.Avalonia.XPlat;

public class ViewLocator : IDataTemplate
{
    public bool Match(object? data) => data is ViewModelBase;

    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var name = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null) return (Control) Activator.CreateInstance(type)!;

        return new TextBlock {Text = "Not Found: " + name};
    }
}
