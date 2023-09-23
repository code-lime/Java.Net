using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;

namespace Java.Net.Model.Raw.Annotation;

public sealed class ExceptionsAnnotation : IAnnotation<ExceptionsAnnotation>
{
    public override AnnotationType Type => AnnotationType.Exceptions;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<ushort> ExceptionIndexTable { get; set; }
}
