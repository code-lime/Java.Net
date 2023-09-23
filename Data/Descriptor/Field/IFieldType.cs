using Java.Net.Data.Descriptor.Method;

namespace Java.Net.Data.Descriptor.Field;

public interface IFieldType : IFieldDescriptor, IReturnDescriptor, IComponentType, IParameterDescriptor { }
public abstract class IFieldType<T> : IDescriptor<T>, IFieldType where T : IFieldType<T>
{
    [IgnoreProperty] public ArrayType Array => new ArrayType() { Type = this };
}
