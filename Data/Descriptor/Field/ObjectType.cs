namespace Java.Net.Data.Descriptor.Field;

[RegexStruct("L", ClassObject.PROP, ";")]
public sealed class ObjectType : IFieldType<ObjectType>
{
    public static ObjectType Create(string class_name) => new ObjectType() { ClassName = class_name };
    public string ClassName { get; set; } = "type";

    public override string ToString() => $"L{ClassName};";
}


