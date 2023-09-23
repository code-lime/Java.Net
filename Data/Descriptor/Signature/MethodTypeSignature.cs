using Java.Net.Data.Descriptor.Field;
using Java.Net.Data.Descriptor.Method;
using System;
using System.Linq;

namespace Java.Net.Data.Descriptor.Signature;

[RegexStruct(ClassObject.PROP, @"\(", ClassObject.ZERO_ARRAY_PROP, @"\)", ClassObject.PROP, ClassObject.ZERO_ARRAY_PROP)]
public sealed class MethodTypeSignature : IDescriptor<MethodTypeSignature>, IDescriptor, INative<MethodDescriptor>
{
    public FormalTypeParameters? FormalTypeParameters { get; set; } = null;
    public ITypeSignature[] TypeSignature { get; set; } = Array.Empty<ITypeSignature>();
    public IReturnType ReturnType { get; set; } = BaseType.Byte;
    public IThrowsSignature[] ThrowsSignature { get; set; } = Array.Empty<IThrowsSignature>();

    public IDescriptor ToDescriptor(TypeVariableReader variable)
    {
        variable = variable.Add(FormalTypeParameters?.FormalTypeParameter ?? Array.Empty<FormalTypeParameter>());
        return new MethodDescriptor()
        {
            Parameters = TypeSignature.Select(v => v.ToDescriptor(variable)).ToArray(),
            ReturnDescriptor = ReturnType switch
            {
                IReturnDescriptor returnDescriptor => returnDescriptor,
                ITypeSignature typeSignature => typeSignature.ToDescriptor(variable),
                _ => throw new ArgumentException("Can't convert '" + ReturnType.GetType().FullName + "' to " + typeof(IReturnDescriptor).FullName),
            }
        };
    }

    public override string ToString() => $"{FormalTypeParameters?.ToString() ?? ""}({string.Join<ITypeSignature>("", TypeSignature)}){ReturnType}{string.Join<IThrowsSignature>("", ThrowsSignature)}";
}
