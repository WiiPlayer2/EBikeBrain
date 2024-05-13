using System;
using BafangLib.Messages;

namespace BafangLib;

public static class RequestParser
{
    public const int MAX_REQUEST_LENGTH = 3;

    public const int MIN_REQUEST_LENGTH = 2;

    public static ParseResult<Request>? Parse(ReadOnlySpan<byte> buffer) =>
        Parse(buffer, 0, buffer.Length);

    public static ParseResult<Request>? Parse(ReadOnlySpan<byte> buffer, int offset, int length)
    {
        while (length >= MIN_REQUEST_LENGTH)
        {
            var a = buffer[offset];
            var b = buffer[offset + 1];

            if (length >= 3)
            {
                var c = buffer[offset + 2];

                if (a == 0x16 && b == 0x0B)
                    return new ParseResult<Request>(new SetPasRequest((Pas) c), offset + 3);
            }

            if (a == 0x11 && b == 0x20)
                return new ParseResult<Request>(new GetRpmRequest(), offset + 2);

            if (a == 0x11 && b == 0x0A)
                return new ParseResult<Request>(new GetCurrentRequest(), offset + 2);

            offset += 1;
            length -= 1;
        }

        return null;
    }
}

public record ParseResult<T>(T Value, int EndOffset);
