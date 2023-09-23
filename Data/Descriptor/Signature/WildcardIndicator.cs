namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(@"(?=(\+|\-))", ClassObject.PROP)]
public sealed class WildcardIndicator : IDescriptor<WildcardIndicator>, IDescriptor
{
    public char Indicator { get; set; } = '+';

    public override string ToString() => $"{Indicator}";
}
