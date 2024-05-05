using System;
using Microsoft.Extensions.DependencyInjection;

namespace EBikeBrainApp.Avalonia.XPlat;

public delegate void PlatformSpecificServiceRegistrationDelegate(IServiceCollection services);
