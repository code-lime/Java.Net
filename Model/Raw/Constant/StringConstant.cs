using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public sealed class StringConstant : IConstant<StringConstant>
{
    public override ConstantTag Tag => ConstantTag.String;
    public override JObject JsonData => new JObject()
    {
        ["tag"] = Tag.ToString(),
        ["value"] = String,
    };
    [JavaRaw] public ushort StringIndex { get; set; }
    public string String
    {
        get => ((Utf8Constant)Handle.Constants[StringIndex]).Value;
        set => StringIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    public override bool Equals(StringConstant? other) => other?.StringIndex == StringIndex;
    public override string ToString() => $"{base.ToString()} {String}";

    public override StringConstant DeepClone(JavaClass handle) => Create(handle, v => v.String = String);
}
