using System.Diagnostics;

namespace EBikeBrainApp.Implementations.Demo;

public class SlowStream : Stream
{
    private readonly Stream baseStream;

    private readonly TimeSpan interval;

    private readonly Stopwatch waitStopwatch;

    public SlowStream(Stream baseStream, TimeSpan interval)
    {
        this.baseStream = baseStream;
        this.interval = interval;
        waitStopwatch = new Stopwatch();
        waitStopwatch.Start();
    }

    public override bool CanRead => baseStream.CanRead;

    public override bool CanSeek { get; }

    public override bool CanWrite { get; }

    public override long Length => baseStream.Length;

    public override long Position
    {
        get => baseStream.Position;
        set => baseStream.Position = value;
    }

    public override async ValueTask DisposeAsync()
    {
        await baseStream.DisposeAsync();
        await base.DisposeAsync();
    }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (waitStopwatch.Elapsed < interval)
            return 0;

        waitStopwatch.Stop();
        var readCount = baseStream.Read(buffer, offset, 1);

        if (readCount > 0)
            waitStopwatch.Restart();

        return readCount;
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }
}
