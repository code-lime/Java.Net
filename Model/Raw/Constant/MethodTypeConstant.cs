using Java.Net.Data.Attribute;
using Java.Net.Data.Descriptor;
using Java.Net.Data.Descriptor.Method;
using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public sealed class MethodTypeConstant : IConstant<MethodTypeConstant>
{
    public override ConstantTag Tag => ConstantTag.MethodType;
    [JavaRaw] public ushort DescriptorIndex { get; set; }
    public string Descriptor
    {
        get => (Handle.Constants[DescriptorIndex] as Utf8Constant).Value;
        set => DescriptorIndex = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    public override bool Equals(MethodTypeConstant other) => other?.DescriptorIndex == DescriptorIndex;
    public override string ToString() => $"{base.ToString()} {Descriptor}";

    public MethodDescriptor MethodDescriptor { get => IDescriptor.TryParse<MethodDescriptor>(Descriptor); set => Descriptor = value.ToString(); }

    public override MethodTypeConstant DeepClone(JavaClass handle) => Create(handle, v => v.Descriptor = Descriptor);
}
