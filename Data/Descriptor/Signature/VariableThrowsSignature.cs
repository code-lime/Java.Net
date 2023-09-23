namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(@"\^", ClassObject.PROP)]
public sealed class VariableThrowsSignature : IDescriptor<VariableThrowsSignature>, IThrowsSignature
{
    public TypeVariableSignature TypeVariableSignature { get; set; } = new TypeVariableSignature();

    public override string ToString() => $"^{TypeVariableSignature}";
}
