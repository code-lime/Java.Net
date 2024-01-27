using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public sealed class IntegerConstant : IConstant<IntegerConstant>
{
    public override ConstantTag Tag => ConstantTag.Integer;
    public override JObject JsonData => new JObject()
    {
        ["tag"] = Tag.ToString(),
        ["value"] = Value,
    };
    [JavaRaw] public int Value { get; set; }
    public override bool Equals(IntegerConstant? other) => other?.Value == Value;
    public override string ToString() => $"{base.ToString()} {Value}";

    public override IntegerConstant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
}
