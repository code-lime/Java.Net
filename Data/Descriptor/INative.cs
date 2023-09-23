using Java.Net.Data.Descriptor.Signature;

namespace Java.Net.Data.Descriptor;

public interface INative : IDescriptor
{
    IDescriptor ToDescriptor(TypeVariableReader variable);
}

public interface INative<T> : INative where T : IDescriptor
{
    new T ToDescriptor(TypeVariableReader variable) => (T)((INative)this).ToDescriptor(variable);
}
