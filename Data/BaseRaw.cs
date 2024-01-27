using System;
using System.Threading;
using System.Threading.Tasks;
using Java.Net.Binary;
using Java.Net.Data.Data;
using Java.Net.Model.Raw.Base;

namespace Java.Net.Data;

public abstract class BaseRaw<I> : IRaw where I : BaseRaw<I>
{
    public JavaClass Handle { get; set; } = null!;
    private JavaRawData? _javaData = null;
    public JavaRawData JavaData => _javaData ??= JavaRawData.OfType(GetType());

    public static I Create(JavaClass handle, Action<I>? apply = null)
    {
        I dat = Activator.CreateInstance<I>();
        dat.SetHandle(handle);
        apply?.Invoke(dat);
        dat.SetHandle(handle);
        return dat;
    }
    public T Create<T>(Action<T>? apply = null) where T : BaseRaw<T> => BaseRaw<T>.Create(Handle, apply);

    public virtual async ValueTask ReadPropertyAsync(JavaByteCodeReader reader, PropertyData data, object? value, CancellationToken cancellationToken)
        => await data.ReaderAsync(reader, new MethodTag() { Parent = this, Reader = reader }, value, cancellationToken);
    public virtual async ValueTask<IRaw> ReadAsync(JavaByteCodeReader reader, CancellationToken cancellationToken)
    {
        foreach (PropertyData data in JavaData.Properties)
            if (data.IsReaded)
                await ReadPropertyAsync(reader, data, data.Property.GetValue(this), cancellationToken);
        return this;
    }
    public virtual ValueTask<JavaByteCodeWriter> WritePropertyAsync(JavaByteCodeWriter writer, PropertyData data, object? value, CancellationToken cancellationToken)
        => data.WriterAsync(writer, this, value, cancellationToken);
    public virtual async ValueTask<JavaByteCodeWriter> WriteAsync(JavaByteCodeWriter writer, CancellationToken cancellationToken)
    {
        foreach (PropertyData data in JavaData.Properties)
            if (data.IsWrited)
                writer = await WritePropertyAsync(writer, data, data.Property.GetValue(this), cancellationToken);
        return writer;
    }

    public virtual IRaw SetHandle(JavaClass handle)
    {
        Handle = handle;
        foreach (PropertyData data in JavaData.Properties)
            if (data.IsReaded)
            {
                object? value = data.Property.GetValue(this);
                if (value is IRaw java) java.SetHandle(handle);
                else if (value is Array array)
                    foreach (var item in array)
                    {
                        if (item is IRaw _java)
                            _java.SetHandle(handle);
                    }
            }
        return this;
    }
    public virtual IRaw Load(JavaClass handle, Action<IRaw>? action)
    {
        SetHandle(handle);
        action?.Invoke(this);
        return this;
    }
}
