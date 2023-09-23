using System;

namespace Java.Net.Data.Attribute;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class JavaInvokeAttribute : System.Attribute
{
    public enum TypeInvoke
    {
        Element,
        Array,
        ArrayElement
    }

    public string Invoke { get; }
    public bool IsRead { get; }
    public bool IsLast { get; }
    public TypeInvoke Type { get; }
    public JavaInvokeAttribute(string Invoke, bool IsRead, bool IsLast = true, TypeInvoke Type = TypeInvoke.Element)
    {
        this.Invoke = Invoke;
        this.IsRead = IsRead;
        this.IsLast = IsLast;
        this.Type = Type;
    }
}
