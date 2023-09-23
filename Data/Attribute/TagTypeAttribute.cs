using System;

namespace Java.Net.Data.Attribute;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class TagTypeAttribute : System.Attribute
{
    public Tag Type { get; }
    public enum Tag
    {
        Parent,
        Handle,
        Reader,
        IndexOfArray,
        LastOfArray
    }
    public TagTypeAttribute(Tag type) { Type = type; }
}
