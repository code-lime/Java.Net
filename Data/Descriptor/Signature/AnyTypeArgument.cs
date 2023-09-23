namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(@"(\*)")]
public sealed class AnyTypeArgument : IDescriptor<AnyTypeArgument>, ITypeArgument
{
    public static AnyTypeArgument Instance { get; } = new AnyTypeArgument();

    public override string ToString() => $"*";
}
