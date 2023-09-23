using System;
using System.Reflection;
using Java.Net.Binary;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;

namespace Java.Net.Data.Data;

public class InstanceOfTagData
{
    public MethodInfo Method { get; }
    public (TagTypeAttribute.Tag tag, bool is_ref)[] Arguments { get; }
    public InstanceOfTagData(MethodInfo method, (TagTypeAttribute.Tag tag, bool is_ref)[] arguments)
    {
        Method = method;
        Arguments = arguments;
    }

    public IRaw InvokeMethod(MethodTag data)
    {
        int length = Arguments.Length;
        object[] args = new object[length];
        for (int i = 0; i < length; i++)
        {
            (TagTypeAttribute.Tag tag, bool _) = Arguments[i];
            args[i] = tag switch
            {
                TagTypeAttribute.Tag.Parent => data.Parent,
                TagTypeAttribute.Tag.Handle => data.Parent?.Handle,
                TagTypeAttribute.Tag.Reader => data.Reader,
                TagTypeAttribute.Tag.IndexOfArray => data.IndexOfArray,
                TagTypeAttribute.Tag.LastOfArray => data.LastOfArray,
                _ => new ArgumentException($"TagType '{Arguments[i]}' not supported!")
            };
        }
        IRaw java = (IRaw)Method.Invoke(null, args);
        for (int i = 0; i < length; i++)
        {
            (TagTypeAttribute.Tag tag, bool is_ref) = Arguments[i];
            if (!is_ref) continue;
            object item = args[i];
            switch (tag)
            {
                case TagTypeAttribute.Tag.Parent: data.Parent = (IRaw)item; break;
                case TagTypeAttribute.Tag.Handle: data.Parent.SetHandle((JavaClass)item); break;
                case TagTypeAttribute.Tag.Reader: data.Reader = (JavaByteCodeReader)item; break;
                case TagTypeAttribute.Tag.IndexOfArray: data.IndexOfArray = (int)item; break;
                case TagTypeAttribute.Tag.LastOfArray: data.LastOfArray = (IRaw)item; break;
            }
        }
        return java;
    }

    public static InstanceOfTagData Of(MethodInfo method)
    {
        if (method.GetCustomAttribute<InstanceOfTagAttribute>() == null) return null;
        if (!method.DeclaringType.IsAssignableFrom(method.ReturnParameter.ParameterType)) return null;

        ParameterInfo[] list = method.GetParameters();
        int length;
        (TagTypeAttribute.Tag tag, bool is_ref)[] arguments = new (TagTypeAttribute.Tag tag, bool is_ref)[length = list.Length];
        for (int i = 0; i < length; i++)
        {
            TagTypeAttribute attribute = list[i].GetCustomAttribute<TagTypeAttribute>();
            if (attribute == null) return null;
            arguments[i] = (attribute.Type, list[i].ParameterType.IsByRef);
        }
        return new InstanceOfTagData(method, arguments);
    }
}
