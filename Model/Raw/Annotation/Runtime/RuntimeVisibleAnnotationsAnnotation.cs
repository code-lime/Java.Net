using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Annotation;
using Java.Net.Model.Raw.Annotation.Runtime.Element;

namespace Java.Net.Model.Raw.Annotation.Runtime;

public sealed class RuntimeVisibleAnnotationsAnnotation : IAnnotation<RuntimeVisibleAnnotationsAnnotation>, IRuntimeAnnotations
{
    public override AnnotationType Type => AnnotationType.RuntimeVisibleAnnotations;
    [JavaRaw][JavaArray(JavaType.UShort)] public List<Element.Annotation> Annotations { get; set; }
}
