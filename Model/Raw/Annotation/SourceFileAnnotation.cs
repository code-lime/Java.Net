using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class SourceFileAnnotation : IAnnotation<SourceFileAnnotation>
{
    public override AnnotationType Type => AnnotationType.SourceFile;
    [JavaRaw] public ushort SourceFileIndex { get; set; }
    public Utf8Constant SourceFile
    {
        get => (Utf8Constant)Handle.Constants[SourceFileIndex];
        set => SourceFileIndex = Handle.OfConstant(value);
    }
}
