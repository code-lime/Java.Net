using Java.Net.Data.Descriptor.Signature;
using System;
using System.Collections.Generic;

namespace Java.Net.Data.Descriptor.Field;

[RegexStruct("(?=(B|C|D|F|I|J|S|Z))", ClassObject.PROP)]
public sealed class BaseType : IFieldType<BaseType>, ITypeSignature, IDescriptorMutate
{
    public static readonly BaseType Byte = new BaseType('B');
    public static readonly BaseType Char = new BaseType('C');
    public static readonly BaseType Double = new BaseType('D');
    public static readonly BaseType Float = new BaseType('F');
    public static readonly BaseType Int = new BaseType('I');
    public static readonly BaseType Long = new BaseType('J');
    public static readonly BaseType Short = new BaseType('S');
    public static readonly BaseType Boolean = new BaseType('Z');

    public static IEnumerable<BaseType> Types => NamedTypes.Values;
    public static IReadOnlyDictionary<string, BaseType> NamedTypes { get; }
    static BaseType()
    {
        Dictionary<string, BaseType> dict = new Dictionary<string, BaseType>()
        {
            ["byte"] = Byte,
            ["char"] = Char,
            ["double"] = Double,
            ["float"] = Float,
            ["int"] = Int,
            ["long"] = Long,
            ["short"] = Short,
            ["boolean"] = Boolean,
        };
        NamedTypes = dict;
    }

    public char Key { get; set; }

    private BaseType() : this('\0') { }
    private BaseType(char key) => Key = key;

    public IDescriptor ToDescriptor(TypeVariableReader variable) => this;

    public override string ToString() => $"{Key}";

    public IDescriptorMutate Modify<T>(Func<T, T> map) => this is T t ? (IDescriptorMutate)map.Invoke(t)! : this;
    public IDescriptorMutate Clone() => this;
}


