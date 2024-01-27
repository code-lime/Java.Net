using Java.Net.Data;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public sealed class MethodHandleConstant : IConstant<MethodHandleConstant>
{
    public override ConstantTag Tag => ConstantTag.MethodHandle;
    public override JObject JsonData => new JObject()
    {
        ["tag"] = Tag.ToString(),
        ["kind"] = ReferenceKind.ToString(),
        ["reference"] = Reference.JsonData,
    };
    [JavaRaw(JavaType.Byte)] public ReferenceKind ReferenceKind { get; set; }
    [JavaRaw] public ushort ReferenceIndex { get; set; }
    public IRefConstant Reference
    {
        get => (IRefConstant)Handle.Constants[ReferenceIndex];
        set => ReferenceIndex = Handle.OfConstant(value);
    }
    public override bool Equals(MethodHandleConstant? other) => other?.ReferenceKind == ReferenceKind && other?.ReferenceIndex == ReferenceIndex;
    public override string ToString() => $"{base.ToString()} {ReferenceKind} {Reference}";

    public override MethodHandleConstant DeepClone(JavaClass handle) => Create(handle, v =>
    {
        v.ReferenceKind = ReferenceKind;
        v.Reference = Reference.DeepClone(handle);
    });
}
