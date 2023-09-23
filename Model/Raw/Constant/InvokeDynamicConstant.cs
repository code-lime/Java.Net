using System.Linq;
using Java.Net.Data.Attribute;
using Java.Net.Model.Raw.Annotation;
using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public sealed class InvokeDynamicConstant : IConstant<InvokeDynamicConstant>
{
    public override ConstantTag Tag => ConstantTag.InvokeDynamic;
    [JavaRaw] public ushort BootstrapMethodAttributeIndex { get; set; }
    public BootstrapMethodsAnnotation.Bootstrap BootstrapMethod
    {
        get => (Handle.Annotations.FirstOrDefault(v => v is BootstrapMethodsAnnotation) as BootstrapMethodsAnnotation).Methods[BootstrapMethodAttributeIndex];
        set => (Handle.Annotations.FirstOrDefault(v => v is BootstrapMethodsAnnotation) as BootstrapMethodsAnnotation).Methods[BootstrapMethodAttributeIndex] = value;
    }
    [JavaRaw] public ushort NameAndTypeIndex { get; set; }
    public NameAndTypeConstant NameAndType
    {
        get => Handle.Constants[NameAndTypeIndex] as NameAndTypeConstant;
        set => NameAndTypeIndex = Handle.OfConstant(value);
    }
    public override bool Equals(InvokeDynamicConstant other) => other?.BootstrapMethodAttributeIndex == BootstrapMethodAttributeIndex && other?.NameAndTypeIndex == NameAndTypeIndex;
    public override string ToString() => $"{base.ToString()} {NameAndType} {BootstrapMethod}";

    public override InvokeDynamicConstant DeepClone(JavaClass handle) => Create(handle, v =>
    {
        v.BootstrapMethod = BootstrapMethod.DeepClone(handle);
        v.NameAndType = NameAndType.DeepClone(handle);
    });
}
