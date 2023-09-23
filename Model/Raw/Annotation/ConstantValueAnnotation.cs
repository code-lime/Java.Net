using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation;

public sealed class ConstantValueAnnotation : IAnnotation<ConstantValueAnnotation>
{
    public override AnnotationType Type => AnnotationType.ConstantValue;
    [JavaRaw] public ushort ValueIndex { get; set; }
    public IConstant Value
    {
        get => Handle.Constants[ValueIndex] as IConstant;
        set => ValueIndex = Handle.OfConstant(value);
    }
}
