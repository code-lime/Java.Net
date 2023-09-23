using Java.Net.Data.Descriptor.Field;

namespace Java.Net.Data.Descriptor.Method;

[RegexStruct("(V)")]
public sealed class VoidDescriptor : IDescriptor<VoidDescriptor>, IReturnDescriptor, IReturnType
{
    public static VoidDescriptor Instance { get; } = new VoidDescriptor();

    public override string ToString() => $"V";
}