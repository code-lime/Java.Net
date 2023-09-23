namespace Java.Net.Model.Raw.Annotation;

public sealed class SyntheticAnnotation : IAnnotation<SyntheticAnnotation>
{
    public override AnnotationType Type => AnnotationType.Synthetic;
}
