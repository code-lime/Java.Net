using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Java.Net.Model.Raw.Base;
using Java.Net.Model;
using Java.Net.Data.Data;
using Java.Net.Data.Attribute;
using Java.Net.Binary;
using System.Threading.Tasks;
using System.Threading;

namespace Java.Net.Data;

public interface IRaw : IHandle
{
    JavaRawData JavaData { get; }

    ValueTask ReadPropertyAsync(JavaByteCodeReader reader, PropertyData data, object? value, CancellationToken cancellationToken);
    ValueTask<IRaw> ReadAsync(JavaByteCodeReader reader, CancellationToken cancellationToken);
    ValueTask<JavaByteCodeWriter> WritePropertyAsync(JavaByteCodeWriter writer, PropertyData data, object? value, CancellationToken cancellationToken);
    ValueTask<JavaByteCodeWriter> WriteAsync(JavaByteCodeWriter writer, CancellationToken cancellationToken);

    IRaw SetHandle(JavaClass handle);
    IRaw Load(JavaClass handle, Action<IRaw> action);

    private static readonly Dictionary<Type, JavaType> typeFormats = new Dictionary<Type, JavaType>()
    {
        [typeof(byte)] = JavaType.Byte,
        [typeof(short)] = JavaType.Short,
        [typeof(ushort)] = JavaType.UShort,
        [typeof(uint)] = JavaType.UInt,
        [typeof(int)] = JavaType.Int,
        [typeof(long)] = JavaType.Long,
        [typeof(float)] = JavaType.Float,
        [typeof(double)] = JavaType.Double,
        [typeof(string)] = JavaType.UTF8,
        [typeof(JavaObject)] = JavaType.Custom
    };

    public static JavaType? FindJavaType(PropertyInfo property)
    {
        JavaRawAttribute? attribute = property.GetCustomAttribute<JavaRawAttribute>();
        if (attribute?.Type != JavaType.Auto) return attribute?.Type;
        Type type = property.PropertyType;
        type = type.IsArray
            ? type.GetElementType()!
            : type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)
                ? type.GenericTypeArguments[0]
                : type;

        if (typeFormats.TryGetValue(type, out JavaType _type)) return _type;
        return typeof(IRaw).IsAssignableFrom(type) ? JavaType.Raw : null;
    }

    public static ValueTask<IRaw> ReadAsync(Type type, MethodTag data, JavaByteCodeReader reader, IRaw? def = null, CancellationToken cancellationToken = default)
        => JavaRawData.ReadOfTypeAsync(type, data, reader, def, cancellationToken);
    public static async ValueTask<I> ReadAsync<I>(MethodTag data, JavaByteCodeReader reader, I? def = null, CancellationToken cancellationToken = default) 
        where I : class, IRaw
        => (I)await ReadAsync(typeof(I), data, reader, def, cancellationToken);
    public static async ValueTask<I> ReadAsync<I>(JavaByteCodeReader reader, I? def = null, CancellationToken cancellationToken = default) 
        where I : class, IRaw
        => (I)await ReadAsync(typeof(I), MethodTag.NULL, reader, def, cancellationToken);
    public static ValueTask<I> ReadAsync<I>(IRaw handle, JavaByteCodeReader reader, I? def = null, CancellationToken cancellationToken = default) 
        where I : class, IRaw
        => ReadAsync(MethodTag.Create(reader, handle), reader, def, cancellationToken);
    public static ValueTask<I> ReadArrayAsync<I>(MethodTag data, byte[] bytes, I? def = null, CancellationToken cancellationToken = default)
        where I : class, IRaw
    {
        using MemoryStream stream = new MemoryStream(bytes);
        return ReadAsync(data, new JavaByteCodeReader(stream), def, cancellationToken);
    }
    public static ValueTask<I> ReadArrayAsync<I>(byte[] bytes, I? def = null, CancellationToken cancellationToken = default) 
        where I : class, IRaw
        => ReadArrayAsync(MethodTag.NULL, bytes, def, cancellationToken);
    public static ValueTask<I> ReadArrayAsync<I>(IRaw handle, byte[] bytes, I? def = null, CancellationToken cancellationToken = default) 
        where I : class, IRaw
        => ReadArrayAsync(MethodTag.Create(null, handle), bytes, def, cancellationToken);
    public static ValueTask<IRaw> ReadArrayAsync(Type type, MethodTag data, byte[] bytes, IRaw? def = null, CancellationToken cancellationToken = default)
    {
        using MemoryStream stream = new MemoryStream(bytes);
        return ReadAsync(type, data, new JavaByteCodeReader(stream), def, cancellationToken);
    }

    public static ValueTask<JavaByteCodeWriter> WriteAsync(IRaw raw, JavaByteCodeWriter writer, CancellationToken cancellationToken) => JavaRawData.WriteOfTypeAsync(raw, writer, cancellationToken);
    public static async ValueTask<byte[]> WriteArrayAsync(IRaw raw, CancellationToken cancellationToken)
    {
        using MemoryStream stream = new MemoryStream();
        await raw.WriteAsync(new JavaByteCodeWriter(stream), cancellationToken);
        stream.Position = 0;
        return stream.ToArray();
    }
    public static void SetHandle(IRaw java, JavaClass handle) => java.SetHandle(handle);
}
