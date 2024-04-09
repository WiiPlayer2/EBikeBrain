using LanguageExt.Effects.Traits;

namespace EBikeBrainApp.Application;

public class BikeService<RT> where RT : struct, HasCancel<RT> { }
