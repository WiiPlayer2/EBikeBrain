using System;

namespace BafangLib;

public static class ResponseParser
{
    public static ParseResult<ushort>? ParseGetRpmResponse(ReadOnlySpan<byte> buffer, int offset, int length) =>
        ParseUInt16(buffer, offset, length);

    private static ParseResult<ushort>? ParseUInt16(ReadOnlySpan<byte> buffer, int offset, int length) =>
        length < 3
            ? null
            : new ParseResult<ushort>((ushort) (buffer[offset] * 256u + buffer[offset + 1]), offset, 2, buffer[offset + 2]);
}
