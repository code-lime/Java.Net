using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public sealed class ArrayValue : ElementValue<ArrayValue>
{
    [JavaRaw][JavaArray(JavaType.UShort)] public List<IElementValue> Values { get; set; } = null!;
}
