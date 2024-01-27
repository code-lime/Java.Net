using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using Java.Net.Data.Attribute;
using Java.Net.Binary;
using System.Threading.Tasks;
using System.Threading;

namespace Java.Net.Data.Data;

public class PropertyData
{
    protected static async ValueTask<long> ReadLengthAsync(JavaByteCodeReader reader, JavaType type, CancellationToken cancellationToken) => type switch
    {
        JavaType.Byte => await reader.ReadByteAsync(cancellationToken),
        JavaType.UShort => await reader.ReadUShortAsync(cancellationToken),
        JavaType.UInt => await reader.ReadUIntAsync(cancellationToken),
        JavaType.Short => await reader.ReadShortAsync(cancellationToken),
        JavaType.Int => await reader.ReadIntAsync(cancellationToken),
        JavaType.Long => await reader.ReadLongAsync(cancellationToken),
        _ => throw new TypeAccessException($"Type of length '{type}' not supported!"),
    };
    protected static ValueTask<JavaByteCodeWriter> WriteLengthAsync(JavaByteCodeWriter reader, JavaType type, long value, CancellationToken cancellationToken) => type switch
    {
        JavaType.Byte => reader.WriteByteAsync((byte)value, cancellationToken),
        JavaType.UShort => reader.WriteUShortAsync((ushort)value, cancellationToken),
        JavaType.UInt => reader.WriteUIntAsync((uint)value, cancellationToken),
        JavaType.Short => reader.WriteShortAsync((short)value, cancellationToken),
        JavaType.Int => reader.WriteIntAsync((int)value, cancellationToken),
        JavaType.Long => reader.WriteLongAsync(value, cancellationToken),
        _ => throw new TypeAccessException($"Type of length '{type}' not supported!"),
    };
    protected static void ResizeArray(ref Array oldArray, long newSize)
    {
        long oldSize = oldArray.LongLength;
        Type elementType = oldArray.GetType().GetElementType()!;
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

    private void InvokeMethod(string method, Type[] types, IRaw? raw, object?[] args)
        => Property.DeclaringType!.GetMethod(method,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static,
            null, types, null)!.Invoke(raw, args);
    private void InvokeDataRead(JavaByteCodeReader reader, IRaw? raw, object? obj, bool isLast, JavaInvokeAttribute.TypeInvoke type)
    {
        foreach (var data in InvokeData)
        {
            if (!data.IsRead) continue;
            if (data.IsLast != isLast || data.Type != type) continue;
            InvokeMethod(
                data.Invoke,
                new Type[] { typeof(JavaByteCodeReader), typeof(object) },
                raw,
                new object?[] { reader, obj }
            );
        }
    }
    private JavaByteCodeWriter InvokeDataWrite(JavaByteCodeWriter writer, IRaw? raw, object? obj, bool isLast, JavaInvokeAttribute.TypeInvoke type)
    {
        foreach (var data in InvokeData)
        {
            if (data.IsRead) continue;
            if (data.IsLast != isLast || data.Type != type) continue;
            InvokeMethod(
                data.Invoke,
                new Type[] { typeof(JavaByteCodeWriter), typeof(object) },
                raw,
                new object?[] { writer, obj }
            );
        }
        return writer;
    }

    private void InvokeDataRead(JavaByteCodeReader reader, IRaw? raw, object? obj, bool isLast, ref int index)
    {
        foreach (var data in InvokeData)
        {
            if (!data.IsRead) continue;
            if (data.IsLast != isLast || data.Type != JavaInvokeAttribute.TypeInvoke.ArrayElement) continue;
            object?[] args = new object?[] { reader, obj, index };
            InvokeMethod(
                data.Invoke,
                new Type[] { typeof(JavaByteCodeReader), typeof(object), typeof(int).MakeByRefType() },
                raw,
                args
            );
            index = (int)args[2]!;
        }
    }
    private JavaByteCodeWriter InvokeDataWrite(JavaByteCodeWriter writer, IRaw? raw, object? obj, bool isLast, ref int index)
    {
        foreach (var data in InvokeData)
        {
            if (data.IsRead) continue;
            if (data.IsLast != isLast || data.Type != JavaInvokeAttribute.TypeInvoke.ArrayElement) continue;
            object?[] args = new object?[] { writer, obj, index };
            InvokeMethod(
                data.Invoke,
                new Type[] { typeof(JavaByteCodeWriter), typeof(object), typeof(int).MakeByRefType() },
                raw,
                args
            );
            index = (int)args[2]!;
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
            ? propertyType.GetElementType()!
            : propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>)
                ? propertyType.GenericTypeArguments[0]
                : propertyType;

        InvokeData.AddRange(property.GetCustomAttributes<JavaInvokeAttribute>());
    }

    protected virtual ValueTask<long?> ReadLengthAsync(JavaByteCodeReader reader, CancellationToken cancellationToken) => ValueTask.FromResult<long?>(null);
    protected virtual ValueTask WriteLengthAsync(JavaByteCodeWriter writer, long length, CancellationToken cancellationToken) => ValueTask.CompletedTask;

    public virtual async ValueTask<object?> ReaderAsync(JavaByteCodeReader reader, MethodTag data, object? value, CancellationToken cancellationToken)
    {
        if (!IsArray) return await ReadElementAsync(reader, value ?? ElementType.GetConstructor(Array.Empty<Type>())?.Invoke(Array.Empty<object>())!, data.Parent, data, cancellationToken);

        long? _length = await ReadLengthAsync(reader, cancellationToken);
        IList arr;
        if (value == null)
        {
            arr = Array.CreateInstance(ElementType, _length ?? throw new ArgumentNullException($"Can't create default array of type '{ElementType.FullName}'"));
            if (!Property.PropertyType.IsArray) arr = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ElementType), arr)!;
        }
        else
        {
            arr = (IList)value;
        }
        int length = arr.Count;
        InvokeDataRead(reader, data.Parent, arr, false, JavaInvokeAttribute.TypeInvoke.Array);
        IRaw? last = null;
        for (int i = 0; i < length; i++)
        {
            InvokeDataRead(reader, data.Parent, arr, false, ref i);
            MethodTag itemData = data.Copy()
                .Edit(v => v.LastOfArray = last)
                .Edit(v => v.IndexOfArray = i);
            object? item = await ReadElementAsync(reader, arr[i]!, null, itemData, cancellationToken);
            if (item is IRaw _last) last = _last;
            arr[i] = item;
            i = itemData.IndexOfArray;
            InvokeDataRead(reader, data.Parent, arr, true, ref i);
        }
        InvokeDataRead(reader, data.Parent, arr, true, JavaInvokeAttribute.TypeInvoke.Array);

        Property.SetValue(data.Parent, arr);
        return arr;
    }
    private async ValueTask<object?> ReadElementAsync(JavaByteCodeReader reader, object? obj, IRaw? raw, MethodTag data, CancellationToken cancellationToken)
    {
        InvokeDataRead(reader, raw, obj, false, JavaInvokeAttribute.TypeInvoke.Element);
        if (PropertyType == JavaType.Custom) await ((JavaObject)obj!).ReadAsync(reader, cancellationToken);
        else if (Property.SetMethod != null)
        {
            object value = CastType(ElementType, PropertyType switch
            {
                JavaType.Byte => await reader.ReadByteAsync(cancellationToken),
                JavaType.UShort => await reader.ReadUShortAsync(cancellationToken),
                JavaType.UInt => await reader.ReadUIntAsync(cancellationToken),
                JavaType.Short => await reader.ReadShortAsync(cancellationToken),
                JavaType.Int => await reader.ReadIntAsync(cancellationToken),
                JavaType.Long => await reader.ReadLongAsync(cancellationToken),
                JavaType.Float => await reader.ReadFloatAsync(cancellationToken),
                JavaType.Double => await reader.ReadDoubleAsync(cancellationToken),
                JavaType.UTF8 => await reader.ReadUTF8Async(await reader.ReadUShortAsync(cancellationToken), cancellationToken),
                JavaType.Raw => await IRaw.ReadAsync(ElementType, data, reader, (IRaw?)obj, cancellationToken),
                _ => throw new TypeAccessException($"Type '{Property}' in property '{Property.DeclaringType!.FullName}.{Property.Name}' not supported!"),
            });
            if (raw != null) Property.SetValue(raw, value);
            obj = value;
        }
        InvokeDataRead(reader, raw, obj, true, JavaInvokeAttribute.TypeInvoke.Element);
        return obj;
    }
    public virtual async ValueTask<JavaByteCodeWriter> WriterAsync(JavaByteCodeWriter writer, IRaw raw, object? value, CancellationToken cancellationToken)
    {
        if (!IsArray) return await WriteElementAsync(writer, raw, value!, cancellationToken);
        IList arr = (IList)value!;
        int length = arr.Count;
        await WriteLengthAsync(writer, length, cancellationToken);
        writer = InvokeDataWrite(writer, raw, value, false, JavaInvokeAttribute.TypeInvoke.Array);
        for (int i = 0; i < length; i++)
        {
            InvokeDataWrite(writer, raw, arr, false, ref i);
            writer = await WriteElementAsync(writer, raw, arr[i]!, cancellationToken);
            InvokeDataWrite(writer, raw, arr, true, ref i);
        }
        writer = InvokeDataWrite(writer, raw, value, true, JavaInvokeAttribute.TypeInvoke.Array);
        return writer;
    }
    private async ValueTask<JavaByteCodeWriter> WriteElementAsync(JavaByteCodeWriter writer, IRaw? raw, object value, CancellationToken cancellationToken)
    {
        writer = InvokeDataWrite(writer, raw, value, false, JavaInvokeAttribute.TypeInvoke.Element);
        return InvokeDataWrite(PropertyType switch
        {
            JavaType.Byte => await writer.WriteByteAsync(CastType<byte>(value), cancellationToken),
            JavaType.UShort => await writer.WriteUShortAsync(CastType<ushort>(value), cancellationToken),
            JavaType.Short => await writer.WriteShortAsync(CastType<short>(value), cancellationToken),
            JavaType.UInt => await writer.WriteUIntAsync(CastType<uint>(value), cancellationToken),
            JavaType.Int => await writer.WriteIntAsync(CastType<int>(value), cancellationToken),
            JavaType.Long => await writer.WriteLongAsync(CastType<long>(value), cancellationToken),
            JavaType.Float => await writer.WriteFloatAsync(CastType<float>(value), cancellationToken),
            JavaType.Double => await writer.WriteDoubleAsync(CastType<double>(value), cancellationToken),
            JavaType.UTF8 => await writer.WriteUTF8Async<ushort>((string)value, writer.WriteUShortAsync, cancellationToken),
            JavaType.Raw => await ((IRaw)value).WriteAsync(writer, cancellationToken),
            JavaType.Custom => await ((JavaObject)value).WriteAsync(writer, cancellationToken),
            _ => throw new TypeAccessException($"Type '{PropertyType}' in property '{Property.DeclaringType!.FullName}.{Property.Name}' not supported!"),
        }, raw, value, true, JavaInvokeAttribute.TypeInvoke.Element);
    }

    public override string ToString() => PropertyType + (IsArray ? "[]" : "") + " " + PropertyName;

    public static PropertyData? Of(PropertyInfo property)
    {
        JavaType? _type = IRaw.FindJavaType(property);
        JavaRawAttribute? attr = property.GetCustomAttribute<JavaRawAttribute>();
        if (_type is null || attr is null) return null;
        JavaArrayAttribute? arr = property.GetCustomAttribute<JavaArrayAttribute>();
        return arr == null
            ? new PropertyData(property, new JavaRawAttribute(_type.Value, attr.IsReaded, attr.IsWrited, attr.Index))
            : new ArrayPropertyData(property, new JavaRawAttribute(_type.Value, attr.IsReaded, attr.IsWrited, attr.Index), arr);
    }
}
