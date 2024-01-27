using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public sealed class EnumValue : ElementValue<EnumValue>
{
    [JavaRaw] public ushort TypeNameIndex { get; set; }
    public Utf8Constant TypeName
    {
        get => (Utf8Constant)Handle.Constants[TypeNameIndex];
        set => TypeNameIndex = Handle.OfConstant(value);
    }
    [JavaRaw] public ushort ConstantNameIndex { get; set; }
    public Utf8Constant ConstantName
    {
        get => (Utf8Constant)Handle.Constants[ConstantNameIndex];
        set => ConstantNameIndex = Handle.OfConstant(value);
    }
}
