using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public sealed class LongConstant : IConstant<LongConstant>
{
    public override ConstantTag Tag => ConstantTag.Long;
    public override JObject JsonData => new JObject()
    {
        ["tag"] = Tag.ToString(),
        ["value"] = Value,
    };
    [JavaRaw] public long Value { get; set; }
    public override bool Equals(LongConstant? other) => other?.Value == Value;
    public override string ToString() => $"{base.ToString()} {Value}";

    public override LongConstant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
}
