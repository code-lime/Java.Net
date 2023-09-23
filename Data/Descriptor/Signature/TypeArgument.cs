namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(ClassObject.PROP, ClassObject.PROP)]
public sealed class TypeArgument : IDescriptor<TypeArgument>, ITypeArgument
{
    public WildcardIndicator? WildcardIndicator { get; set; } = null;
    public FieldTypeSignature FieldTypeSignature { get; set; } = new FieldTypeSignature();

    public override string ToString() => $"{WildcardIndicator?.ToString() ?? ""}{FieldTypeSignature}";
}
