using System;

namespace BafangLib.Messages;

public record GetBatteryResponse(byte Percentage)
{
    public const int LENGTH = 1;
}
