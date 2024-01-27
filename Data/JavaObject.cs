using System;
using System.Threading;
using System.Threading.Tasks;
using Java.Net.Binary;

namespace Java.Net.Data;

public class JavaObject
{
    private readonly Func<JavaByteCodeReader, CancellationToken, ValueTask> reader;
    private readonly Func<JavaByteCodeWriter, CancellationToken, ValueTask<JavaByteCodeWriter>> writer;

    public JavaObject(Func<JavaByteCodeReader, CancellationToken, ValueTask> reader, Func<JavaByteCodeWriter, CancellationToken, ValueTask<JavaByteCodeWriter>> writer)
    {
        this.reader = reader;
        this.writer = writer;
    }
    public JavaObject(Func<JavaByteCodeReader, CancellationToken, ValueTask> reader, Func<JavaByteCodeWriter, CancellationToken, ValueTask> writer) : this(reader, async (a, c) => {
        await writer.Invoke(a, c);
        return a;
    }) { }
    /*
    public static JavaObject Property<T>(
        Func<JavaByteCodeReader, T> reader,
        Func<JavaByteCodeWriter, T, JavaByteCodeWriter> writer,
        Func<T> getter,
        Action<T> setter
    ) => new JavaObject((_reader) => setter.Invoke(reader.Invoke(_reader)), (_writer) => writer.Invoke(_writer, getter.Invoke()));
    */
    public ValueTask ReadAsync(JavaByteCodeReader reader, CancellationToken cancellationToken) => this.reader.Invoke(reader, cancellationToken);
    public ValueTask<JavaByteCodeWriter> WriteAsync(JavaByteCodeWriter writer, CancellationToken cancellationToken) => this.writer.Invoke(writer, cancellationToken);
}