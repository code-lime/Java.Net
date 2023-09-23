using System.Collections.Generic;
using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Annotation;
using Java.Net.Model.Raw.Annotation.Runtime.Element;

namespace Java.Net.Model.Raw.Annotation.Runtime;

public sealed class RuntimeInvisibleParameterAnnotationsAnnotation : IAnnotation<RuntimeInvisibleParameterAnnotationsAnnotation>, IRuntimeParameterAnnotations
{
    public override AnnotationType Type => AnnotationType.RuntimeInvisibleParameterAnnotations;
    [JavaRaw][JavaArray(JavaType.Byte)] public List<Element.ParameterAnnotation> ParameterAnnotations { get; set; }
}
