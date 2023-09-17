#define LOGGER
//#undef LOGGER
#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Java.Net.Data
{
    using System.Collections;
    using System.Text.RegularExpressions;

    internal static class Variable
    {
        internal const string PROP = "\u0000#VALUE";
        internal const string ZERO_ARRAY_PROP = "\u0000#ARRAY#0";
        internal const string ONE_ARRAY_PROP = "\u0000#ARRAY#1";
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)] internal class RegexStructAttribute : Attribute
    {
        public bool ParseAll { get; }
        public string[] Format { get; }
        public RegexStructAttribute(params string[] format) : this(true, format) { }
        public RegexStructAttribute(bool parse_all, params string[] format)
        {
            this.ParseAll = parse_all;
            this.Format = format;
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] internal class IgnorePropertyAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] internal class NullablePropertyAttribute : Attribute { }

    namespace Element
    {
        public interface IElement : IEquatable<IElement>
        {
            private static readonly Regex re = new Regex(@"{(\d+)}", RegexOptions.Compiled);
            private static string ReplaceArgs(string text, List<string> args)
                => ReplaceArgs(text, v => args[v]);
            private static string ReplaceArgs(string text, Func<int, string> func)
                => re.Replace(text, match => func.Invoke(int.Parse(match.Groups[1].Value)));

#if LOGGER
            private static void Log(string text) { }
            private class PrefixUsage : IDisposable { public void Dispose() { } }
#else
            private static void Log(string text)
            {
                Console.WriteLine(PrefixUsage.Prefix + " " + text);
            }
            private class PrefixUsage : IDisposable
            {
                private static int prefix_index = 0;
                public static string Prefix => new string(' ', prefix_index * 3);
                public PrefixUsage() => prefix_index++;
                public void Dispose() => prefix_index--;
            }
#endif

            private class ClassObject
            {
                public Type Type { get; }
                public RegexStructAttribute Attribute { get; }
                public List<PropertyInfo> Properties { get; }

                public ClassObject(Type type, RegexStructAttribute attribute)
                {
                    Type = type;
                    Attribute = attribute;
                    Properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(v => !v.GetCustomAttributes<IgnorePropertyAttribute>().Any())
                        .ToList();
                }

                public bool TryRegexCreator(ref string text, out object? result)
                {
                    using PrefixUsage _0 = new PrefixUsage();
                    result = default;
                    string part_text = text;
                    int field_index = 0;
                    List<object?> values = new List<object?>();
                    string[] formats = Attribute.Format;
                    int length = formats.Length;

                    Log("Create: " + Type.FullName);
                    for (int i = 0; i < length; i++)
                    {
                        using PrefixUsage _1 = new PrefixUsage();
                        string format = formats[i];

                        bool TryReadField(out object? value, bool step_field) => TryReadFieldType(out value, out _, step_field);
                        bool TryReadFieldType(out object? value, out Type? type, bool step_field)
                        {
                            value = default;
                            PropertyInfo property = Properties[field_index];
                            Log("Field: " + property.Name);
                            if (step_field) field_index++;
                            type = property.PropertyType;
                            if (TryGetIListType(type, out Type element_type)) type = element_type;
                            if (type == typeof(char))
                            {
                                if (part_text.Length == 0) return false;
                                value = part_text[0];
                                part_text = part_text[1..];
                                return true;
                            }
                            if (type == typeof(string))
                            {
                                if (i + 1 == length)
                                {
                                    value = part_text;
                                    part_text = "";
                                    return true;
                                }
                                string next = formats[i + 1];
                                switch (next)
                                {
                                    case Variable.PROP:
                                    case Variable.ONE_ARRAY_PROP:
                                    case Variable.ZERO_ARRAY_PROP: return false;
                                }
                                MatchCollection collection = new Regex(next).Matches(part_text);
                                if (collection.Count == 0) return false;
                                int index = collection[0].Index;
                                value = part_text[..index];
                                part_text = part_text[index..];
                                return true;
                            }
                            if (type == null) return false;
                            bool isNullable;
                            if (property.GetCustomAttributes<NullablePropertyAttribute>().Any())
                            {
                                isNullable = true;
                            }
                            else
                            {
                                if (part_text.Length == 0) return false;
                                isNullable = false;
                            }
                            foreach (ClassObject cobject in TryGetAll(type))
                                if (cobject.TryRegexCreator(ref part_text, out value))
                                    return true;
                            if (!isNullable) return false;
                            value = null;
                            return true;
                        }

                        Log("Format: " + format);
                        switch (format)
                        {
                            case Variable.PROP:
                                {
                                    if (!TryReadField(out object? value, true))
                                    {
                                        Log("IGNORED#F: " + Type.FullName);
                                        return false;
                                    }
                                    values.Add(value);
                                    break;
                                }
                            case Variable.ZERO_ARRAY_PROP:
                            case Variable.ONE_ARRAY_PROP:
                                {
                                    List<object?> list = new List<object?>();
                                    Type? type;
                                    while (TryReadFieldType(out object? value, out type, false)) list.Add(value);
                                    int _length = list.Count;
                                    if (format == Variable.ONE_ARRAY_PROP && _length == 0)
                                    {
                                        Log("IGNORED#A: " + Type.FullName);
                                        return false;
                                    }
                                    PropertyInfo property = Properties[field_index];
                                    if (type == null) throw new Exception("Type is 'null'");
                                    Array array = Array.CreateInstance(type, _length);
                                    for (int _i = 0; _i < _length; _i++) array.SetValue(list[_i], _i);
                                    values.Add(array);
                                    field_index++;
                                    break;
                                }
                            default:
                                {
                                    Regex regex = new Regex("^" + format + "(?'end'.*)");
                                    int out_index = -1;
                                    Log("Check regex: " + regex);
                                    Log("Text: " + part_text);
                                    MatchCollection collection = regex.Matches(part_text);
                                    if (collection.Count == 0)
                                    {
                                        Log("IGNORED: Regex match empty");
                                        return false;
                                    }
                                    foreach (Match? match in regex.Matches(part_text))
                                    {
                                        foreach (Group? group in match?.Groups ?? (IEnumerable<Group>)new Group[0])
                                        {
                                            if (group.Name == "end")
                                            {
                                                out_index = group.Index;
                                                break;
                                            }
                                        }
                                        if (out_index != -1) break;
                                    }
                                    if (out_index == -1) out_index = part_text.Length;
                                    part_text = part_text[out_index..];
                                    Log("Out: " + part_text);
                                    break;
                                }
                        }
                    }

                    length = values.Count;
                    if (length != Properties.Count)
                    {
                        Log("IGNORED#L: " + Type.FullName);
                        return false;
                    }

                    result = Activator.CreateInstance(Type, true);
                    for (int i = 0; i < length; i++)
                    {
                        PropertyInfo property = Properties[i];
                        property.SetValue(result, values[i]);
                    }
                    text = part_text;
                    Log("CREATED: " + Type.FullName);
                    return true;
                }
            }
            private static readonly Dictionary<Type, ClassObject> classes = new Dictionary<Type, ClassObject>();
            static IEnumerable<(Type type, T attribute)> GetAllAttributes<T>() where T : Attribute
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    foreach (var type in assembly.GetTypes())
                        foreach (var attribute in type.GetCustomAttributes<T>(true))
                            yield return (type, attribute);
            }
            static IElement()
            {
                foreach ((Type type, RegexStructAttribute attribute) in GetAllAttributes<RegexStructAttribute>())
                {
                    Log($"Load type '{type.FullName}' with '{string.Join(" & ", attribute.Format)}'");
                    classes.Add(type, new ClassObject(type, attribute));
                }
            }

            private static bool TryGet(Type type, out ClassObject? dat)
            {
                dat = default;
                foreach ((Type ctype, ClassObject cobject) in classes)
                {
                    if (!type.IsAssignableFrom(ctype)) continue;
                    dat = cobject;
                    return true;
                }
                return false;
            }
            private static IEnumerable<ClassObject> TryGetAll(Type type)
            {
                foreach ((Type ctype, ClassObject cobject) in classes)
                {
                    if (!type.IsAssignableFrom(ctype)) continue;
                    yield return cobject;
                }
            }

            private static bool TryGetIListType(Type type, out Type result)
            {
                Type? ret = type.IsArray
                    ? type.GetElementType()
                    : (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
                        ? type.GenericTypeArguments[0]
                        : null;
                if (ret == null)
                {
                    result = typeof(object);
                    return false;
                }
                result = ret;
                return true;
            }

            public static T TryParse<T>(string text, bool full = true) where T : IElement => TryParse(text, out T? value, full) && value != null ? value : throw new ArgumentException($"Error parse {text} to class '{typeof(T)}'");
            public static bool TryParse<T>(string text, out T? descriptor, bool full = true) where T : IElement
            {
                descriptor = default;
                Type type = typeof(T);
                foreach ((Type ctype, ClassObject cobject) in classes)
                {
                    if (cobject.Attribute.ParseAll ? (!type.IsAssignableFrom(ctype)) : (ctype != type)) continue;
                    string part_text = text;
                    if (!cobject.TryRegexCreator(ref part_text, out object? value)) continue;
                    if (full && part_text.Length > 0) continue;
                    descriptor = (T?)value;
                    return true;
                }
                return false;
            }
            public static IElement TryParseAny(string text, bool full = true) => TryParse<IElement>(text, full);
            public static bool TryParseAny(string text, out IElement? descriptor, bool full = true) => TryParse(text, out descriptor, full);
            public static IElement ReplaceAll<T>(IElement descriptor, Func<T, T> func) where T : IElement
            {
                object? ReplaceObject(object? value)
                {
                    if (value == null) return null;
                    else if (value is IElement descriptor) return ReplaceAll(descriptor, func);
                    else if (value is Array array)
                    {
                        if (array.Rank != 1) return value;
                        int length = array.Length;
                        for (int i = 0; i < length; i++) array.SetValue(ReplaceObject(array.GetValue(i)), i);
                        return array;
                    }
                    else return value;
                }
                if (descriptor is T _descriptor) return func.Invoke(_descriptor);
                if (!TryGet(descriptor.GetType(), out ClassObject? dat) || dat == null) return descriptor;
                foreach (var prop in dat.Properties)
                {
                    object? value = prop.GetValue(descriptor);
                    if (value == null) continue;
                    if (value is T _value)
                    {
                        prop.SetValue(descriptor, func.Invoke(_value));
                        continue;
                    }
                    prop.SetValue(descriptor, ReplaceObject(value));
                }
                return descriptor;
            }
            public static IElement ReplaceAllClone<T>(IElement descriptor, Func<T, T> func) where T : IElement
            {
                return descriptor.DeepClone().ReplaceAll(func);
            }
            public static bool Equals(IElement? descriptor1, IElement? descriptor2)
            {
                if (null == descriptor1) return null == descriptor2;
                if (null == descriptor2) return null == descriptor1;

                Type type = descriptor1.GetType();
                if (descriptor2.GetType() != type) return false;
                if (!classes.TryGetValue(type, out ClassObject? cobject) || cobject == null) return false;
                foreach (var prop in cobject.Properties)
                {
                    object? v1 = prop.GetValue(descriptor1);
                    object? v2 = prop.GetValue(descriptor2);
                    if (v1 is IList a1 && v2 is IList a2)
                    {
                        int length = a1.Count;
                        if (length != a2.Count) return false;
                        for (int i = 0; i < length; i++)
                            if (!object.Equals(a1[i], a2[i]))
                                return false;
                        continue;
                    }
                    if (!object.Equals(v1, v2))
                        return false;
                }
                return true;
            }
            public static IEnumerable<object?> GetDescriptorValues(IElement descriptor)
            {
                Type type = descriptor.GetType();
                if (!classes.TryGetValue(type, out ClassObject? cobject) || cobject == null) yield break;
                foreach (var prop in cobject.Properties) yield return prop.GetValue(descriptor);
            }
            public static T DeepClone<T>(T descriptor) where T : IElement
            {
                object? CloneObject(object? value)
                {
                    if (value == null) return null;
                    else if (value is IElement descriptor) return descriptor.DeepClone();
                    else if (value is IList array)
                    {
                        Array _array = Array.CreateInstance(array.GetType().GetElementType() ?? throw new Exception($"Type {array.GetType()} is not array"), array.Count);
                        int length = _array.Length;
                        for (int i = 0; i < length; i++) _array.SetValue(CloneObject(array[i]), i);
                        if (array is Array) return _array;
                        IList? dat = (IList?)Activator.CreateInstance(array.GetType());
                        foreach (var item in _array) dat?.Add(item);
                        return dat;
                    }
                    else return value;
                }
                Type type = descriptor.GetType();
                object? other;
                if (!classes.TryGetValue(type, out ClassObject? cobject) || cobject == null || (other = Activator.CreateInstance(type, true)) == null)
                    throw new NotSupportedException($"Type '{type.FullName}' not support DeepClone");
                foreach (var prop in cobject.Properties) prop.SetValue(other, CloneObject(prop.GetValue(descriptor)));
                return (T)other;
            }

            [IgnoreProperty] object?[] Values { get; }
            IElement DeepClone();
            IElement ReplaceAll<I>(Func<I, I> func) where I : IElement;
            IElement ReplaceAllClone<I>(Func<I, I> func) where I : IElement;
        }
        public abstract class IElement<T> : IElement where T : IElement<T>
        {
            public abstract string ToDisplay();
            [IgnoreProperty] public string Display => ToDisplay();
            [IgnoreProperty] public object?[] Values => IElement.GetDescriptorValues(this).ToArray();

            public T ReplaceAll<I>(Func<I, I> func) where I : IElement => (T)IElement.ReplaceAll(this, func);
            public T ReplaceAllClone<I>(Func<I, I> func) where I : IElement => (T)IElement.ReplaceAllClone(this, func);

            public static bool TryParse(string text, out T? descriptor, bool full = true) => IElement.TryParse<T>(text, out descriptor, full);
            public static T TryParse(string text, bool full = true) => IElement.TryParse<T>(text, full);

            public bool Equals(T? other)
                => IElement.Equals(this, other);
            public override bool Equals(object? obj)
                => obj is T descriptor && Equals((T?)descriptor);
            public override int GetHashCode()
            {
                object?[] array = Values;
                int hash = array.Length;
                foreach (var item in array) hash = HashCode.Combine(hash, item);
                return hash;
            }

            public T DeepClone() => (T)IElement.DeepClone(this);
            IElement IElement.DeepClone() => DeepClone();
            IElement IElement.ReplaceAll<I>(Func<I, I> func) => ReplaceAll(func);
            IElement IElement.ReplaceAllClone<I>(Func<I, I> func) => ReplaceAllClone(func);
            bool IEquatable<IElement>.Equals(IElement? other) => Equals(other);
        }
    }
    namespace Descriptor
    {
        public interface IDescriptor : Element.IElement
        {
            new IDescriptor DeepClone();
            new IDescriptor ReplaceAll<I>(Func<I, I> func) where I : IDescriptor;
            new IDescriptor ReplaceAllClone<I>(Func<I, I> func) where I : IDescriptor;
        }
        public abstract class IDescriptor<T> : Element.IElement<T>, IDescriptor where T : IDescriptor<T>
        {
            [IgnoreProperty] public string DescriptorFormat => this.ToString() ?? "";
            public override string ToDisplay() => GetType().Name + ": " + DescriptorFormat;

            IDescriptor IDescriptor.DeepClone() => DeepClone();
            IDescriptor IDescriptor.ReplaceAll<I>(Func<I, I> func) => ReplaceAll(func);
            IDescriptor IDescriptor.ReplaceAllClone<I>(Func<I, I> func) => ReplaceAllClone(func);
        }

        public interface IMemberDescriptor : IDescriptor
        {
            [IgnoreProperty] string DescriptorFormat { get; }
        }
        public interface IFieldDescriptor : IDescriptor, IMemberDescriptor { }
        public interface IComponentType : IDescriptor
        {
            [IgnoreProperty] ArrayType Array { get; }
        }
        public interface IReturnType : IDescriptor { }
        public interface IFieldType : IFieldDescriptor, IReturnDescriptor, IComponentType, IParameterDescriptor { }
        public abstract class IFieldType<T> : IDescriptor<T>, IFieldType where T : IFieldType<T>
        {
            [IgnoreProperty] public ArrayType Array => new ArrayType() { Type = this };
        }
        [RegexStruct("(?=(B|C|D|F|I|J|S|Z))", Variable.PROP)] public sealed class BaseType : IFieldType<BaseType>, Signature.ITypeSignature
        {
            public static readonly BaseType Byte = new BaseType('B');
            public static readonly BaseType Char = new BaseType('C');
            public static readonly BaseType Double = new BaseType('D');
            public static readonly BaseType Float = new BaseType('F');
            public static readonly BaseType Int = new BaseType('I');
            public static readonly BaseType Long = new BaseType('J');
            public static readonly BaseType Short = new BaseType('S');
            public static readonly BaseType Boolean = new BaseType('Z');

            private static IEnumerable<(string name, BaseType type)> GetNamedValues()
            {
                yield return ("byte", Byte);
                yield return ("char", Char);
                yield return ("double", Double);
                yield return ("float", Float);
                yield return ("int", Int);
                yield return ("long", Long);
                yield return ("short", Short);
                yield return ("boolean", Boolean);
            }
            public static IEnumerable<BaseType> Types => GetNamedValues().Select(v => v.type);
            public static IEnumerable<string> Names => GetNamedValues().Select(v => v.name);
            public static IReadOnlyDictionary<string, BaseType> NamedTypes => GetNamedValues().ToDictionary(kv => kv.name, kv => kv.type);

            public char Key { get; set; }

            private BaseType() : this('\0') { }
            private BaseType(char key) => this.Key = key;

            public override string ToString() => $"{Key}";

            Signature.ISignature Signature.ISignature.DeepClone() => DeepClone();
            Signature.ISignature Signature.ISignature.ReplaceAll<I>(Func<I, I> func) => ReplaceAll(func);
            Signature.ISignature Signature.ISignature.ReplaceAllClone<I>(Func<I, I> func) => ReplaceAllClone(func);

            IFieldType Signature.IDescriptorSignature<IFieldType>.ToDescriptor(Signature.TypeVariableReader variable) => this;
            IDescriptor Signature.IDescriptorSignature.ToDescriptor(Signature.TypeVariableReader variable) => this;

            string Signature.ISignature.SignatureFormat => DescriptorFormat;
        }
        [RegexStruct("L", Variable.PROP, ";")] public sealed class ObjectType : IFieldType<ObjectType>
        {
            public static ObjectType Create(string class_name) => new ObjectType() { ClassName = class_name };
            public string ClassName { get; set; } = "type";

            public override string ToString() => $"L{ClassName};";
        }
        [RegexStruct("\\[", Variable.PROP)] public sealed class ArrayType : IFieldType<ArrayType>
        {
            public IComponentType Type { get; set; } = BaseType.Byte;

            public override string ToString() => $"[{Type}";
        }
        public interface IReturnDescriptor : IDescriptor { }
        public interface IParameterDescriptor : IDescriptor { }
        [RegexStruct("(V)")] public sealed class VoidDescriptor : IDescriptor<VoidDescriptor>, IReturnDescriptor, IReturnType
        {
            public static VoidDescriptor Instance { get; } = new VoidDescriptor();
            private VoidDescriptor() { }

            public override string ToString() => $"V";
        }
        [RegexStruct("\\(", Variable.ZERO_ARRAY_PROP, "\\)", Variable.PROP)] public sealed class MethodDescriptor : IDescriptor<MethodDescriptor>, IMemberDescriptor
        {
            public IParameterDescriptor[] Parameters { get; set; } = new IParameterDescriptor[0];
            public IReturnDescriptor ReturnDescriptor { get; set; } = BaseType.Byte;

            public override string ToString() => $"({string.Join("", Parameters.Select(v => v.ToString()))}){ReturnDescriptor}";
        }
    }
    namespace Signature
    {
        public interface ISignature : Element.IElement
        {
            [IgnoreProperty] string SignatureFormat { get; }

            new ISignature DeepClone();
            new ISignature ReplaceAll<I>(Func<I, I> func) where I : ISignature;
            new ISignature ReplaceAllClone<I>(Func<I, I> func) where I : ISignature;
        }
        public abstract class ISignature<T> : Element.IElement<T>, ISignature where T : ISignature<T>
        {
            [IgnoreProperty] public string SignatureFormat => this.ToString() ?? "";
            public override string ToDisplay() => GetType().Name + ": " + SignatureFormat;

            ISignature ISignature.DeepClone() => DeepClone();
            ISignature ISignature.ReplaceAll<I>(Func<I, I> func) => ReplaceAll(func);
            ISignature ISignature.ReplaceAllClone<I>(Func<I, I> func) => ReplaceAllClone(func);
        }
        public class TypeVariableReader
        {
            public static TypeVariableReader EMPTY { get; } = new TypeVariableReader();
            private TypeVariableReader() { }
            private TypeVariableReader(IReadOnlyDictionary<string, Descriptor.IFieldType?> types)
            {
                foreach ((string key, Descriptor.IFieldType? type) in types) Types[key] = type;
            }
            private Dictionary<string, Descriptor.IFieldType?> Types { get; } = new Dictionary<string, Descriptor.IFieldType?>();

            public TypeVariableReader Add(string name, Descriptor.IFieldType type)
            {
                TypeVariableReader reader = new TypeVariableReader(Types);
                reader.Types[name] = type;
                return reader;
            }
            public TypeVariableReader Add(IEnumerable<(string name, Descriptor.IFieldType? type)>? values)
            {
                TypeVariableReader reader = new TypeVariableReader(Types);
                if (values == null) return reader;
                foreach ((string name, Descriptor.IFieldType? type) in values) reader.Types[name] = type;
                return reader;
            }
            public TypeVariableReader Add(IEnumerable<FormalTypeParameter>? values) => Add(values?.Select(v => (v.Identifier.Value, v.ClassBound.FieldTypeSignature?.ToDescriptor(this))));
            public Descriptor.IFieldType TryGet(string name, Descriptor.IFieldType? def = null) => (Types.TryGetValue(name, out Descriptor.IFieldType? val) ? val : def) ?? new Descriptor.ObjectType() { ClassName = "java/lang/Object" };
        }
        public interface IDescriptorSignature
        {
            Descriptor.IDescriptor ToDescriptor(TypeVariableReader variable);
        }
        public interface IDescriptorSignature<T> : IDescriptorSignature where T : Descriptor.IDescriptor
        {
            new T ToDescriptor(TypeVariableReader variable);
        }
        public interface IMemberSignature : ISignature { }
        public interface ITypeSignature : ISignature, IMemberSignature, Descriptor.IReturnType, IDescriptorSignature<Descriptor.IFieldType> { }
        public interface IFieldTypeSignature : ISignature, ITypeSignature, IDescriptorSignature<Descriptor.IFieldType> { }
        [RegexStruct(Variable.PROP)] public sealed class FieldTypeSignature : ISignature<FieldTypeSignature>, ISignature, ITypeSignature
        {
            public IFieldTypeSignature Value { get; set; } = new ClassTypeSignature();

            public Descriptor.IFieldType ToDescriptor(TypeVariableReader variable) => Value.ToDescriptor(variable);

            public override string ToString() => $"{Value}";

            Descriptor.IDescriptor Descriptor.IDescriptor.DeepClone() => DeepClone();
            Descriptor.IDescriptor Descriptor.IDescriptor.ReplaceAll<I>(Func<I, I> func) => ReplaceAll(func);
            Descriptor.IDescriptor Descriptor.IDescriptor.ReplaceAllClone<I>(Func<I, I> func) => ReplaceAllClone(func);
            Descriptor.IDescriptor IDescriptorSignature.ToDescriptor(TypeVariableReader variable) => ToDescriptor(variable);
        }
        [RegexStruct(false, Variable.PROP, @"(?=\.|;|\[|\/|<|>|:|$)")] public sealed class Identifier : ISignature<Identifier>, ISignature
        {
            public static Identifier Create(string value) => new Identifier() { Value = value };

            public string Value { get; set; } = "identifier";

            public override string ToString() => $"{Value}";
            public static implicit operator Identifier(string value) => Create(value);
        }
        [RegexStruct(Variable.PROP, Variable.PROP, Variable.ZERO_ARRAY_PROP)] public sealed class ClassSignature : ISignature<ClassSignature>, ISignature
        {
            [NullableProperty] public FormalTypeParameters? FormalTypeParameters { get; set; } = null;
            public SuperclassSignature SuperclassSignature { get; set; } = new SuperclassSignature();
            public List<SuperinterfaceSignature> SuperinterfaceSignature { get; set; } = new List<SuperinterfaceSignature>();

            public override string ToString() => $"{FormalTypeParameters?.ToString() ?? ""}{SuperclassSignature}{string.Join("", SuperinterfaceSignature.Select(v => v.ToString()))}";
        }
        [RegexStruct("\\<", Variable.ONE_ARRAY_PROP, "\\>")] public sealed class FormalTypeParameters : ISignature<FormalTypeParameters>, ISignature
        {
            public FormalTypeParameter[] FormalTypeParameter { get; set; } = new FormalTypeParameter[] { new FormalTypeParameter() };

            public override string ToString() => $"<{string.Join("", FormalTypeParameter.Select(v => v.ToString()))}>";
        }
        [RegexStruct(Variable.PROP, Variable.PROP, Variable.ZERO_ARRAY_PROP)] public sealed class FormalTypeParameter : ISignature<FormalTypeParameter>, ISignature
        {
            public Identifier Identifier { get; set; } = new Identifier();
            public ClassBound ClassBound { get; set; } = new ClassBound();
            public InterfaceBound[] InterfaceBound { get; set; } = new InterfaceBound[0];

            public override string ToString() => $"{Identifier}{ClassBound}{string.Join("", InterfaceBound.Select(v => v.ToString()))}";
        }
        [RegexStruct("\\:", Variable.PROP)] public sealed class ClassBound : ISignature<ClassBound>, ISignature
        {
            [NullableProperty] public FieldTypeSignature? FieldTypeSignature { get; set; } = null;

            public override string ToString() => $":{FieldTypeSignature?.ToString() ?? ""}";
        }
        [RegexStruct("\\:", Variable.PROP)] public sealed class InterfaceBound : ISignature<InterfaceBound>, ISignature
        {
            public FieldTypeSignature FieldTypeSignature { get; set; } = new FieldTypeSignature();

            public override string ToString() => $":{FieldTypeSignature}";
        }
        [RegexStruct(Variable.PROP)] public sealed class SuperclassSignature : ISignature<SuperclassSignature>, ISignature
        {
            public ClassTypeSignature ClassTypeSignature { get; set; } = new ClassTypeSignature();

            public override string ToString() => $"{ClassTypeSignature}";
        }
        [RegexStruct(Variable.PROP)] public sealed class SuperinterfaceSignature : ISignature<SuperinterfaceSignature>, ISignature
        {
            public ClassTypeSignature ClassTypeSignature { get; set; } = new ClassTypeSignature();

            public override string ToString() => $"{ClassTypeSignature}";
        }
        [RegexStruct("L", Variable.PROP, Variable.PROP, Variable.ZERO_ARRAY_PROP, ";")] public sealed class ClassTypeSignature : ISignature<ClassTypeSignature>, IFieldTypeSignature
        {
            [NullableProperty] public PackageSpecifier? PackageSpecifier { get; set; } = null;
            public SimpleClassTypeSignature SimpleClassTypeSignature { get; set; } = new SimpleClassTypeSignature();
            public ClassTypeSignatureSuffix[] ClassTypeSignatureSuffix { get; set; } = new ClassTypeSignatureSuffix[0];

            [IgnoreProperty] public string FullSimplePackage => $"{PackageSpecifier?.FullPackage ?? ""}{SimpleClassTypeSignature.Identifier.Value}";

            public Descriptor.IFieldType ToDescriptor(TypeVariableReader variable) => new Descriptor.ObjectType() { ClassName = FullSimplePackage };

            public override string ToString() => $"L{PackageSpecifier?.ToString() ?? ""}{SimpleClassTypeSignature}{string.Join("", ClassTypeSignatureSuffix.Select(v => v.ToString()))};";

            Descriptor.IDescriptor Descriptor.IDescriptor.DeepClone() => DeepClone();
            Descriptor.IDescriptor Descriptor.IDescriptor.ReplaceAll<I>(Func<I, I> func) => ReplaceAll(func);
            Descriptor.IDescriptor Descriptor.IDescriptor.ReplaceAllClone<I>(Func<I, I> func) => ReplaceAllClone(func);
            Descriptor.IDescriptor IDescriptorSignature.ToDescriptor(TypeVariableReader variable) => ToDescriptor(variable);
        }
        [RegexStruct(Variable.PROP, @"\/", Variable.PROP)] public sealed class PackageSpecifier : ISignature<PackageSpecifier>, ISignature
        {
            public Identifier Identifier { get; set; } = new Identifier();
            [NullableProperty] public PackageSpecifier? NextPackageSpecifier { get; set; } = null;

            public override string ToString() => $"{Identifier}/{NextPackageSpecifier}";
            [IgnoreProperty] public IEnumerable<string> FullPackageArgs
            {
                get
                {
                    yield return Identifier.Value;
                    if (NextPackageSpecifier == null) yield break;
                    foreach (string arg in NextPackageSpecifier.FullPackageArgs) yield return arg;
                }
            }
            [IgnoreProperty] public string FullPackage => string.Join("/", FullPackageArgs) + "/";

            public static implicit operator PackageSpecifier?(string[] args)
            {
                PackageSpecifier? specifier = null;
                for (int i = args.Length - 1; i >= 0; i--)
                    specifier = new PackageSpecifier() { Identifier = args[i], NextPackageSpecifier = specifier };
                return specifier;
            }
        }
        [RegexStruct(Variable.PROP, Variable.PROP)] public sealed class SimpleClassTypeSignature : ISignature<SimpleClassTypeSignature>, ISignature
        {
            public Identifier Identifier { get; set; } = new Identifier();
            [NullableProperty] public TypeArguments? TypeArguments { get; set; } = null;

            public override string ToString() => $"{Identifier}{TypeArguments?.ToString() ?? ""}";
        }
        [RegexStruct(@"(\.)", Variable.PROP)] public sealed class ClassTypeSignatureSuffix : ISignature<ClassTypeSignatureSuffix>, ISignature
        {
            public SimpleClassTypeSignature SimpleClassTypeSignature { get; set; } = new SimpleClassTypeSignature();

            public override string ToString() => $".{SimpleClassTypeSignature}";
        }
        [RegexStruct("T", Variable.PROP, ";")] public sealed class TypeVariableSignature : ISignature<TypeVariableSignature>, IFieldTypeSignature
        {
            public Identifier Identifier { get; set; } = new Identifier();

            public Descriptor.IFieldType ToDescriptor(TypeVariableReader variable) => variable.TryGet(Identifier.Value);

            public override string ToString() => $"T{Identifier};";

            Descriptor.IDescriptor Descriptor.IDescriptor.DeepClone() => DeepClone();
            Descriptor.IDescriptor Descriptor.IDescriptor.ReplaceAll<I>(Func<I, I> func) => ReplaceAll(func);
            Descriptor.IDescriptor Descriptor.IDescriptor.ReplaceAllClone<I>(Func<I, I> func) => ReplaceAllClone(func);
            Descriptor.IDescriptor IDescriptorSignature.ToDescriptor(TypeVariableReader variable) => ToDescriptor(variable);
        }
        [RegexStruct("<", Variable.ONE_ARRAY_PROP, ">")] public sealed class TypeArguments : ISignature<TypeArguments>, ISignature
        {
            public ITypeArgument[] TypeArgument { get; set; } = new ITypeArgument[1] { AnyTypeArgument.Instance };

            public override string ToString() => $"<{string.Join("", TypeArgument.Select(v => v.ToString()))}>";
        }
        public interface ITypeArgument : ISignature { }
        [RegexStruct(Variable.PROP, Variable.PROP)] public sealed class TypeArgument : ISignature<TypeArgument>, ITypeArgument
        {
            [NullableProperty] public WildcardIndicator? WildcardIndicator { get; set; } = null;
            public FieldTypeSignature FieldTypeSignature { get; set; } = new FieldTypeSignature();

            public override string ToString() => $"{WildcardIndicator?.ToString() ?? ""}{FieldTypeSignature}";
        }
        [RegexStruct(@"(\*)")] public sealed class AnyTypeArgument : ISignature<AnyTypeArgument>, ITypeArgument
        {
            public static AnyTypeArgument Instance { get; } = new AnyTypeArgument();
            private AnyTypeArgument() { }

            public override string ToString() => $"*";
        }
        [RegexStruct(@"(?=(\+|\-))", Variable.PROP)] public sealed class WildcardIndicator : ISignature<WildcardIndicator>, ISignature
        {
            public char Indicator { get; set; } = '+';

            public override string ToString() => $"{Indicator}";
        }
        [RegexStruct("\\[", Variable.PROP)] public sealed class ArrayTypeSignature : ISignature<ArrayTypeSignature>, IFieldTypeSignature
        {
            public ITypeSignature TypeSignature { get; set; } = Data.Descriptor.BaseType.Byte;

            public Descriptor.IFieldType ToDescriptor(TypeVariableReader variable) => TypeSignature.ToDescriptor(variable).Array;

            public override string ToString() => $"[{TypeSignature}";

            Descriptor.IDescriptor Descriptor.IDescriptor.DeepClone() => DeepClone();
            Descriptor.IDescriptor Descriptor.IDescriptor.ReplaceAll<I>(Func<I, I> func) => ReplaceAll(func);
            Descriptor.IDescriptor Descriptor.IDescriptor.ReplaceAllClone<I>(Func<I, I> func) => ReplaceAllClone(func);
            Descriptor.IDescriptor IDescriptorSignature.ToDescriptor(TypeVariableReader variable) => ToDescriptor(variable);
        }
        [RegexStruct(Variable.PROP, @"\(", Variable.ZERO_ARRAY_PROP, @"\)", Variable.PROP, Variable.ZERO_ARRAY_PROP)] public sealed class MethodTypeSignature : ISignature<MethodTypeSignature>, IMemberSignature, IDescriptorSignature<Descriptor.MethodDescriptor>
        {
            [NullableProperty] public FormalTypeParameters? FormalTypeParameters { get; set; } = null;
            public ITypeSignature[] TypeSignature { get; set; } = new ITypeSignature[0];
            public Descriptor.IReturnType ReturnType { get; set; } = Data.Descriptor.BaseType.Byte;
            public IThrowsSignature[] ThrowsSignature { get; set; } = new IThrowsSignature[0];

            public Descriptor.MethodDescriptor ToDescriptor(TypeVariableReader variable)
            {
                variable = variable.Add(FormalTypeParameters?.FormalTypeParameter);
                return new Data.Descriptor.MethodDescriptor()
                {
                    Parameters = TypeSignature.Select(v => v.ToDescriptor(variable)).ToArray(),
                    ReturnDescriptor = ReturnType is Descriptor.IReturnDescriptor _void
                    ? _void
                    : ReturnType is IDescriptorSignature<Descriptor.IFieldType> field
                        ? field.ToDescriptor(variable)
                        : throw new ArgumentException($"Can't convert '{ReturnType.GetType().FullName}' to {typeof(Descriptor.IReturnDescriptor).FullName}")
                };
            }

            public override string ToString() => $"{FormalTypeParameters?.ToString() ?? ""}({string.Join("", TypeSignature.Select(v => v.ToString()))}){ReturnType}{string.Join("", ThrowsSignature.Select(v => v.ToString()))}";
            Descriptor.IDescriptor IDescriptorSignature.ToDescriptor(TypeVariableReader variable) => ToDescriptor(variable);
        }
        public interface IThrowsSignature : ISignature { }
        [RegexStruct(@"\^", Variable.PROP)] public sealed class ClassThrowsSignature : ISignature<ClassThrowsSignature>, IThrowsSignature
        {
            public ClassTypeSignature ClassTypeSignature { get; set; } = new ClassTypeSignature();

            public override string ToString() => $"^{ClassTypeSignature}";
        }
        [RegexStruct(@"\^", Variable.PROP)] public sealed class VariableThrowsSignature : ISignature<VariableThrowsSignature>, IThrowsSignature
        {
            public TypeVariableSignature TypeVariableSignature { get; set; } = new TypeVariableSignature();

            public override string ToString() => $"^{TypeVariableSignature}";
        }
    }
}
