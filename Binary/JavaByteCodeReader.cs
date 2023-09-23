using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

    public byte[] ReadCount(int count)
    {
        byte[] arr = new byte[count];
        stream.Read(arr, 0, count);
        return arr;
    }
    public byte[] ReadCount(long count)
    {
        List<byte[]> arrays = new List<byte[]>();
        while (true)
        {
            if (count <= int.MaxValue)
            {
                arrays.Add(ReadCount((int)count));
                break;
            }
            arrays.Add(ReadCount(int.MaxValue));
            count -= int.MaxValue;
        }
        return Combine(arrays);
    }
    private byte[] ReadCountReverse(int count)
    {
        byte[] arr = ReadCount(count);
        Array.Reverse(arr);
        return arr;
    }

    public byte ReadByte() => (byte)stream.ReadByte();
    public ushort ReadUShort() => BitConverter.ToUInt16(ReadCountReverse(2));
    public uint ReadUInt() => BitConverter.ToUInt32(ReadCountReverse(4));

    public short ReadShort() => BitConverter.ToInt16(ReadCountReverse(2));
    public int ReadInt() => BitConverter.ToInt32(ReadCountReverse(4));
    public long ReadLong() => BitConverter.ToInt64(ReadCountReverse(8));

    public float ReadFloat() => BitConverter.ToSingle(ReadCountReverse(4));
    public double ReadDouble() => BitConverter.ToDouble(ReadCountReverse(8));

    public string ReadUTF8(int count) => Encoding.UTF8.GetString(ReadCount(count));

    public byte[] ReadToEnd()
    {
        using MemoryStream memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
