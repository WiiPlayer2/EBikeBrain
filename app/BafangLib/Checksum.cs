using System;
using System.Linq;

namespace BafangLib;

public static class Checksum
{
    public static byte Calculate(ReadOnlySpan<byte> buffer, int offset, int length) =>
        length == 0
            ? throw new InvalidOperationException("Length should be greater than zero.")
            : (byte) buffer.Slice(offset, length).ToArray().Sum(x => x);
}
