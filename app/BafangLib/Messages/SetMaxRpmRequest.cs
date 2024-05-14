using System;

namespace BafangLib.Messages;

public record SetMaxRpmRequest(ushort Rpm) : Request;
