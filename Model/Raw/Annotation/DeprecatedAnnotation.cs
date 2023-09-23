namespace Java.Net.Model.Raw.Annotation;

public sealed class DeprecatedAnnotation : IAnnotation<DeprecatedAnnotation>
{
    public override AnnotationType Type => AnnotationType.Deprecated;
}
