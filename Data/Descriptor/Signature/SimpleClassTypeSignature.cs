namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(ClassObject.PROP, ClassObject.PROP)]
public sealed class SimpleClassTypeSignature : IDescriptor<SimpleClassTypeSignature>, IDescriptor
{
    public Identifier Identifier { get; set; } = new Identifier();
    public TypeArguments? TypeArguments { get; set; } = null;

    public override string ToString() => $"{Identifier}{TypeArguments?.ToString() ?? ""}";
}
