using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public sealed class PackageInfoConstant : IConstant<PackageInfoConstant>
{
    public override ConstantTag Tag => ConstantTag.PackageInfo;
    public override JObject JsonData => new JObject()
    {
        ["tag"] = Tag.ToString(),
        ["name"] = Name,
    };
    [JavaRaw] public ushort NameIndex { get; set; }
    public string Name
    {
        get => ((Utf8Constant)Handle.Constants[NameIndex]).Value;
        set => NameIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    public override bool Equals(PackageInfoConstant? other) => other?.NameIndex == NameIndex;
    public override string ToString() => $"{base.ToString()} {Name}";

    public override PackageInfoConstant DeepClone(JavaClass handle) => Create(handle, v =>
    {
        v.Name = Name;
    });
}
