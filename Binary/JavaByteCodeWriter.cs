using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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


    public event Action<JavaByteCodeWriter, Handle>? OnUpdate;

    protected T Debug<T>(T obj, Handle value)
    {
        Debug(value);
        return obj;
    }
    protected JavaByteCodeWriter Debug(Handle value)
    {
        OnUpdate?.Invoke(this, value);
        return this;
    }
    protected T Debug<T>(T obj, string method, object value) => Debug(obj, new Handle(method, value));
    protected JavaByteCodeWriter Debug(string method, object value) => Debug(new Handle(method, value));
    protected T Debug<T>(T obj, object value) => Debug(obj, new Handle(value.GetType().Name, value));
    protected JavaByteCodeWriter Debug(object value) => Debug(new Handle(value.GetType().Name, value));

    public JavaByteCodeWriter(Stream stream) : base(stream) { }

    public async ValueTask<JavaByteCodeWriter> WriteCountAsync(byte[] data, CancellationToken cancellationToken)
    {
        await stream.WriteAsync(data, cancellationToken);
        return this;
    }
    private ValueTask<JavaByteCodeWriter> WriteCountReverseAsync(byte[] data, CancellationToken cancellationToken)
    {
        data = (byte[])data.Clone();
        Array.Reverse(data);
        return WriteCountAsync(data, cancellationToken);
    }

    public async ValueTask<JavaByteCodeWriter> WriteByteAsync(byte value, CancellationToken cancellationToken)
    {
        await stream.WriteAsync(new byte[] { value }, cancellationToken);
        return Debug(value);
    }

    public async ValueTask<JavaByteCodeWriter> WriteUShortAsync(ushort value, CancellationToken cancellationToken) => (await WriteCountReverseAsync(BitConverter.GetBytes(value), cancellationToken)).Debug(value);
    public async ValueTask<JavaByteCodeWriter> WriteUIntAsync(uint value, CancellationToken cancellationToken) => (await WriteCountReverseAsync(BitConverter.GetBytes(value), cancellationToken)).Debug(value);

    public async ValueTask<JavaByteCodeWriter> WriteShortAsync(short value, CancellationToken cancellationToken) => (await WriteCountReverseAsync(BitConverter.GetBytes(value), cancellationToken)).Debug(value);
    public async ValueTask<JavaByteCodeWriter> WriteIntAsync(int value, CancellationToken cancellationToken) => (await WriteCountReverseAsync(BitConverter.GetBytes(value), cancellationToken)).Debug(value);
    public async ValueTask<JavaByteCodeWriter> WriteLongAsync(long value, CancellationToken cancellationToken) => (await WriteCountReverseAsync(BitConverter.GetBytes(value), cancellationToken)).Debug(value);

    public async ValueTask<JavaByteCodeWriter> WriteFloatAsync(float value, CancellationToken cancellationToken) => (await WriteCountReverseAsync(BitConverter.GetBytes(value), cancellationToken)).Debug(value);
    public async ValueTask<JavaByteCodeWriter> WriteDoubleAsync(double value, CancellationToken cancellationToken) => (await WriteCountReverseAsync(BitConverter.GetBytes(value), cancellationToken)).Debug(value);

    public async ValueTask<JavaByteCodeWriter> WriteUTF8Async(string text, CancellationToken cancellationToken) => (await WriteCountAsync(Encoding.UTF8.GetBytes(text), cancellationToken)).Debug(text);
    public async ValueTask<JavaByteCodeWriter> WriteUTF8Async<T>(string text, Func<T, CancellationToken, ValueTask<JavaByteCodeWriter>> length, CancellationToken cancellationToken)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        return (await (await length.Invoke((T)Convert.ChangeType(bytes.LongLength, typeof(T)), cancellationToken)).WriteCountAsync(bytes, cancellationToken)).Debug(text);
    }
    public async ValueTask<JavaByteCodeWriter> WriteUTF8Async<T>(string text, Func<JavaByteCodeWriter, T, CancellationToken, ValueTask<JavaByteCodeWriter>> length, CancellationToken cancellationToken)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        return (await (await length.Invoke(this, (T)(object)bytes.LongLength, cancellationToken)).WriteCountAsync(bytes, cancellationToken)).Debug(text);
    }
    public async ValueTask<JavaByteCodeWriter> AppendAsync(JavaByteCodeWriter writer, CancellationToken cancellationToken)
    {
        writer.Stream.Position = 0;
        return await WriteCountAsync(await writer.Stream.ReadAllBytesAsync(cancellationToken), cancellationToken);
    }
}
