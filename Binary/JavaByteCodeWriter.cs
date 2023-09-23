using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;

namespace Java.Net.Binary;

public class JavaByteCodeWriter : JavaByteCode
{
    protected readonly JArray json = new JArray();
    public class Handle
    {
        public string Method { get; }
        public object Value { get; }

        public Handle(string method, object value)
        {
            Method = method;
            Value = value;
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
        return Debug(value);
    }

    public JavaByteCodeWriter WriteUShort(ushort value) => WriteCountReverse(BitConverter.GetBytes(value)).Debug(value);
    public JavaByteCodeWriter WriteUInt(uint value) => WriteCountReverse(BitConverter.GetBytes(value)).Debug(value);

    public JavaByteCodeWriter WriteShort(short value) => WriteCountReverse(BitConverter.GetBytes(value)).Debug(value);
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
