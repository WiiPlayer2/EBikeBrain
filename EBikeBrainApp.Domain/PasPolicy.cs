namespace EBikeBrainApp.Domain;

public static class PasPolicy
{
    public static bool CanDecrease(this PasLevel level) => level.TryDecrease().IsSome;

    public static bool CanIncrease(this PasLevel level) => level.TryIncrease().IsSome;

    public static Option<PasLevel> TryDecrease(this PasLevel level) => level switch
    {
        PasLevel.Level2 => PasLevel.Level1,
        PasLevel.Level3 => PasLevel.Level2,
        PasLevel.Level4 => PasLevel.Level3,
        PasLevel.Level5 => PasLevel.Level4,
        PasLevel.Level6 => PasLevel.Level5,
        PasLevel.Level7 => PasLevel.Level6,
        PasLevel.Level8 => PasLevel.Level7,
        PasLevel.Level9 => PasLevel.Level8,
        _ => None,
    };

    public static Option<PasLevel> TryIncrease(this PasLevel level) => level switch
    {
        PasLevel.Level1 => PasLevel.Level2,
        PasLevel.Level2 => PasLevel.Level3,
        PasLevel.Level3 => PasLevel.Level4,
        PasLevel.Level4 => PasLevel.Level5,
        PasLevel.Level5 => PasLevel.Level6,
        PasLevel.Level6 => PasLevel.Level7,
        PasLevel.Level7 => PasLevel.Level8,
        PasLevel.Level8 => PasLevel.Level9,
        _ => None,
    };
}
