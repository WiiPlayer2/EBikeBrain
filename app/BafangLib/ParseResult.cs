using System;

namespace BafangLib;

public record ParseResult<T>(T Value, int Offset, int Length, byte? Checksum = default);
