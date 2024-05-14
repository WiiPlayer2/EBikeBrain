using System;

namespace BafangLib.Utils;

public static class Extensions
{
    public static B? Map<A, B>(this A? value, Func<A, B> fn)
        where A : notnull
        => value is null ? default : fn(value);

    public static B? MapS<A, B>(this A? value, Func<A, B> fn)
        where A : struct
        => value is null ? default : fn(value.Value);
}
