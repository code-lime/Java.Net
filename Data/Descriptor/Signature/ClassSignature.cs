using System.Linq;

namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(ClassObject.PROP, ClassObject.PROP, ClassObject.ZERO_ARRAY_PROP)]
public sealed class ClassSignature : IDescriptor<ClassSignature>, IDescriptor
{
    public FormalTypeParameters? FormalTypeParameters { get; set; } = null;
    public SuperClassSignature SuperClassSignature { get; set; } = new SuperClassSignature();
    public SuperInterfaceSignature[] SuperInterfaceSignature { get; set; } = new SuperInterfaceSignature[0];

    public override string ToString() => $"{FormalTypeParameters?.ToString() ?? ""}{SuperClassSignature}{string.Join<SuperInterfaceSignature>("", SuperInterfaceSignature)}";
}
