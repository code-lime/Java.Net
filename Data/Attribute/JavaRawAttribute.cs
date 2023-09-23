using System;

namespace Java.Net.Data.Attribute;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class JavaRawAttribute : System.Attribute
{
    public int Index { get; }
    public bool IsReaded { get; }
    public bool IsWrited { get; }
    public JavaType Type { get; }
    public JavaRawAttribute(JavaType Type = JavaType.Auto, bool IsReaded = true, bool IsWrited = true, int Index = -1)
    {
        this.Index = Index;
        this.Type = Type;
        this.IsReaded = IsReaded;
        this.IsWrited = IsWrited;
    }
}
