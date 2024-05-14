using System;
using BafangLib.Messages;
using BafangLib.Utils;

namespace BafangLib;

public static class ResponseParser
{
    public static ParseResult<GetBatteryResponse>? ParseGetBatteryResponse(ReadOnlySpan<byte> buffer, int offset, int length) =>
        ParseUInt8(buffer, offset, length)
            .Map(x => new ParseResult<GetBatteryResponse>(new GetBatteryResponse(x.Value), x.Offset, x.Length, x.Checksum));

    public static ParseResult<GetCurrentResponse>? ParseGetCurrentResponse(ReadOnlySpan<byte> buffer, int offset, int length) =>
        ParseUInt8(buffer, offset, length)
            .Map(x => new ParseResult<GetCurrentResponse>(new GetCurrentResponse(x.Value / 2m), x.Offset, x.Length, x.Checksum));

    public static ParseResult<GetRpmResponse>? ParseGetRpmResponse(ReadOnlySpan<byte> buffer, int offset, int length) =>
        ParseUInt16(buffer, offset, length)
            .Map(x => new ParseResult<GetRpmResponse>(
                new GetRpmResponse(x.Value),
                x.Offset,
                x.Length,
                x.Checksum.MapS(x => (byte) (x - 0x20))));

    public static ParseResult<ReadOnlyMemory<byte>>? ParseUnknownResponse(ReadOnlySpan<byte> buffer, int offset, int length)
    {
        for (var i = 1; i < length; i++)
        {
            var calculatedChecksum = Checksum.Calculate(buffer, offset, i);
            var sentChecksum = buffer[offset + i];

            if (calculatedChecksum == sentChecksum)
                return new ParseResult<ReadOnlyMemory<byte>>(
                    buffer.Slice(offset, i).ToArray(),
                    offset,
                    i,
                    sentChecksum);
        }

        return null;
    }

    public static ParseResult<UnknownX1108Response>? ParseUnknownX1108Response(ReadOnlySpan<byte> buffer, int offset, int length) =>
        length < 1
            ? null
            : new ParseResult<UnknownX1108Response>(new UnknownX1108Response(buffer[offset]), offset, 1);

    private static ParseResult<ushort>? ParseUInt16(ReadOnlySpan<byte> buffer, int offset, int length) =>
        length < 3
            ? null
            : new ParseResult<ushort>((ushort) (buffer[offset] * 256u + buffer[offset + 1]), offset, 2, buffer[offset + 2]);

    private static ParseResult<byte>? ParseUInt8(ReadOnlySpan<byte> buffer, int offset, int length) =>
        length < 2
            ? null
            : new ParseResult<byte>(buffer[offset], offset, 1, buffer[offset + 1]);
}
