using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using BafangLib;
using BafangLib.Messages;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;
using Microsoft.Extensions.Logging;
using UnitsNet;

namespace EBikeBrainApp.Protocols.Bafang;

public class ProtocolInterceptorBikeMotor : IBikeMotor, IDisposable
{
    private const string TAG_DISPLAY_TO_MOTOR = "DISPLAY -> MOTOR:";

    private const string TAG_MOTOR_TO_DISPLAY = "MOTOR -> DISPLAY:";

    private readonly IDisposable connection;

    public ProtocolInterceptorBikeMotor(Stream inputStream, ILogger<ProtocolInterceptorBikeMotor> logger)
    {
        var interceptedLines = Observable.Create<string>(async (observer, token) =>
            {
                using var reader = new StreamReader(inputStream);

                var buffer = new char[16];

                var text = string.Empty;
                logger.LogDebug("Starting protocol interception...");
                while (!token.IsCancellationRequested)
                {
                    var readCount = await reader.ReadBlockAsync(buffer, token);
                    text += new string(buffer, 0, readCount);

                    int newLineIndex;
                    while ((newLineIndex = text.IndexOf('\n')) >= 0)
                    {
                        var line = text[..newLineIndex];

                        // HACK: idk why but apparently this sometimes locks up on android if run synchronously (even if awaited)
                        // Task.Run(() => observer.OnNext(line.Trim()), token);
                        observer.OnNext(line.Trim());

                        text = text[(newLineIndex + 1)..];
                    }
                }
            })
            .Do(line => { logger.LogTrace("{{{{{line}}}}}", line); })
            .Publish()
            .RefCount();

        var packets = interceptedLines
            .Select(line =>
            {
                if (line.StartsWith(TAG_DISPLAY_TO_MOTOR))
                {
                    var data = GetData(TAG_DISPLAY_TO_MOTOR);
                    return (isRequest: true, data);
                }

                if (line.StartsWith(TAG_MOTOR_TO_DISPLAY))
                {
                    var data = GetData(TAG_MOTOR_TO_DISPLAY);
                    return (isRequest: false, data);
                }

                logger.LogWarning("Unable to parse line \"{line}\".", line);
                return (isRequest: default(bool?), data: default(byte[]));

                byte[] GetData(string tagLine) => line[tagLine.Length..]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s =>
                    {
                        if (byte.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
                            return value;

                        logger.LogWarning("Failed to parse \"{line}\" with tag line \"{tagLine}\".", line, tagLine);
                        return default(byte?);
                    })
                    .Where(x => x is not null)
                    .Select(x => x!.Value)
                    .ToArray();
            })
            .Where(x => x.isRequest.HasValue)
            .Select(t => (isRequest: t.isRequest!.Value, data: t.data!));

        // var messages = packets
        //     .Buffer(2)
        //     .Where(x => x.Count >= 2)
        //     .Select(l => (Request: l[0].data, Response: l[1].data));

        // RotationalSpeed = messages
        //     .Where(t => IsRequest(t.Request, 0x11, 0x20))
        //     .Select(t => UnitsNet.RotationalSpeed.FromRevolutionsPerMinute((ushort) (t.Response[0] * 256u + t.Response[1])));
        //
        // PasLevel = messages
        //     .Where(t => IsRequest(t.Request, 0x16, 0x0B))
        //     .Select(t => t.Request[2] switch
        //     {
        //         0x00 => Domain.PasLevel.Level0,
        //         0x01 => Domain.PasLevel.Level1,
        //         0x02 => Domain.PasLevel.Level5,
        //         0x03 => Domain.PasLevel.Level9,
        //
        //         0x0B => Domain.PasLevel.Level2,
        //         0x0C => Domain.PasLevel.Level3,
        //         0x0D => Domain.PasLevel.Level4,
        //
        //         0x15 => Domain.PasLevel.Level6,
        //         0x16 => Domain.PasLevel.Level7,
        //         0x17 => Domain.PasLevel.Level8,
        //
        //         _ => Domain.PasLevel.Unknown, // 0x06
        //     });
        //
        // Current = messages
        //     .Where(t => IsRequest(t.Request, 0x11, 0x0A))
        //     .Select(t => ElectricCurrent.FromAmperes(t.Response[0] / 2.0));
        //
        // Battery = messages
        //     .Where(t => IsRequest(t.Request, 0x11, 0x11))
        //     .Select(t => Percentage.From(t.Response[0]));

        var parsedMessages = Observable.Create<object>(async (observer, token) =>
            {
                var initRpm = false;
                await using var packetEnumerator = packets
                    .ToAsyncEnumerable()
                    .GetAsyncEnumerator(token);

                var displayBuffer = new List<byte>();
                var motorPacketBuffer = new List<byte>();

                while (!token.IsCancellationRequested)
                {
                    while (displayBuffer.Count < RequestParser.MIN_REQUEST_LENGTH)
                        if (!await ConsumePacket())
                            return;

                    var request = RequestParser.Parse(displayBuffer.ToArray());
                    if (request is null)
                    {
                        // logger.LogError("Did not find command in: {data}", string.Join(" ", displayBuffer.Select(x => x.ToString("X2"))));
                        // return;
                        //
                        // displayBuffer.RemoveAt(0);

                        logger.LogTrace("Did not find command. Consuming more data...");
                        await ConsumePacket();
                        continue;
                    }

                    if (request.Offset != 0)
                    {
                        var requestData = displayBuffer[..request.Offset];
                        logger.LogWarning("Unknown command: {command}", string.Join(" ", requestData.Select(x => x.ToString("X2"))));
                        displayBuffer = displayBuffer[request.Offset..];
                        continue;
                    }

                    // while (motorPacketBuffer.Count < (ResponseParser.MAX_RESPONSE_LENGTH + 1))
                    //     if (!await ConsumePacket())
                    //         return;

                    logger.LogTrace("Handling {request}...", request.Value);

                    switch (request.Value)
                    {
                        case SetPasRequest setPasRequest:
                            observer.OnNext(setPasRequest);
                            break;

                        case GetRpmRequest:
                            if (initRpm)
                            {
                                if (!await EnsureBuffer(motorPacketBuffer, GetRpmResponse.LENGTH + 1))
                                    return;
                                HandleResponse("GetRpm", ResponseParser.ParseGetRpmResponse);
                            }
                            else
                                initRpm = true;

                            break;

                        case GetCurrentRequest:
                            if (!await EnsureBuffer(motorPacketBuffer, GetCurrentResponse.LENGTH + 1))
                                return;
                            HandleResponse("GetCurrent", ResponseParser.ParseGetCurrentResponse);
                            break;

                        case GetBatteryRequest:
                            if (!await EnsureBuffer(motorPacketBuffer, GetBatteryResponse.LENGTH + 1))
                                return;
                            HandleResponse("GetBattery", ResponseParser.ParseGetBatteryResponse);
                            break;

                        case GetErrorRequest:
                            if (!await EnsureBuffer(motorPacketBuffer, GetErrorResponse.LENGTH))
                                return;
                            HandleResponse("GetError", ResponseParser.ParseGetErrorResponse, false);
                            break;

                        case SetMaxRpmRequest setMaxRpmRequest:
                            observer.OnNext(setMaxRpmRequest);
                            break;

                        case SetLightsRequest setLightsRequest:
                            observer.OnNext(setLightsRequest);
                            break;

                        case Unknown_x1122_Request:
                            logger.LogDebug("Unknown request 0x1122 received.");
                            break;

                        case Unknown_x11F0_Request:
                            logger.LogDebug("Unknown request 0x11F0 received.");
                            break;

                        default:
                            logger.LogWarning("Unhandled command: {command}", request.Value);
                            break;
                    }

                    displayBuffer = request.Checksum.HasValue
                        ? displayBuffer[(request.Offset + request.Length + 1)..]
                        : displayBuffer[(request.Offset + request.Length)..];

                    ParseResult<T>? TryFindResponse<T>(ParseResponseDelegate<T> parse)
                    {
                        var buffer = motorPacketBuffer.ToArray();
                        for (var i = 0; i < buffer.Length; i++)
                        {
                            var response = parse(buffer, i, buffer.Length - i);

                            if (response?.Checksum == null)
                                return response;

                            var checksum = Checksum.Calculate(buffer, i, response.Length);
                            if (checksum == response.Checksum)
                                return response;
                        }

                        return default;
                    }

                    void HandleResponse<T>(string name, ParseResponseDelegate<T> parse, bool check = true)
                        where T : notnull
                    {
                        var response = TryFindResponse(parse);
                        if (response is null)
                        {
                            logger.LogError($"Failed to parse {name} response");
                            return;
                        }

                        if (check)
                        {
                            var checksum = Checksum.Calculate(motorPacketBuffer.ToArray(), response.Offset, response.Length);
                            if (response.Checksum != checksum)
                                logger.LogError($"Checksum for {name} failed. Expected {{expectedChecksum}}, but got {{actualChecksum}}.", checksum, response.Checksum);
                            else
                                observer.OnNext(response.Value);
                        }
                        else
                        {
                            observer.OnNext(response.Value);
                        }

                        if (response.Offset != 0) logger.LogWarning("Discarding response data: {data}", string.Join(" ", motorPacketBuffer.Take(response.Offset).Select(x => x.ToString("X2"))));

                        motorPacketBuffer = response.Checksum.HasValue
                            ? motorPacketBuffer[(response.Offset + response.Length + 1)..]
                            : motorPacketBuffer[(response.Offset + response.Length)..];
                    }

                    async Task<bool> EnsureBuffer(List<byte> buffer, int count)
                    {
                        // HACK just adding a few more bytes in case of misaligned responses
                        while (buffer.Count < count + ResponseParser.MAX_RESPONSE_LENGTH)
                            if (!await ConsumePacket())
                                return false;

                        return true;
                    }

                    async Task<bool> ConsumePacket()
                    {
                        if (!await packetEnumerator.MoveNextAsync())
                        {
                            observer.OnCompleted();
                            return false;
                        }

                        if (packetEnumerator.Current.isRequest)
                            displayBuffer.AddRange(packetEnumerator.Current.data);
                        else
                            motorPacketBuffer.AddRange(packetEnumerator.Current.data);

                        return true;
                    }
                }
            })
            .Do(
                o => { },
                e => logger.LogError(e, "Error while parsing packets."))
            .Publish();

        PasLevel = parsedMessages
            .OfType<SetPasRequest>()
            .Select(x => x.Level switch
            {
                Pas.Level0 => Domain.PasLevel.Level0,
                Pas.Level1 => Domain.PasLevel.Level1,
                Pas.Level5 => Domain.PasLevel.Level5,
                Pas.Level9 => Domain.PasLevel.Level9,

                Pas.Level2 => Domain.PasLevel.Level2,
                Pas.Level3 => Domain.PasLevel.Level3,
                Pas.Level4 => Domain.PasLevel.Level4,

                Pas.Level6 => Domain.PasLevel.Level6,
                Pas.Level7 => Domain.PasLevel.Level7,
                Pas.Level8 => Domain.PasLevel.Level8,

                _ => Domain.PasLevel.Unknown, // 0x06
            });

        RotationalSpeed = parsedMessages
            .OfType<GetRpmResponse>()
            .Select(x => UnitsNet.RotationalSpeed.FromRevolutionsPerMinute(x.Rpm));

        Battery = parsedMessages
            .OfType<GetBatteryResponse>()
            .Select(x => Percentage.From(x.Percentage));

        Current = parsedMessages
            .OfType<GetCurrentResponse>()
            .Select(x => ElectricCurrent.FromAmperes(x.Amperes));

        var errorLogging = parsedMessages
            .OfType<GetErrorResponse>()
            .Where(x => x.Value != ErrorCode.Ok)
            .Subscribe(x => logger.LogWarning("Received error code {error}", x.Value));

        connection = new CompositeDisposable(
            parsedMessages.Connect(),
            // interceptedLines.Connect(),
            errorLogging);
    }

    public IObservable<Percentage> Battery { get; }

    public IObservable<ElectricCurrent> Current { get; }

    public IObservable<PasLevel> PasLevel { get; }

    public IObservable<RotationalSpeed> RotationalSpeed { get; }

    public ValueTask SetPasLevel(PasLevel level, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public void Dispose()
    {
        connection.Dispose();
    }

    private static bool IsRequest(byte[] request, params byte[] expectedRequest) =>
        request.Length >= expectedRequest.Length && request.Take(expectedRequest.Length).Zip(expectedRequest).All(t => t.Item1 == t.Item2);

    private delegate ParseResult<T>? ParseResponseDelegate<T>(ReadOnlySpan<byte> buffer, int offset, int length);
}
