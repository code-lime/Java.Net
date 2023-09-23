using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class EnclosingMethodAnnotation : IAnnotation<EnclosingMethodAnnotation>
{
    public override AnnotationType Type => AnnotationType.EnclosingMethod;
    [JavaRaw] public ushort ClassIndex { get; set; }
    public ClassConstant Class
    {
        get => Handle.Constants[ClassIndex] as ClassConstant;
        set => ClassIndex = Handle.OfConstant(value);
    }
    [JavaRaw] public ushort MethodIndex { get; set; }
    public NameAndTypeConstant Method
    {
        get => Handle.Constants[MethodIndex] as NameAndTypeConstant;
        set => MethodIndex = Handle.OfConstant(value);
    }
}
