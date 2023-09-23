using System;
using Java.Net.Binary;
using Java.Net.Data.Data;
using Java.Net.Model.Raw.Base;

namespace Java.Net.Data;

public abstract class BaseRaw<I> : IRaw where I : BaseRaw<I>
{
    public JavaClass Handle { get; set; }
    private JavaRawData _javaData;
    public JavaRawData JavaData => _javaData ??= JavaRawData.OfType(GetType());

    public static I Create(JavaClass handle, Action<I> apply = null)
    {
        I dat = Activator.CreateInstance<I>();
        dat.SetHandle(handle);
        apply?.Invoke(dat);
        dat.SetHandle(handle);
        return dat;
    }
    public T Create<T>(Action<T> apply = null) where T : BaseRaw<T> => BaseRaw<T>.Create(Handle, apply);

    public virtual void ReadProperty(JavaByteCodeReader reader, PropertyData data, object value) => data.Reader(reader, new MethodTag() { Parent = this, Reader = reader }, value);
    public virtual IRaw Read(JavaByteCodeReader reader)
    {
        foreach (PropertyData data in JavaData.Properties)
            if (data.IsReaded)
                ReadProperty(reader, data, data.Property.GetValue(this));
        return this;
    }
    public virtual JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, PropertyData data, object value) => data.Writer(writer, this, value);
    public virtual JavaByteCodeWriter Write(JavaByteCodeWriter writer)
    {
        foreach (PropertyData data in JavaData.Properties)
            if (data.IsWrited)
                writer = WriteProperty(writer, data, data.Property.GetValue(this));
        return writer;
    }

    public virtual IRaw SetHandle(JavaClass handle)
    {
        Handle = handle;
        foreach (PropertyData data in JavaData.Properties)
            if (data.IsReaded)
            {
                object value = data.Property.GetValue(this);
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
    public virtual IRaw Load(JavaClass handle, Action<IRaw> action)
    {
        SetHandle(handle);
        action?.Invoke(this);
        return this;
    }
}
