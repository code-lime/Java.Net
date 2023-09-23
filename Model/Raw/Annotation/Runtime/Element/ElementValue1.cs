using Java.Net.Data;
using Java.Net.Data.Attribute;

namespace Java.Net.Model.Raw.Annotation.Runtime.Element;

public abstract class ElementValue<I> : BaseRaw<I>, IElementValue where I : ElementValue<I>
{
    [JavaRaw(JavaType.Byte, IsReaded: false)] public ElementTag Tag { get; set; }
}
