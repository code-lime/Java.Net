namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct("\\:", ClassObject.PROP)]
public sealed class ClassBound : IDescriptor<ClassBound>, IDescriptor
{
    public FieldTypeSignature? FieldTypeSignature { get; set; } = null;

    public override string ToString() => $":{FieldTypeSignature?.ToString() ?? ""}";
}
