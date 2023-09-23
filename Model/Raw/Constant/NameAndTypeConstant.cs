using Java.Net.Data.Attribute;
using Java.Net.Data.Descriptor;
using Java.Net.Data.Descriptor.Field;
using Java.Net.Data.Descriptor.Signature;
using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public sealed class NameAndTypeConstant : IConstant<NameAndTypeConstant>
{
    public override ConstantTag Tag => ConstantTag.NameAndType;
    [JavaRaw] public ushort NameIndex { get; set; }
    [JavaRaw] public ushort DescriptorIndex { get; set; }
    public string Name
    {
        get => (Handle.Constants[NameIndex] as Utf8Constant).Value;
        set => NameIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    public string Descriptor
    {
        get => (Handle.Constants[DescriptorIndex] as Utf8Constant).Value;
        set => DescriptorIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    public override bool Equals(NameAndTypeConstant other) => other?.NameIndex == NameIndex && other?.DescriptorIndex == DescriptorIndex;
    public override string ToString() => $"{base.ToString()} {Name} {Descriptor}";

    public ITypeSignature FieldSignature { get => IDescriptor.TryParse<ITypeSignature>(Descriptor); set => Descriptor = value.DescriptorFormat; }
    public MethodTypeSignature MethodSignature { get => IDescriptor.TryParse<MethodTypeSignature>(Descriptor); set => Descriptor = value.DescriptorFormat; }

    public override NameAndTypeConstant DeepClone(JavaClass handle) => Create(handle, v =>
    {
        v.Name = Name;
        v.Descriptor = Descriptor;
    });
}
