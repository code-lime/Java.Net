using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Constant;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public sealed class ElementValuePair : BaseRaw<ElementValuePair>
{
    [JavaRaw] public ushort NameIndex { get; set; }
    public Utf8Constant Name
    {
        get => (Utf8Constant)Handle.Constants[NameIndex];
        set => NameIndex = Handle.OfConstant(value);
    }
    [JavaRaw] public IElementValue ElementValue { get; set; } = null!;
}
