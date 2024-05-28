using System;

namespace BafangLib.Messages;

public record GetErrorResponse(ErrorCode Value)
{
    public const int LENGTH = 1;
}
