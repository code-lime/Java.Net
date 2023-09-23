using Java.Net.Data.Attribute;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public sealed class AnnotationValue : ElementValue<AnnotationValue>
{
    [JavaRaw] public Annotation Annotation { get; set; }
}
