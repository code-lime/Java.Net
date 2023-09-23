using Java.Net.Data.Attribute;
using Java.Net.Data.Descriptor;
using Java.Net.Data.Descriptor.Field;
using Java.Net.Model.Raw.Base;

namespace Java.Net.Model.Raw.Constant;

public sealed class ClassConstant : IConstant<ClassConstant>
{
    public override ConstantTag Tag => ConstantTag.Class;
    [JavaRaw] public ushort Index { get; set; }
    public string Name
    {
        get => (Handle.Constants[Index] as Utf8Constant).Value;
        set => Index = Handle.OfConstant(new Utf8Constant() { Value = value });
    }
    public override bool Equals(ClassConstant other) => other?.Index == Index;
    public override string ToString() => $"{base.ToString()} [{Index}] {Name}";

    public IFieldType FieldType { get => IDescriptor.TryParse<IFieldType>(Name); set => Name = value.ToString(); }

    public override ClassConstant DeepClone(JavaClass handle) => Create(handle, v => v.Name = Name);
}
