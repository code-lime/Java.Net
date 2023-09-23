namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(ClassObject.PROP, @"(?=\.|;|\[|\/|<|>|:|$)")]
public sealed class Identifier : IDescriptor<Identifier>, IDescriptor
{
    public static Identifier Create(string value) => new Identifier() { Value = value };

    public string Value { get; set; } = "identifier";

    public override string ToString() => $"{Value}";
}
