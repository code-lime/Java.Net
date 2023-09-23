using System.Linq;

namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(ClassObject.PROP, ClassObject.PROP, ClassObject.ZERO_ARRAY_PROP)]
public sealed class FormalTypeParameter : IDescriptor<FormalTypeParameter>, IDescriptor
{
    public Identifier Identifier { get; set; } = new Identifier();
    public ClassBound ClassBound { get; set; } = new ClassBound();
    public InterfaceBound[] InterfaceBound { get; set; } = System.Array.Empty<InterfaceBound>();

    public override string ToString() => $"{Identifier}{ClassBound}{string.Join<InterfaceBound>("", InterfaceBound)}";
}
