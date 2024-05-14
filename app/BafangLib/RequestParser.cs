using System;
using BafangLib.Messages;

namespace BafangLib;

public static class RequestParser
{
    public const int MAX_REQUEST_LENGTH = 6;

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

                if (a == 0x16)
                {
                    if (length >= 4)
                    {
                        var d = buffer[offset + 3];

                        if (length >= 5)
                        {
                            var e = buffer[offset + 4];

                            switch (b)
                            {
                                case 0x1F:
                                    return new ParseResult<Request>(new SetMaxRpmRequest((ushort) (c * 256 + d)), offset, 4, e);
                            }
                        }

                        switch (b)
                        {
                            case 0x0B:
                                return new ParseResult<Request>(new SetPasRequest((Pas) c), offset, 3, d);
                        }
                    }

                    switch (b)
                    {
                        case 0x1A:
                            return new ParseResult<Request>(new SetLightsRequest(c == 0xF1), offset, 3);
                    }
                }
            }

            if (a == 0x11)
            {
                if (length >= 3)
                {
                    var c = buffer[offset + 2];

                    switch (b)
                    {
                        case 0x22:
                            return new ParseResult<Request>(new UnknownX1122Request(), offset, 2, c);
                    }
                }

                switch (b)
                {
                    case 0x08:
                        return new ParseResult<Request>(new GetErrorRequest(), offset, 2);
                    case 0x0A:
                        return new ParseResult<Request>(new GetCurrentRequest(), offset, 2);
                    case 0x11:
                        return new ParseResult<Request>(new GetBatteryRequest(), offset, 2);
                    case 0x20:
                        return new ParseResult<Request>(new GetRpmRequest(), offset, 2);
                }
            }

            offset += 1;
            length -= 1;
        }

        return null;
    }
}
