using Java.Net.Data.Descriptor.Field;

namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct("\\[", ClassObject.PROP)]
public sealed class ArrayTypeSignature : IDescriptor<ArrayTypeSignature>, IFieldTypeSignature
{
    public ITypeSignature TypeSignature { get; set; } = BaseType.Byte;

    public IDescriptor ToDescriptor(TypeVariableReader variable) => TypeSignature.ToDescriptor(variable).Array;

    public override string ToString() => $"[{TypeSignature}";
}
