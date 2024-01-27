using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public sealed class DoubleConstant : IConstant<DoubleConstant>
{
    public override ConstantTag Tag => ConstantTag.Double;
    public override JObject JsonData => new JObject()
    {
        ["tag"] = Tag.ToString(),
        ["value"] = Value,
    };
    [JavaRaw] public double Value { get; set; }
    public override bool Equals(DoubleConstant? other) => other?.Value == Value;
    public override string ToString() => $"{base.ToString()} {Value}";

    public override DoubleConstant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
}
