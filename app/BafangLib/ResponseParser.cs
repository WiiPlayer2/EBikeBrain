using System;
using BafangLib.Utils;

namespace BafangLib;

public static class ResponseParser
{
    public static ParseResult<byte>? ParseGetBatteryResponse(ReadOnlySpan<byte> buffer, int offset, int length) =>
        ParseUInt8(buffer, offset, length);

    public static ParseResult<decimal>? ParseGetCurrentResponse(ReadOnlySpan<byte> buffer, int offset, int length) =>
        ParseUInt8(buffer, offset, length)
            .Map(x => new ParseResult<decimal>(x.Value / 2m, x.Offset, x.Length, x.Checksum));

    public static ParseResult<ushort>? ParseGetRpmResponse(ReadOnlySpan<byte> buffer, int offset, int length) =>
        ParseUInt16(buffer, offset, length);

    private static ParseResult<ushort>? ParseUInt16(ReadOnlySpan<byte> buffer, int offset, int length) =>
        length < 3
            ? null
            : new ParseResult<ushort>((ushort) (buffer[offset] * 256u + buffer[offset + 1]), offset, 2, buffer[offset + 2]);

    private static ParseResult<byte>? ParseUInt8(ReadOnlySpan<byte> buffer, int offset, int length) =>
        length < 2
            ? null
            : new ParseResult<byte>(buffer[offset], offset, 1, buffer[offset + 1]);
}
