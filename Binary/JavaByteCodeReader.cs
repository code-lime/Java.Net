using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Java.Net.Binary;

public class JavaByteCodeReader : JavaByteCode
{
    public JavaByteCodeReader(Stream stream) : base(stream) { }

    private static T[] Combine<T>(IEnumerable<T[]> arrays)
    {
        T[] bytes = Array.Empty<T>();
        foreach (T[] array in arrays)
        {
            int length = bytes.Length;
            Array.Resize(ref bytes, length + array.Length);
            Array.Copy(array, 0, bytes, length, array.Length);
        }
        return bytes;
    }

    public async ValueTask<byte[]> ReadCountAsync(int count, CancellationToken cancellationToken)
    {
        byte[] arr = new byte[count];
        await stream.ReadAsync(arr, cancellationToken);
        return arr;
    }
    public async ValueTask<byte[]> ReadCountAsync(long count, CancellationToken cancellationToken)
    {
        List<byte[]> arrays = new List<byte[]>();
        while (true)
        {
            if (count <= int.MaxValue)
            {
                arrays.Add(await ReadCountAsync((int)count, cancellationToken));
                break;
            }
            arrays.Add(await ReadCountAsync(int.MaxValue, cancellationToken));
            count -= int.MaxValue;
        }
        return Combine(arrays);
    }
    private async ValueTask<byte[]> ReadCountReverseAsync(int count, CancellationToken cancellationToken)
    {
        byte[] arr = await ReadCountAsync(count, cancellationToken);
        Array.Reverse(arr);
        return arr;
    }

    public async ValueTask<byte> ReadByteAsync(CancellationToken cancellationToken)
    {
        byte[] bytes = new byte[1];
        await stream.ReadAsync(bytes, cancellationToken);
        return bytes[0];
    }

    public async ValueTask<ushort> ReadUShortAsync(CancellationToken cancellationToken) => BitConverter.ToUInt16(await ReadCountReverseAsync(2, cancellationToken));
    public async ValueTask<uint> ReadUIntAsync(CancellationToken cancellationToken) => BitConverter.ToUInt32(await ReadCountReverseAsync(4, cancellationToken));

    public async ValueTask<short> ReadShortAsync(CancellationToken cancellationToken) => BitConverter.ToInt16(await ReadCountReverseAsync(2, cancellationToken));
    public async ValueTask<int> ReadIntAsync(CancellationToken cancellationToken) => BitConverter.ToInt32(await ReadCountReverseAsync(4, cancellationToken));
    public async ValueTask<long> ReadLongAsync(CancellationToken cancellationToken) => BitConverter.ToInt64(await ReadCountReverseAsync(8, cancellationToken));

    public async ValueTask<float> ReadFloatAsync(CancellationToken cancellationToken) => BitConverter.ToSingle(await ReadCountReverseAsync(4, cancellationToken));
    public async ValueTask<double> ReadDoubleAsync(CancellationToken cancellationToken) => BitConverter.ToDouble(await ReadCountReverseAsync(8, cancellationToken));

    public async ValueTask<string> ReadUTF8Async(int count, CancellationToken cancellationToken) => Encoding.UTF8.GetString(await ReadCountAsync(count, cancellationToken));

    public async ValueTask<byte[]> ReadToEndAsync(CancellationToken cancellationToken)
    {
        using MemoryStream memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        return memoryStream.ToArray();
    }
}
