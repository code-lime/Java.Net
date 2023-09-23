using System;
using Java.Net.Binary;

namespace Java.Net.Data;

public class JavaObject
{
    private readonly Action<JavaByteCodeReader> reader;
    private readonly Func<JavaByteCodeWriter, JavaByteCodeWriter> writer;

    public JavaObject(Action<JavaByteCodeReader> reader, Func<JavaByteCodeWriter, JavaByteCodeWriter> writer)
    {
        this.reader = reader;
        this.writer = writer;
    }
    public JavaObject(Action<JavaByteCodeReader> reader, Action<JavaByteCodeWriter> writer) : this(reader, (a) => { writer.Invoke(a); return a; }) { }

    public static JavaObject Property<T>(
        Func<JavaByteCodeReader, T> reader,
        Func<JavaByteCodeWriter, T, JavaByteCodeWriter> writer,
        Func<T> getter,
        Action<T> setter
    ) => new JavaObject((_reader) => setter.Invoke(reader.Invoke(_reader)), (_writer) => writer.Invoke(_writer, getter.Invoke()));

    public void Read(JavaByteCodeReader reader) => this.reader.Invoke(reader);
    public JavaByteCodeWriter Write(JavaByteCodeWriter writer) => this.writer.Invoke(writer);
}