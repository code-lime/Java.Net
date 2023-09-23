namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct("<", ClassObject.ONE_ARRAY_PROP, ">")]
public sealed class TypeArguments : IDescriptor<TypeArguments>, IDescriptor
{
    public ITypeArgument[] TypeArgument { get; set; } = new ITypeArgument[1] { new AnyTypeArgument() };

    public override string ToString() => $"<{string.Join<ITypeArgument>("", TypeArgument)}>";
}
