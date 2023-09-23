using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Java.Net.Model.Raw.Base;
using Java.Net.Model;
using Java.Net.Data.Data;
using Java.Net.Data.Attribute;
using Java.Net.Binary;

namespace Java.Net.Data;

public interface IRaw : IHandle
{
    JavaRawData JavaData { get; }

    void ReadProperty(JavaByteCodeReader reader, PropertyData data, object value);
    IRaw Read(JavaByteCodeReader reader);
    JavaByteCodeWriter WriteProperty(JavaByteCodeWriter writer, PropertyData data, object value);
    JavaByteCodeWriter Write(JavaByteCodeWriter writer);

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
        return typeof(IRaw).IsAssignableFrom(type) ? JavaType.IJava : null;
    }

    public static IRaw Read(Type type, MethodTag data, JavaByteCodeReader reader, IRaw? def = null) => JavaRawData.ReadOfType(type, data, reader, def);
    public static I Read<I>(MethodTag data, JavaByteCodeReader reader, I? def = null) where I : class, IRaw => (I)Read(typeof(I), data, reader, def);
    public static I Read<I>(JavaByteCodeReader reader, I? def = null) where I : class, IRaw => (I)Read(typeof(I), null, reader, def);
    public static I Read<I>(IRaw handle, JavaByteCodeReader reader, I? def = null) where I : class, IRaw => Read(MethodTag.Create(reader, handle), reader, def);
    public static I ReadArray<I>(MethodTag data, byte[] bytes, I? def = null) where I : class, IRaw
    {
        using MemoryStream stream = new MemoryStream(bytes);
        return Read(data, new JavaByteCodeReader(stream), def);
    }
    public static I ReadArray<I>(byte[] bytes, I? def = null) where I : class, IRaw => ReadArray(MethodTag.NULL, bytes, def);
    public static I ReadArray<I>(IRaw handle, byte[] bytes, I? def = null) where I : class, IRaw => ReadArray(MethodTag.Create(null, handle), bytes, def);
    public static IRaw ReadArray(Type type, MethodTag data, byte[] bytes, IRaw? def = null)
    {
        using MemoryStream stream = new MemoryStream(bytes);
        return Read(type, data, new JavaByteCodeReader(stream), def);
    }

    public static JavaByteCodeWriter Write(IRaw obj, JavaByteCodeWriter writer) => JavaRawData.WriteOfType(obj, writer);
    public static byte[] WriteArray(IRaw java)
    {
        using MemoryStream stream = new MemoryStream();
        java.Write(new JavaByteCodeWriter(stream));
        stream.Position = 0;
        return stream.ToArray();
    }
    public static void SetHandle(IRaw java, JavaClass handle) => java.SetHandle(handle);
}
