using Java.Net.Data.Descriptor.Field;

namespace Java.Net.Data.Descriptor.Method;

[RegexStruct("\\(", ClassObject.ZERO_ARRAY_PROP, "\\)", ClassObject.PROP)]
public sealed class MethodDescriptor : IDescriptor<MethodDescriptor>, IDescriptor
{
    public IParameterDescriptor[] Parameters { get; set; } = System.Array.Empty<IParameterDescriptor>();
    public IReturnDescriptor ReturnDescriptor { get; set; } = BaseType.Byte;

    public override string ToString() => $"({string.Join<IParameterDescriptor>("", Parameters)}){ReturnDescriptor}";
}