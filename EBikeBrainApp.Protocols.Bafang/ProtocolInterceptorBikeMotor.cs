using System.Globalization;
using System.Reactive.Linq;
using EBikeBrainApp.Application.Abstractions;
using EBikeBrainApp.Domain;
using UnitsNet;

namespace EBikeBrainApp.Implementations.Android;

public class ProtocolInterceptorBikeMotor : IBikeMotor, IDisposable
{
    private const string TAG_DISPLAY_TO_MOTOR = "DISPLAY -> MOTOR:";

    private const string TAG_MOTOR_TO_DISPLAY = "MOTOR -> DISPLAY:";

    private readonly IDisposable connection;

    public ProtocolInterceptorBikeMotor(Stream inputStream)
    {
        var interceptedLines = Observable.Create<string>(async (observer, token) =>
            {
                using var reader = new StreamReader(inputStream);
                while (await reader.ReadLineAsync(token) is { } line) observer.OnNext(line);
            })
            .Publish();

        IObservable<(byte[] Request, byte[] Response)> messages = interceptedLines
            .Select(line =>
            {
                if (line.StartsWith(TAG_DISPLAY_TO_MOTOR))
                {
                    var data = GetData(TAG_DISPLAY_TO_MOTOR);
                    return (true, data);
                }

                if (line.StartsWith(TAG_MOTOR_TO_DISPLAY))
                {
                    var data = GetData(TAG_MOTOR_TO_DISPLAY);
                    return (false, data);
                }

                byte[] GetData(string tagLine) => line
                    .Substring(tagLine.Length)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => byte.Parse(s, NumberStyles.HexNumber))
                    .ToArray();

                throw new InvalidOperationException($"Unable to parse line \"{line}\"");
            })
            .SkipWhile(t => !t.Item1)
            .Buffer(2)
            .Select(l => (l[0].data, l[1].data));

        RotationalSpeed = messages
            .Where(t => IsRequest(t.Request, 0x11, 0x20))
            .Select(t => UnitsNet.RotationalSpeed.FromRevolutionsPerMinute((ushort) (t.Response[0] * 256u + t.Response[1])));

        connection = interceptedLines.Connect();
    }

    public IObservable<PasLevel> PasLevel { get; } = Observable.Never<PasLevel>();

    public IObservable<RotationalSpeed> RotationalSpeed { get; }

    public ValueTask SetPasLevel(PasLevel level, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public void Dispose()
    {
        connection.Dispose();
    }

    private static bool IsRequest(byte[] request, params byte[] expectedRequest) =>
        request.Length == expectedRequest.Length && request.Zip(expectedRequest).All(t => t.Item1 == t.Item2);
}
