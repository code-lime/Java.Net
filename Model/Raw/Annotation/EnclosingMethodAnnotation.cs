using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class EnclosingMethodAnnotation : IAnnotation<EnclosingMethodAnnotation>
{
    public override AnnotationType Type => AnnotationType.EnclosingMethod;
    [JavaRaw] public ushort ClassIndex { get; set; }
    public ClassConstant Class
    {
        get => (ClassConstant)Handle.Constants[ClassIndex];
        set => ClassIndex = Handle.OfConstant(value);
    }
    [JavaRaw] public ushort MethodIndex { get; set; }
    public NameAndTypeConstant Method
    {
        get => (NameAndTypeConstant)Handle.Constants[MethodIndex];
        set => MethodIndex = Handle.OfConstant(value);
    }
}
