using System.Linq;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Annotation;
using Java.Net.Model.Raw.Base;
using Newtonsoft.Json.Linq;

namespace Java.Net.Model.Raw.Constant;

public sealed class InvokeDynamicConstant : IConstant<InvokeDynamicConstant>
{
    public override ConstantTag Tag => ConstantTag.InvokeDynamic;
    public override JObject JsonData => new JObject()
    {
        ["tag"] = Tag.ToString(),
        ["bootstrap"] = BootstrapMethod.JsonData,
        ["nameAndType"] = NameAndType.JsonData,
    };
    [JavaRaw] public ushort BootstrapMethodAttributeIndex { get; set; }
    public BootstrapMethodsAnnotation.Bootstrap BootstrapMethod
    {
        get => (Handle.Annotations.OfType<BootstrapMethodsAnnotation>().First()).Methods[BootstrapMethodAttributeIndex];
        set => (Handle.Annotations.OfType<BootstrapMethodsAnnotation>().First()).Methods[BootstrapMethodAttributeIndex] = value;
    }
    [JavaRaw] public ushort NameAndTypeIndex { get; set; }
    public NameAndTypeConstant NameAndType
    {
        get => (NameAndTypeConstant)Handle.Constants[NameAndTypeIndex];
        set => NameAndTypeIndex = Handle.OfConstant(value);
    }
    public override bool Equals(InvokeDynamicConstant? other) => other?.BootstrapMethodAttributeIndex == BootstrapMethodAttributeIndex && other?.NameAndTypeIndex == NameAndTypeIndex;
    public override string ToString() => $"{base.ToString()} {NameAndType} {BootstrapMethod}";

    public override InvokeDynamicConstant DeepClone(JavaClass handle) => Create(handle, v =>
    {
        v.BootstrapMethod = BootstrapMethod.DeepClone(handle);
        v.NameAndType = NameAndType.DeepClone(handle);
    });
}
