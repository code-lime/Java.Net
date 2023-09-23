using Java.Net.Data.Descriptor.Field;

namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(ClassObject.PROP)]
public sealed class FieldTypeSignature : IDescriptor<FieldTypeSignature>, IDescriptor, ITypeSignature
{
    public IFieldTypeSignature Value { get; set; } = new ClassTypeSignature();

    public IDescriptor ToDescriptor(TypeVariableReader variable) => this.Value.ToDescriptor(variable);

    public override string ToString() => $"{Value}";
}
