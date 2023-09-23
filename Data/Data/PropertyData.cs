using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using Java.Net.Data.Attribute;
using Java.Net.Binary;

namespace Java.Net.Data.Data;

public class PropertyData
{
    protected static long ReadLength(JavaByteCodeReader reader, JavaType type) => type switch
    {
        JavaType.Byte => reader.ReadByte(),
        JavaType.UShort => reader.ReadUShort(),
        JavaType.UInt => reader.ReadUInt(),
        JavaType.Short => reader.ReadShort(),
        JavaType.Int => reader.ReadInt(),
        JavaType.Long => reader.ReadLong(),
        _ => throw new TypeAccessException($"Type of length '{type}' not supported!"),
    };
    protected static JavaByteCodeWriter WriteLength(JavaByteCodeWriter reader, JavaType type, long value) => type switch
    {
        JavaType.Byte => reader.WriteByte((byte)value),
        JavaType.UShort => reader.WriteUShort((ushort)value),
        JavaType.UInt => reader.WriteUInt((uint)value),
        JavaType.Short => reader.WriteShort((short)value),
        JavaType.Int => reader.WriteInt((int)value),
        JavaType.Long => reader.WriteLong(value),
        _ => throw new TypeAccessException($"Type of length '{type}' not supported!"),
    };
    protected static void ResizeArray(ref Array oldArray, long newSize)
    {
        long oldSize = oldArray.LongLength;
        Type elementType = oldArray.GetType().GetElementType();
        Array newArray = Array.CreateInstance(elementType, newSize);
        long preserveLength = Math.Min(oldSize, newSize);
        if (preserveLength > 0) Array.Copy(oldArray, newArray, preserveLength);
        oldArray = newArray;
    }
    protected static object CastType(Type type, object value) => type.IsEnum
        ? Enum.ToObject(type, value)
        : value.GetType().IsEnum
            ? Convert.ChangeType(Convert.ChangeType(value, ((Enum)value).GetTypeCode()), type)
            : value
        ;
    protected static T CastType<T>(object value) => (T)CastType(typeof(T), value);

    public PropertyInfo Property { get; }
    public JavaType PropertyType { get; }
    public Type ElementType { get; }
    public bool IsArray { get; }

    public bool IsReaded { get; }
    public bool IsWrited { get; }

    public string PropertyName { get; }
    public int Index { get; }

    public List<JavaInvokeAttribute> InvokeData { get; } = new List<JavaInvokeAttribute>();

    private void InvokeMethod(string method, Type[] types, IRaw ijava, object[] args)
    {
        Property.DeclaringType.GetMethod(
            method,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
            null,
            types,
            null).Invoke(ijava, args);
    }
    private void InvokeDataRead(JavaByteCodeReader reader, IRaw ijava, object obj, bool isLast, JavaInvokeAttribute.TypeInvoke type)
    {
        foreach (var data in InvokeData)
        {
            if (!data.IsRead) continue;
            if (data.IsLast != isLast || data.Type != type) continue;
            InvokeMethod(
                data.Invoke,
                new Type[] { typeof(JavaByteCodeReader), typeof(object) },
                ijava,
                new object[] { reader, obj }
            );
        }
    }
    private JavaByteCodeWriter InvokeDataWrite(JavaByteCodeWriter writer, IRaw ijava, object obj, bool isLast, JavaInvokeAttribute.TypeInvoke type)
    {
        foreach (var data in InvokeData)
        {
            if (data.IsRead) continue;
            if (data.IsLast != isLast || data.Type != type) continue;
            InvokeMethod(
                data.Invoke,
                new Type[] { typeof(JavaByteCodeWriter), typeof(object) },
                ijava,
                new object[] { writer, obj }
            );
        }
        return writer;
    }

    private void InvokeDataRead(JavaByteCodeReader reader, IRaw ijava, object obj, bool isLast, ref int index)
    {
        foreach (var data in InvokeData)
        {
            if (!data.IsRead) continue;
            if (data.IsLast != isLast || data.Type != JavaInvokeAttribute.TypeInvoke.ArrayElement) continue;
            object[] args = new object[] { reader, obj, index };
            InvokeMethod(
                data.Invoke,
                new Type[] { typeof(JavaByteCodeReader), typeof(object), typeof(int).MakeByRefType() },
                ijava,
                args
            );
            index = (int)args[2];
        }
    }
    private JavaByteCodeWriter InvokeDataWrite(JavaByteCodeWriter writer, IRaw ijava, object obj, bool isLast, ref int index)
    {
        foreach (var data in InvokeData)
        {
            if (data.IsRead) continue;
            if (data.IsLast != isLast || data.Type != JavaInvokeAttribute.TypeInvoke.ArrayElement) continue;
            object[] args = new object[] { writer, obj, index };
            InvokeMethod(
                data.Invoke,
                new Type[] { typeof(JavaByteCodeWriter), typeof(object), typeof(int).MakeByRefType() },
                ijava,
                args
            );
            index = (int)args[2];
        }
        return writer;
    }

    public PropertyData(PropertyInfo property, JavaRawAttribute javaAttribute)
    {
        Property = property;
        Index = javaAttribute.Index;
        PropertyType = javaAttribute.Type;
        PropertyName = property.Name;
        IsReaded = javaAttribute.IsReaded;
        IsWrited = javaAttribute.IsWrited;
        Type propertyType = property.PropertyType;
        IsArray = propertyType.IsArray || propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>);
        ElementType = propertyType.IsArray
            ? propertyType.GetElementType()
            : propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>)
                ? propertyType.GenericTypeArguments[0]
                : propertyType;

        InvokeData.AddRange(property.GetCustomAttributes<JavaInvokeAttribute>());
    }

    protected virtual long? ReadLength(JavaByteCodeReader reader) => null;
    protected virtual void WriteLength(JavaByteCodeWriter writer, long length) { }

    public virtual object Reader(JavaByteCodeReader reader, MethodTag data, object value)
    {
        if (!IsArray) return ReadElement(reader, value ?? ElementType.GetConstructor(Array.Empty<Type>())?.Invoke(Array.Empty<object>()), data.Parent, data);

        long? _length = ReadLength(reader);
        IList arr;
        if (value == null)
        {
            arr = Array.CreateInstance(ElementType, _length ?? throw new ArgumentNullException($"Can't create default array of type '{ElementType.FullName}'"));
            if (!Property.PropertyType.IsArray) arr = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ElementType), arr);
        }
        else
        {
            arr = (IList)value;
        }
        int length = arr.Count;
        InvokeDataRead(reader, data.Parent, arr, false, JavaInvokeAttribute.TypeInvoke.Array);
        IRaw last = null;
        for (int i = 0; i < length; i++)
        {
            InvokeDataRead(reader, data.Parent, arr, false, ref i);
            MethodTag itemData = data.Copy()
                .Edit(v => v.LastOfArray = last)
                .Edit(v => v.IndexOfArray = i);
            object item = ReadElement(reader, arr[i], null, itemData);
            if (item is IRaw _last) last = _last;
            arr[i] = item;
            i = itemData.IndexOfArray;
            InvokeDataRead(reader, data.Parent, arr, true, ref i);
        }
        InvokeDataRead(reader, data.Parent, arr, true, JavaInvokeAttribute.TypeInvoke.Array);

        Property.SetValue(data.Parent, arr);
        return arr;
    }
    private object ReadElement(JavaByteCodeReader reader, object obj, IRaw ijava, MethodTag data)
    {
        InvokeDataRead(reader, ijava, obj, false, JavaInvokeAttribute.TypeInvoke.Element);
        if (PropertyType == JavaType.Custom) ((JavaObject)obj).Read(reader);
        else if (Property.SetMethod != null)
        {
            object value = CastType(ElementType, PropertyType switch
            {
                JavaType.Byte => reader.ReadByte(),
                JavaType.UShort => reader.ReadUShort(),
                JavaType.UInt => reader.ReadUInt(),
                JavaType.Short => reader.ReadShort(),
                JavaType.Int => reader.ReadInt(),
                JavaType.Long => reader.ReadLong(),
                JavaType.Float => reader.ReadFloat(),
                JavaType.Double => reader.ReadDouble(),
                JavaType.UTF8 => reader.ReadUTF8(reader.ReadUShort()),
                JavaType.IJava => IRaw.Read(ElementType, data, reader, (IRaw)obj),
                _ => throw new TypeAccessException($"Type '{Property}' in property '{Property.DeclaringType.FullName}.{Property.Name}' not supported!"),
            });
            if (ijava != null) Property.SetValue(ijava, value);
            obj = value;
        }
        InvokeDataRead(reader, ijava, obj, true, JavaInvokeAttribute.TypeInvoke.Element);
        return obj;
    }
    public virtual JavaByteCodeWriter Writer(JavaByteCodeWriter writer, IRaw ijava, object value)
    {
        if (!IsArray) return WriteElement(writer, ijava, value);
        IList arr = (IList)value;
        int length = arr.Count;
        WriteLength(writer, length);
        writer = InvokeDataWrite(writer, ijava, value, false, JavaInvokeAttribute.TypeInvoke.Array);
        for (int i = 0; i < length; i++)
        {
            InvokeDataWrite(writer, ijava, arr, false, ref i);
            writer = WriteElement(writer, ijava, arr[i]);
            InvokeDataWrite(writer, ijava, arr, true, ref i);
        }
        writer = InvokeDataWrite(writer, ijava, value, true, JavaInvokeAttribute.TypeInvoke.Array);
        return writer;
    }
    private JavaByteCodeWriter WriteElement(JavaByteCodeWriter writer, IRaw ijava, object value)
    {
        writer = InvokeDataWrite(writer, ijava, value, false, JavaInvokeAttribute.TypeInvoke.Element);
        return InvokeDataWrite(PropertyType switch
        {
            JavaType.Byte => writer.WriteByte(CastType<byte>(value)),
            JavaType.UShort => writer.WriteUShort(CastType<ushort>(value)),
            JavaType.Short => writer.WriteShort(CastType<short>(value)),
            JavaType.UInt => writer.WriteUInt(CastType<uint>(value)),
            JavaType.Int => writer.WriteInt(CastType<int>(value)),
            JavaType.Long => writer.WriteLong(CastType<long>(value)),
            JavaType.Float => writer.WriteFloat(CastType<float>(value)),
            JavaType.Double => writer.WriteDouble(CastType<double>(value)),
            JavaType.UTF8 => writer.WriteUTF8<ushort>((string)value, writer.WriteUShort),
            JavaType.IJava => ((IRaw)value).Write(writer),
            JavaType.Custom => ((JavaObject)value).Write(writer),
            _ => throw new TypeAccessException($"Type '{PropertyType}' in property '{Property.DeclaringType.FullName}.{Property.Name}' not supported!"),
        }, ijava, value, true, JavaInvokeAttribute.TypeInvoke.Element);
    }

    public override string ToString() => PropertyType + (IsArray ? "[]" : "") + " " + PropertyName;

    public static PropertyData Of(PropertyInfo property)
    {
        JavaType? _type = IRaw.FindJavaType(property);
        JavaRawAttribute attr = property.GetCustomAttribute<JavaRawAttribute>();
        if (_type == null) return null;
        JavaArrayAttribute arr = property.GetCustomAttribute<JavaArrayAttribute>();
        return arr == null
            ? new PropertyData(property, new JavaRawAttribute(_type.Value, attr.IsReaded, attr.IsWrited, attr.Index))
            : new ArrayPropertyData(property, new JavaRawAttribute(_type.Value, attr.IsReaded, attr.IsWrited, attr.Index), arr);
    }
}
