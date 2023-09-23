namespace Java.Net.Model.Raw.Annotation;

public sealed class UnknownAnnotation : BytesAnnotation<UnknownAnnotation>
{
    public override AnnotationType Type => AnnotationType.Unknown;
    public UnknownAnnotation(byte[] info) : base(info) { }
}
