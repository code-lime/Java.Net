using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public sealed class Annotation : BaseRaw<Annotation>
{
    [JavaRaw] public ushort TypeIndex { get; set; }
    public Utf8Constant Type
    {
        get => (Utf8Constant)Handle.Constants[TypeIndex];
        set => TypeIndex = Handle.OfConstant(value);
    }
    [JavaRaw][JavaArray(JavaType.UShort)] public List<ElementValuePair> ElementValuePairs { get; set; } = null!;
}
