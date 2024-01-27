using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public sealed class ClassValue : ElementValue<ClassValue>
{
    [JavaRaw] public ushort ClassInfoIndex { get; set; }
    public Utf8Constant ClassInfo
    {
        get => (Utf8Constant)Handle.Constants[ClassInfoIndex];
        set => ClassInfoIndex = Handle.OfConstant(value);
    }
}
