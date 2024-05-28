using System;

namespace BafangLib.Messages;

public record GetRpmResponse(ushort Rpm)
{
    public const int LENGTH = 2;
}
