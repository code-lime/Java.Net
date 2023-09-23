namespace Java.Net.Model.Raw.Annotation;

public sealed class SourceDebugExtensionAnnotation : BytesAnnotation<SourceDebugExtensionAnnotation>
{
    public override AnnotationType Type => AnnotationType.SourceFile;
    public SourceDebugExtensionAnnotation(byte[] info) : base(info) { }
}
