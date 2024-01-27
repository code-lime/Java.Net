using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public interface IRefConstant : IConstant
{
    ushort ClassIndex { get; set; }
    ushort NameAndTypeIndex { get; set; }
    ClassConstant Class { get; set; }
    NameAndTypeConstant NameAndType { get; set; }

    new IRefConstant DeepClone(JavaClass handle);
}

public abstract class BaseRefConstant<I> : IConstant<I>, IRefConstant where I : BaseRefConstant<I>
{
    [JavaRaw] public ushort ClassIndex { get; set; }
    [JavaRaw] public ushort NameAndTypeIndex { get; set; }

    public override JObject JsonData => new JObject()
    {
        ["tag"] = Tag.ToString(),
        ["class"] = Class.JsonData,
        ["nameAndType"] = NameAndType.JsonData
    };

    public ClassConstant Class
    {
        get => (ClassConstant)Handle.Constants[ClassIndex];
        set => ClassIndex = Handle.OfConstant(value);
    }
    public NameAndTypeConstant NameAndType
    {
        get => (NameAndTypeConstant)Handle.Constants[NameAndTypeIndex];
        set => NameAndTypeIndex = Handle.OfConstant(value);
    }
    public override bool Equals(I? other) => other?.ClassIndex == ClassIndex && other?.NameAndTypeIndex == NameAndTypeIndex;
    public override string ToString() => $"{base.ToString()} ({Class}, {NameAndType})";

    public override I DeepClone(JavaClass handle) => Create(handle, v =>
    {
        v.Class = Class.DeepClone(handle);
        v.NameAndType = NameAndType.DeepClone(handle);
    });
    IRefConstant IRefConstant.DeepClone(JavaClass handle) => DeepClone(handle);
}
