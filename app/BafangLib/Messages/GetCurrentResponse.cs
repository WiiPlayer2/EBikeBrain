using System;

namespace BafangLib.Messages;

public record GetCurrentResponse(decimal Amperes)
{
    public const int LENGTH = 1;
}
