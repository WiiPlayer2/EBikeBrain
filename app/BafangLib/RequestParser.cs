using System;
using BafangLib.Messages;

namespace BafangLib;

public static class RequestParser
{
    public static ParseResult<Request>? Parse(ReadOnlySpan<byte> buffer) =>
        Parse(buffer, 0, buffer.Length);

    public static ParseResult<Request>? Parse(ReadOnlySpan<byte> buffer, int offset, int length)
    {
        while (true)
        {
            if (length < 2)
                return null;

            if (length >= 3)
                if (buffer[offset] == 0x16 && buffer[offset + 1] == 0x0B)
                    return new ParseResult<Request>(new SetPasRequest((Pas) buffer[offset + 2]), offset + 3);

            if (buffer[offset] == 0x11 && buffer[offset + 1] == 0x20)
                return new ParseResult<Request>(new GetRpmRequest(), offset + 2);

            offset += 1;
            length -= 1;
        }
    }
}

public record ParseResult<T>(T Value, int EndOffset);
