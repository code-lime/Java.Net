#nullable enable

using System;
using System.Collections;
using System.Reflection;

namespace Java.Net.Data.Descriptor;

public static class DescriptorUtils
{
    public static string ToDisplay<T>(this T descriptor) where T : IDescriptor => descriptor.GetType().Name + ": " + descriptor.DescriptorFormat;
    public static string ToFormat<T>(this T descriptor) where T : IDescriptor => descriptor.DescriptorFormat;
    public static bool IsEquals<T>(this T descriptor, T other) where T : IDescriptor => descriptor.DescriptorFormat == other.DescriptorFormat;

    public static object? Modify<T>(object? value, Func<T, T> map) where T : IDescriptor
    {
        if (value == null) return null;
        if (value is IDescriptor descriptor) return descriptor.Modify(map);
        if (value is not Array array) return value;
        if (array.Rank != 1) return value;
        int length = array.Length;
        for (int i = 0; i < length; i++)
            array.SetValue(Modify(array.GetValue(i), map), i);
        return array;
    }
    public static IDescriptor Modify<T>(this IDescriptor descriptor, Func<T, T> map) where T : IDescriptor
    {
        if (descriptor is T obj)
            return map.Invoke(obj);
        if (descriptor is IDescriptorMutate mutate)
            return mutate.Modify(map);
        if (!IDescriptor.TryGet(descriptor.GetType(), out ClassObject? classObject))
            return descriptor;
        foreach (PropertyInfo propertyInfo in classObject.Properties)
            if (propertyInfo.GetValue(descriptor) is object value)
                propertyInfo.SetValue(descriptor, value is T val ? map.Invoke(val) : Modify(value, map));
        return descriptor;
    }

    public static object? ModifyClone<T>(object? value, Func<T, T> map)
        where T : IDescriptor
        => Modify(DeepClone(value), map);
    public static IDescriptor ModifyClone<T>(this IDescriptor descriptor, Func<T, T> map)
        where T : IDescriptor
        => descriptor.DeepClone().Modify(map);

    public static object? DeepClone(object? value)
    {
        if (value == null) return null;
        if (value is IDescriptor descriptor) return descriptor.DeepClone();
        if (value is not IList list) return value;
        if (list.GetType().GetElementType() is not Type elementType)
            throw new Exception($"Type {list.GetType()} is not array");

        Array array = Array.CreateInstance(elementType, list.Count);
        int length = array.Length;
        for (int i = 0; i < length; i++) array.SetValue(DeepClone(list[i]), i);
        if (list is Array) return array;

        IList newList = (IList)Activator.CreateInstance(list.GetType())!;
        foreach (object obj in array) newList.Add(obj);
        return newList;
    }
    public static IDescriptor DeepClone(this IDescriptor descriptor)
    {
        if (descriptor is IDescriptorMutate mutate)
            return mutate.Clone();
        Type type = descriptor.GetType();
        if (!IDescriptor.TryGetNative(type, out ClassObject? classObject) || Activator.CreateInstance(type, true) is not IDescriptor obj)
            throw new NotSupportedException("Type '" + type.FullName + "' not support DeepClone");
        foreach (PropertyInfo propertyInfo in classObject.Properties)
            propertyInfo.SetValue(obj, DeepClone(propertyInfo.GetValue(descriptor)));
        return obj;
    }

    public static T DeepClone<T>(this T descriptor) where T : IDescriptor
        => (T)DeepClone((IDescriptor)descriptor);
    public static T ModifyClone<T, J>(this T descriptor, Func<J, J> map) where T : IDescriptor where J : IDescriptor
        => (T)ModifyClone((IDescriptor)descriptor, map);
    public static T Modify<T, J>(this T descriptor, Func<J, J> map) where T : IDescriptor where J : IDescriptor
        => (T)Modify((IDescriptor)descriptor, map);
}
