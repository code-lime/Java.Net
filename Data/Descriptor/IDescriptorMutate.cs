using System;

namespace Java.Net.Data.Descriptor;

public interface IDescriptorMutate : IDescriptor
{
    IDescriptorMutate Modify<T>(Func<T, T> map);
    IDescriptorMutate Clone();
}
