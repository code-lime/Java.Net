#nullable enable
using System;

namespace Java.Net.Data.Descriptor;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RegexStructAttribute : System.Attribute
{
    public string[] Format { get; }
    public RegexStructAttribute(params string[] format) => Format = format;
}
