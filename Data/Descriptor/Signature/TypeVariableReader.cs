using Java.Net.Data.Descriptor.Field;
using System.Collections.Generic;
using System.Linq;

namespace Java.Net.Data.Descriptor.Signature;

public class TypeVariableReader
{
    public static TypeVariableReader EMPTY { get; } = new TypeVariableReader();

    private Dictionary<string, IFieldType> Types { get; } = new Dictionary<string, IFieldType>();

    private TypeVariableReader() { }
    private TypeVariableReader(IReadOnlyDictionary<string, IFieldType> types)
    {
        foreach ((string text, IFieldType fieldType) in types)
            Types[text] = fieldType;
    }

    public TypeVariableReader Add(string name, IFieldType type)
    {
        TypeVariableReader typeVariableReader = new TypeVariableReader(Types);
        typeVariableReader.Types[name] = type;
        return typeVariableReader;
    }
    public TypeVariableReader Add(IEnumerable<(string name, IFieldType type)>? values)
    {
        TypeVariableReader typeVariableReader = new TypeVariableReader(this.Types);
        if (values == null) return typeVariableReader;
        foreach ((string name, IFieldType type) in values)
            typeVariableReader.Types[name] = type;
        return typeVariableReader;
    }
    public TypeVariableReader Add(IEnumerable<FormalTypeParameter> values)
        => this.Add(values?.Select(v => (v.Identifier.Value, (IFieldType)v.ClassBound.FieldTypeSignature!.ToDescriptor(this)!)));
    public IFieldType TryGet(string name, IFieldType? def = null)
    {
        if (this.Types.TryGetValue(name, out IFieldType? type) || def is not null) type ??= def;
        return type ?? new ObjectType() { ClassName = "java/lang/Object" };
    }
}
