namespace Java.Net.Data.Descriptor.Field;

public interface IComponentType : IDescriptor
{
    [IgnoreProperty] ArrayType Array { get; }
}


