using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Java.Net
{
    public abstract class JavaByteCode
    {
        public Stream Stream => stream;
        public long Length => stream.Length;
        public long Position => stream.Position;
        public bool CanReadNext => stream.Position < stream.Length;
        protected readonly Stream stream;
        protected JavaByteCode(Stream stream) => this.stream = stream;
    }
    public class JavaByteCodeWriter : JavaByteCode
    {
        protected readonly JArray json = new JArray();
        public class Handle
        {
            public string Method { get; }
            public object Value { get; }

            public Handle(string method, object value)
            {
                this.Method = method;
                this.Value = value;
            }
        }


        public event Action<JavaByteCodeWriter, Handle> OnUpdate;

        protected T Debug<T>(T obj, Handle value)
        {
            Debug(value);
            return obj;
        }
        protected JavaByteCodeWriter Debug(Handle value)
        {
            if (OnUpdate != null) OnUpdate.Invoke(this, value);
            return this;
        }
        protected T Debug<T>(T obj, string method, object value) => Debug(obj, new Handle(method, value));
        protected JavaByteCodeWriter Debug(string method, object value) => Debug(new Handle(method, value));
        protected T Debug<T>(T obj, object value) => Debug(obj, new Handle(value.GetType().Name, value));
        protected JavaByteCodeWriter Debug(object value) => Debug(new Handle(value.GetType().Name, value));

        public JavaByteCodeWriter(Stream stream) : base(stream) { }

        public JavaByteCodeWriter WriteCount(byte[] data)
        {
            stream.Write(data, 0, data.Length);
            return this;
        }
        private JavaByteCodeWriter WriteCountReverse(byte[] data)
        {
            data = (byte[])data.Clone();
            Array.Reverse(data);
            return WriteCount(data);
        }

        public JavaByteCodeWriter WriteByte(byte value)
        {
            stream.WriteByte(value);
            return this.Debug(value);
        }

        public JavaByteCodeWriter WriteUShort(ushort value) => WriteCountReverse(BitConverter.GetBytes(value)).Debug(value);
        public JavaByteCodeWriter WriteUInt(uint value) => WriteCountReverse(BitConverter.GetBytes(value)).Debug(value);

        public JavaByteCodeWriter WriteInt(int value) => WriteCountReverse(BitConverter.GetBytes(value)).Debug(value);
        public JavaByteCodeWriter WriteLong(long value) => WriteCountReverse(BitConverter.GetBytes(value)).Debug(value);

        public JavaByteCodeWriter WriteFloat(float value) => WriteCountReverse(BitConverter.GetBytes(value)).Debug(value);
        public JavaByteCodeWriter WriteDouble(double value) => WriteCountReverse(BitConverter.GetBytes(value)).Debug(value);

        public JavaByteCodeWriter WriteUTF8(string text) => WriteCount(Encoding.UTF8.GetBytes(text)).Debug(text);
        public JavaByteCodeWriter WriteUTF8<T>(string text, Func<T, JavaByteCodeWriter> length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return length.Invoke((T)Convert.ChangeType(bytes.LongLength, typeof(T))).WriteCount(bytes).Debug(text);
        }
        public JavaByteCodeWriter WriteUTF8<T>(string text, Func<JavaByteCodeWriter, T, JavaByteCodeWriter> length)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return length.Invoke(this, (T)(object)bytes.LongLength).WriteCount(bytes).Debug(text);
        }
        public JavaByteCodeWriter Append(JavaByteCodeWriter writer)
        {
            writer.Stream.Position = 0;
            return WriteCount(writer.Stream.ReadAllBytes());
        }
    }
    public class JavaByteCodeReader : JavaByteCode
    {
        public JavaByteCodeReader(Stream stream) : base(stream) { }

        private static T[] Combine<T>(IEnumerable<T[]> arrays)
        {
            T[] bytes = new T[0];
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

        public string ReadUTF8(int count) => System.Text.Encoding.UTF8.GetString(ReadCount(count));

        public byte[] ReadToEnd()
        {
            using MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
