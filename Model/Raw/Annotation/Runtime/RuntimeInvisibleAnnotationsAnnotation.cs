using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Annotation.Runtime.Element;

namespace Java.Net.Model.Raw.Annotation.Runtime;

public sealed class RuntimeInvisibleAnnotationsAnnotation : IAnnotation<RuntimeInvisibleAnnotationsAnnotation>, IRuntimeAnnotations
{
    public override AnnotationType Type => AnnotationType.RuntimeInvisibleAnnotations;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<Element.Annotation> Annotations { get; set; }
}
