using Java.Net.Data.Descriptor.Field;

namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct("T", ClassObject.PROP, ";")]
public sealed class TypeVariableSignature : IDescriptor<TypeVariableSignature>, IFieldTypeSignature
{
    public Identifier Identifier { get; set; } = new Identifier();
    public IDescriptor ToDescriptor(TypeVariableReader variable) => variable.TryGet(Identifier.Value, null);
    public override string ToString() => $"T{Identifier};";
}
