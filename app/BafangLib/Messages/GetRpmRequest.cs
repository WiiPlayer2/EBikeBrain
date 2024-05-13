using System;

namespace BafangLib.Messages;

public abstract record Request;

public record GetRpmRequest : Request;

public record GetCurrentRequest : Request;

public record SetPasRequest(Pas Level) : Request;

public enum Pas : byte
{
    Level0 = 0x00,

    Level1 = 0x01,

    Level2 = 0x0B,

    Level3 = 0x0C,

    Level4 = 0x0D,

    Level5 = 0x02,

    Level6 = 0x15,

    Level7 = 0x16,

    Level8 = 0x17,

    Level9 = 0x03,
}
