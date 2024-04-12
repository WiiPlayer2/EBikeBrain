namespace EBikeBrainApp.Implementations.Demo;

public class SlowStream : Stream
{
    private readonly Stream baseStream;

    private readonly TimeSpan interval;

    public SlowStream(Stream baseStream, TimeSpan interval)
    {
        this.baseStream = baseStream;
        this.interval = interval;
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
        var totalReadCount = 0;
        for (var i = 0; i < count; i++)
        {
            var readCount = baseStream.Read(buffer, offset + i, 1);
            if (readCount == 0)
                return totalReadCount;

            totalReadCount += readCount;
            Thread.Sleep(interval);
        }

        return totalReadCount;
    }

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        var totalReadCount = 0;
        for (var i = 0; i < count; i++)
        {
            var readCount = await baseStream.ReadAsync(buffer, offset + i, 1, cancellationToken);
            if (readCount == 0)
                return totalReadCount;

            totalReadCount += readCount;
            await Task.Delay(interval, cancellationToken);
        }

        return totalReadCount;
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
