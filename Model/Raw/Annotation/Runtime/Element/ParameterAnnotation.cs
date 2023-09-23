using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public sealed class ParameterAnnotation : BaseRaw<ParameterAnnotation>
{
    [JavaRaw][JavaArray(JavaType.UShort)] public List<Annotation> Annotations { get; set; }
}
