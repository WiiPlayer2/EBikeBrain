using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EBikeBrain
{
    internal class BikeComm : IAsyncDisposable
    {
        public enum ErrorCode : byte
        {
            Ok = 0x01,
        }

        public enum PasLevel : byte
        {
            PAS0 = 0x00,
            PAS1 = 0x01,
            PAS2 = 0x0B,
            PAS3 = 0x0C,
            PAS4 = 0x0D,
            PAS5 = 0x02,
            PAS6 = 0x15,
            PAS7 = 0x16,
            PAS8 = 0x17,
            PAS9 = 0x03,
        }

        private readonly Stream inputStream;

        private readonly Stream outputStream;

        private readonly SemaphoreSlim semaphoreSlim = new(1);

        private readonly byte[] buffer = new byte[16];

        private readonly TimeSpan timeout = TimeSpan.FromSeconds(1);

        public bool IsBusy => semaphoreSlim.CurrentCount == 0;

        public BikeComm(Stream inputStream, Stream outputStream)
        {
            this.inputStream = inputStream;
            this.outputStream = outputStream;
        }

        public Task<byte[]> Request(int responseLength, params byte[] request)
            => Request(responseLength, default, request);

        public async Task<byte[]> Request(int responseLength, CancellationToken cancellationToken, params byte[] request)
        {
            var timedCancellationTokenSource = new CancellationTokenSource(timeout);
            var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(timedCancellationTokenSource.Token, cancellationToken);

            try
            {
                await semaphoreSlim.WaitAsync(timeout, linkedCancellationTokenSource.Token);
                await outputStream.WriteAsync(request, 0, request.Length, linkedCancellationTokenSource.Token);
                await outputStream.FlushAsync(linkedCancellationTokenSource.Token);
                if (responseLength == 0)
                    return Array.Empty<byte>();

                var actualResponseLength = await inputStream.ReadAsync(buffer, 0, responseLength, linkedCancellationTokenSource.Token);
                return buffer.Take(actualResponseLength).ToArray();
            }
            finally
            {
                semaphoreSlim.Release();
                timedCancellationTokenSource.Dispose();
            }
        }

        public async Task<ErrorCode> GetErrorCode()
        {
            var response = await Request(1, 0x11, 0x08);
            return (ErrorCode)response[0];
        }

        public async Task<ushort> GetWheelRpm()
        {
            var response = await Request(3, 0x11, 0x20);
            return (ushort)((response[0] * 256u) + response[1]);
        }

        public Task SetLights(bool isOn)
            => Request(1, 0x16, 0x1A, (byte) (isOn ? 0xF1 : 0xF0));

        public async Task<byte> GetBatteryPercentage()
        {
            var response = await Request(2, 0x11, 0x11);
            return response[0];
        }

        public async Task<double> GetAmps()
        {
            var response = await Request(2, 0x11, 0x0A);
            return response[0] / 2.0;
        }

        public Task SetPasLevel(PasLevel level, CancellationToken cancellationToken = default)
            => Request(0, cancellationToken, WithChecksum(0x16, 0x0B, (byte) level));

        private static byte[] WithChecksum(params byte[] data)
        {
            byte checksum = 0;
            foreach (var b in data)
                checksum += b;
            return data.Concat(new[] {checksum}).ToArray();
        }

        public async ValueTask DisposeAsync()
        {
            semaphoreSlim.Dispose();
            await inputStream.DisposeAsync();
            await outputStream.DisposeAsync();
        }
    }
}
