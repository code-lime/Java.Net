using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Annotation.Runtime.Element;

namespace Java.Net.Model.Raw.Annotation;

public sealed class AnnotationDefaultAnnotation : IAnnotation<AnnotationDefaultAnnotation>
{
    public override AnnotationType Type => AnnotationType.AnnotationDefault;
    [JavaRaw] public IElementValue DefaultValue { get; set; } = null!;
}
