using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public sealed class FloatConstant : IConstant<FloatConstant>
{
    public override ConstantTag Tag => ConstantTag.Float;
    public override JObject JsonData => new JObject()
    {
        ["tag"] = Tag.ToString(),
        ["value"] = Value,
    };
    [JavaRaw] public float Value { get; set; }
    public override bool Equals(FloatConstant? other) => other?.Value == Value;
    public override string ToString() => $"{base.ToString()} {Value}";

    public override FloatConstant DeepClone(JavaClass handle) => Create(handle, v => v.Value = Value);
}
