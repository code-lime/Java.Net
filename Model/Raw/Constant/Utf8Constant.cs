using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public sealed class Utf8Constant : IConstant<Utf8Constant>
{
    public override ConstantTag Tag => ConstantTag.Utf8;
    [JavaRaw] public string Value { get; set; }
    public override bool Equals(Utf8Constant other) => other?.Value == Value;
    public override string ToString() => $"{base.ToString()} {Value}";

    public override Utf8Constant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
}
