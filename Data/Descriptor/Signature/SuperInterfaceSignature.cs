namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(ClassObject.PROP)]
public sealed class SuperInterfaceSignature : IDescriptor<SuperInterfaceSignature>, IDescriptor
{
    public ClassTypeSignature ClassTypeSignature { get; set; } = new ClassTypeSignature();

    public override string ToString() => $"{ClassTypeSignature}";
}
