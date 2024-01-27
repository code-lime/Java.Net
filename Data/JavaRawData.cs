using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using Java.Net.Model.Raw.Base;
using Java.Net.Data.Data;
using Java.Net.Binary;
using System.Threading.Tasks;
using System.Threading;

namespace Java.Net.Data;

public class JavaRawData
{
    public List<PropertyData> Properties { get; } = new List<PropertyData>();
    public InstanceOfTagData? InstanceOfTag { get; }
    public Type Type { get; }

    private static IEnumerable<PropertyInfo> GetProperties(Type? type)
    {
        if (type == null) yield break;
        foreach (var prop in GetProperties(type.BaseType)) yield return prop;
        foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            if (prop.DeclaringType == type)
                yield return prop;
    }
    private static IEnumerable<MethodInfo> GetMethods(Type? type)
    {
        if (type == null) yield break;
        foreach (var meth in GetMethods(type.BaseType))
            yield return meth;
        foreach (var interfaceType in type.GetInterfaces())
            foreach (var meth in GetMethods(interfaceType))
                yield return meth;
        foreach (MethodInfo meth in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            if (meth.DeclaringType == type)
                yield return meth;
    }

    public JavaRawData(Type type)
    {
        Type = type;
        foreach (MethodInfo method in GetMethods(type))
        {
            InstanceOfTagData? data = InstanceOfTagData.Of(method);
            if (data == null) continue;
            InstanceOfTag = data;
        }

        Dictionary<string, PropertyData> props = new Dictionary<string, PropertyData>();
        foreach (PropertyInfo prop in GetProperties(type))
        {
            PropertyData? data = PropertyData.Of(prop);
            if (data == null) continue;
            props[data.PropertyName] = data;
        }
        Properties.AddRange(props.Values);
    }

    private static readonly ConcurrentDictionary<Type, JavaRawData> classes = new ConcurrentDictionary<Type, JavaRawData>();
    public static JavaRawData OfType(Type type)
    {
        lock (classes) return classes.TryGetValue(type, out JavaRawData? _data) ? _data : classes[type] = new JavaRawData(type);
    }

    public static async ValueTask<IRaw> ReadOfTypeAsync(Type type, MethodTag data, JavaByteCodeReader reader, IRaw? def, CancellationToken cancellationToken)
    {
        def ??= await (OfType(type).InstanceOfTag?.InvokeMethod(data, cancellationToken) ?? ValueTask.FromResult<IRaw?>(null)) ?? (IRaw)Activator.CreateInstance(type)!;
        return await (def is JavaClass j ? def.SetHandle(j) : def.SetHandle(data.Parent?.Handle!)).ReadAsync(reader, cancellationToken);
    }

    public static ValueTask<JavaByteCodeWriter> WriteOfTypeAsync(IRaw obj, JavaByteCodeWriter writer, CancellationToken cancellationToken) => obj.WriteAsync(writer, cancellationToken);
}
