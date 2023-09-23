using System;
using Java.Net.Data;

namespace Java.Net.Data.Attribute;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class JavaArrayAttribute : System.Attribute
{
    public JavaType LengthType { get; }
    public JavaArrayAttribute(JavaType LengthType = JavaType.UShort) => this.LengthType = LengthType;
}
