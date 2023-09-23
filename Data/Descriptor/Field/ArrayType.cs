namespace Java.Net.Data.Descriptor.Field;

[RegexStruct("\\[", ClassObject.PROP)]
public sealed class ArrayType : IFieldType<ArrayType>
{
    public IComponentType Type { get; set; } = BaseType.Byte;

    public override string ToString() => $"[{Type}";
}