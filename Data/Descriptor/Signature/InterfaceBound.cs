namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct("\\:", ClassObject.PROP)]
public sealed class InterfaceBound : IDescriptor<InterfaceBound>, IDescriptor
{
    public FieldTypeSignature FieldTypeSignature { get; set; } = new FieldTypeSignature();

    public override string ToString() => $":{FieldTypeSignature}";
}
