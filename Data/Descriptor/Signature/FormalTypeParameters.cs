using System.Linq;

namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct("\\<", ClassObject.ONE_ARRAY_PROP, "\\>")]
public sealed class FormalTypeParameters : IDescriptor<FormalTypeParameters>, IDescriptor
{
    public FormalTypeParameter[] FormalTypeParameter { get; set; } = new FormalTypeParameter[] { new FormalTypeParameter() };

    public override string ToString() => $"<{string.Join<FormalTypeParameter>("", FormalTypeParameter)}>";
}
