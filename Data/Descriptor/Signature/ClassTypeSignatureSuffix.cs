namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(@"(\.)", ClassObject.PROP)]
public sealed class ClassTypeSignatureSuffix : IDescriptor<ClassTypeSignatureSuffix>, IDescriptor
{
    public SimpleClassTypeSignature SimpleClassTypeSignature { get; set; } = new SimpleClassTypeSignature();

    public override string ToString() => $".{SimpleClassTypeSignature}";
}
