using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class SignatureAnnotation : IAnnotation<SignatureAnnotation>
{
    public override AnnotationType Type => AnnotationType.Signature;
    [JavaRaw] public ushort SignatureIndex { get; set; }
    public Utf8Constant Signature
    {
        get => Handle.Constants[SignatureIndex] as Utf8Constant;
        set => SignatureIndex = Handle.OfConstant(value);
    }
}
