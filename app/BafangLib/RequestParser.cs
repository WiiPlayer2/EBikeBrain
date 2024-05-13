using System;
using BafangLib.Messages;

namespace BafangLib;

public static class RequestParser
{
    public static ParseResult<Request>? Parse(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 2)
            return null;

        if (buffer[0] == 0x11 && buffer[1] == 0x20)
            return new ParseResult<Request>(new GetRpmRequest(), 2);

        return null;
    }
}

public record ParseResult<T>(T Value, int EndOffset);
