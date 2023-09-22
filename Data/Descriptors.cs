#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Java.Net.Descriptor
{
    using Field;
    using Method;
    using System.Text.RegularExpressions;

    public interface IDescriptor
    {
        private static readonly Regex re = new Regex(@"{(\d+)}", RegexOptions.Compiled);
        private static string ReplaceArgs(string text, List<string> args)
            => ReplaceArgs(text, v => args[v]);
        private static string ReplaceArgs(string text, Func<int, string> func)
            => re.Replace(text, match => func.Invoke(int.Parse(match.Groups[1].Value)));

        public class ClassObject
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

            private static int prefix_index = 0;
            private static string Prefix => new string(' ', prefix_index * 3);
            public bool TryRegexCreator(ref string text, out object? result)
            {
                result = default;
                string part_text = text;
                int field_index = 0;
                List<object?> values = new List<object?>();
                string[] formats = Attribute.Format;
                int length = formats.Length;

                Console.WriteLine(Prefix + "Create: " + Type.FullName);
                prefix_index++;
                for (int i = 0; i < length; i++)
                {
                    string format = formats[i];

                    bool TryReadField(out object? value, bool step_field) => TryReadFieldType(out value, out _, step_field);
                    bool TryReadFieldType(out object? value, out Type? type, bool step_field)
                    {
                        value = default;
                        PropertyInfo property = Properties[field_index];
                        Console.WriteLine(Prefix + "Field: " + property.Name);
                        if (step_field) field_index++;
                        type = property.PropertyType;
                        if (type.IsArray) type = type.GetElementType();
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
                                case PROP:
                                case ONE_ARRAY_PROP:
                                case ZERO_ARRAY_PROP: return false;
                            }
                            MatchCollection collection = new Regex(next).Matches(part_text);
                            if (collection.Count == 0) return false;
                            int index = collection[0].Index;
                            value = part_text[..index];
                            part_text = part_text[index..];
                            return true;
                        }
                        if (type == null) return false;
                        Type? underlyingType = Nullable.GetUnderlyingType(type);
                        bool isNullable;
                        if (underlyingType == null)
                        {
                            if (part_text.Length == 0) return false;
                            isNullable = false;
                        }
                        else
                        {
                            isNullable = true;
                            type = underlyingType;
                        }
                        foreach (ClassObject cobject in TryGetAll(type))
                            if (cobject.TryRegexCreator(ref part_text, out value))
                                return true;
                        if (!isNullable) return false;
                        value = null;
                        return true;
                    }

                    Console.WriteLine(Prefix + "Format: " + format);
                    switch (format)
                    {
                        case PROP:
                            {
                                if (!TryReadField(out object? value, true))
                                {
                                    prefix_index--;
                                    Console.WriteLine(Prefix + "IGNORED#F: " + Type.FullName);
                                    return false;
                                }
                                values.Add(value);
                                break;
                            }
                        case ZERO_ARRAY_PROP:
                        case ONE_ARRAY_PROP:
                            {
                                List<object?> list = new List<object?>();
                                Type? type;
                                while (TryReadFieldType(out object? value, out type, false)) list.Add(value);
                                int _length = list.Count;
                                if (format == ONE_ARRAY_PROP && _length == 0)
                                {
                                    prefix_index--;
                                    Console.WriteLine(Prefix + "IGNORED#A: " + Type.FullName);
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
                                Console.WriteLine(Prefix + "Check regex: " + regex);
                                Console.WriteLine(Prefix + "Text: " + part_text);
                                MatchCollection collection = regex.Matches(part_text);
                                if (collection.Count == 0)
                                {
                                    prefix_index--;
                                    Console.WriteLine(Prefix + "IGNORED: Regex match empty");
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
                                Console.WriteLine(Prefix + "Out: " + part_text);
                                break;
                            }
                    }
                }

                length = values.Count;
                if (length != Properties.Count)
                {
                    prefix_index--;
                    Console.WriteLine(Prefix + "IGNORED#L: " + Type.FullName);
                    return false;
                }

                result = Activator.CreateInstance(Type, true);
                for (int i = 0; i < length; i++)
                {
                    PropertyInfo property = Properties[i];
                    property.SetValue(result, values[i]);
                }
                text = part_text;
                prefix_index--;
                Console.WriteLine(Prefix + "CREATED: " + Type.FullName);
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
        static IDescriptor()
        {
            foreach ((Type type, RegexStructAttribute attribute) in GetAllAttributes<RegexStructAttribute>())
            {
                Console.WriteLine($"Load type '{type.FullName}' with '{string.Join(" & ", attribute.Format)}'");
                classes.Add(type, new ClassObject(type, attribute));
            }
        }
        public static bool TryGet(Type type, out ClassObject? dat)
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
        public static IEnumerable<ClassObject> TryGetAll(Type type)
        {
            foreach ((Type ctype, ClassObject cobject) in classes)
            {
                if (!type.IsAssignableFrom(ctype)) continue;
                yield return cobject;
            }
        }

        public static T TryParse<T>(string text, bool full = true) where T : IDescriptor => TryParse(text, out T? value, full) && value != null ? value : throw new ArgumentException($"Error parse {text} to class '{typeof(T)}'");
        public static bool TryParse<T>(string text, out T? descriptor, bool full = true) where T : IDescriptor
        {
            descriptor = default;
            Type type = typeof(T);
            foreach ((Type ctype, ClassObject cobject) in classes)
            {
                if (!type.IsAssignableFrom(ctype)) continue;
                string part_text = text;
                if (!cobject.TryRegexCreator(ref part_text, out object? value)) continue;
                if (full && part_text.Length > 0) continue;
                descriptor = (T?)value;
                return true;
            }
            return false;
        }
        public static IDescriptor TryParseAny(string text, bool full = true) => TryParse<IDescriptor>(text, full);
        public static bool TryParseAny(string text, out IDescriptor? descriptor, bool full = true) => TryParse(text, out descriptor, full);

        internal const string PROP = "\u0000#VALUE";
        internal const string ZERO_ARRAY_PROP = "\u0000#ARRAY#0";
        internal const string ONE_ARRAY_PROP = "\u0000#ARRAY#1";
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)] public class RegexStructAttribute : Attribute
        {
            public string[] Format { get; }
            public RegexStructAttribute(params string[] format) => this.Format = format;
        }
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] public class IgnorePropertyAttribute : Attribute { }

        [IgnoreProperty] string DescriptorFormat { get; }
    }
    public abstract class IDescriptor<T> : IDescriptor where T : IDescriptor<T>
    {
        [IDescriptor.IgnoreProperty] public string DescriptorFormat => this.ToString()??"";
        [IDescriptor.IgnoreProperty] public string Display => ToDisplay();
        public virtual string ToDisplay() => GetType().Name + ": " + DescriptorFormat;
    }
    public static class IDescriptorExt
    {
        public static string ToDisplay<T>(this T descriptor) where T : IDescriptor => descriptor.GetType().Name + ": " + descriptor.DescriptorFormat;
        public static string ToFormat<T>(this T descriptor) where T : IDescriptor => descriptor.DescriptorFormat;
    }

    namespace Field
    {
        public interface IFieldDescriptor : IDescriptor { }
        public interface IComponentType : IDescriptor
        {
            [IDescriptor.IgnoreProperty] ArrayType Array { get; }
        }
        public interface IReturnType : IDescriptor { }
        public interface ITypeSignature : IDescriptor, IReturnType { }
        public interface IFieldType : IFieldDescriptor, IReturnDescriptor, IComponentType, IParameterDescriptor { }
        public abstract class IFieldType<T> : IDescriptor<T>, IFieldType where T : IFieldType<T>
        {
            [IDescriptor.IgnoreProperty] public ArrayType Array => new ArrayType() { Type = this };
        }
        [IDescriptor.RegexStruct("(?=(B|C|D|F|I|J|S|Z))", IDescriptor.PROP)]
        public sealed class BaseType : IFieldType<BaseType>, ITypeSignature
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
            private BaseType(char key) => this.Key = key;

            public override string ToString() => $"{Key}";
        }
        [IDescriptor.RegexStruct("L", IDescriptor.PROP, ";")]
        public sealed class ObjectType : IFieldType<ObjectType>
        {
            public static ObjectType Create(string class_name) => new ObjectType() { ClassName = class_name };
            public string ClassName { get; set; } = "type";

            public override string ToString() => $"L{ClassName};";
        }
        [IDescriptor.RegexStruct("\\[", IDescriptor.PROP)]
        public sealed class ArrayType : IFieldType<ArrayType>
        {
            public IComponentType Type { get; set; } = BaseType.Byte;

            public override string ToString() => $"[{Type}";
        }
    }
    namespace Method
    {
        public interface IReturnDescriptor : IDescriptor { }
        public interface IParameterDescriptor : IDescriptor { }
        [IDescriptor.RegexStruct("(V)")]
        public sealed class VoidDescriptor : IDescriptor<VoidDescriptor>, IReturnDescriptor, IReturnType
        {
            public static VoidDescriptor Instance { get; } = new VoidDescriptor();

            public override string ToString() => $"V";
        }
        [IDescriptor.RegexStruct("\\(", IDescriptor.ZERO_ARRAY_PROP, "\\)", IDescriptor.PROP)]
        public sealed class MethodDescriptor : IDescriptor<MethodDescriptor>, IDescriptor
        {
            public IParameterDescriptor[] Parameters { get; set; } = new IParameterDescriptor[0];
            public IReturnDescriptor ReturnDescriptor { get; set; } = BaseType.Byte;

            public override string ToString() => $"({string.Join("", Parameters.Select(v => v.ToString()))}){ReturnDescriptor}";
        }
    }
    namespace Class
    {
        public interface IFieldTypeSignature : IDescriptor, ITypeSignature { }
        [IDescriptor.RegexStruct(IDescriptor.PROP)]
        public sealed class FieldTypeSignature : IDescriptor<FieldTypeSignature>, IDescriptor, ITypeSignature
        {
            public IFieldTypeSignature Value { get; set; } = new ClassTypeSignature();

            public override string ToString() => $"{Value}";
        }
        [IDescriptor.RegexStruct(IDescriptor.PROP, @"(?=\.|;|\[|\/|<|>|:|$)")]
        public sealed class Identifier : IDescriptor<Identifier>, IDescriptor
        {
            public static Identifier Create(string value) => new Identifier() { Value = value };

            public string Value { get; set; } = "identifier";

            public override string ToString() => $"{Value}";
        }
        [IDescriptor.RegexStruct(IDescriptor.PROP, IDescriptor.PROP, IDescriptor.ZERO_ARRAY_PROP)]
        public sealed class ClassSignature : IDescriptor<ClassSignature>, IDescriptor
        {
            public FormalTypeParameters? FormalTypeParameters { get; set; } = null;
            public SuperclassSignature SuperclassSignature { get; set; } = new SuperclassSignature();
            public SuperinterfaceSignature[] SuperinterfaceSignature { get; set; } = new SuperinterfaceSignature[0];

            public override string ToString() => $"{FormalTypeParameters?.ToString() ?? ""}{SuperclassSignature}{string.Join("", SuperinterfaceSignature.Select(v => v.ToString()))}";
        }
        [IDescriptor.RegexStruct("\\<", IDescriptor.ONE_ARRAY_PROP, "\\>")]
        public sealed class FormalTypeParameters : IDescriptor<FormalTypeParameters>, IDescriptor
        {
            public FormalTypeParameter[] FormalTypeParameter { get; set; } = new FormalTypeParameter[] { new FormalTypeParameter() };

            public override string ToString() => $"<{string.Join("", FormalTypeParameter.Select(v => v.ToString()))}>";
        }
        [IDescriptor.RegexStruct(IDescriptor.PROP, IDescriptor.PROP, IDescriptor.ZERO_ARRAY_PROP)]
        public sealed class FormalTypeParameter : IDescriptor<FormalTypeParameter>, IDescriptor
        {
            public Identifier Identifier { get; set; } = new Identifier();
            public ClassBound ClassBound { get; set; } = new ClassBound();
            public InterfaceBound[] InterfaceBound { get; set; } = new InterfaceBound[0];

            public override string ToString() => $"{Identifier}{ClassBound}{string.Join("", InterfaceBound.Select(v => v.ToString()))}";
        }
        [IDescriptor.RegexStruct("\\:", IDescriptor.PROP)]
        public sealed class ClassBound : IDescriptor<ClassBound>, IDescriptor
        {
            public FieldTypeSignature? FieldTypeSignature { get; set; } = null;

            public override string ToString() => $":{FieldTypeSignature?.ToString() ?? ""}";
        }
        [IDescriptor.RegexStruct("\\:", IDescriptor.PROP)]
        public sealed class InterfaceBound : IDescriptor<InterfaceBound>, IDescriptor
        {
            public FieldTypeSignature FieldTypeSignature { get; set; } = new FieldTypeSignature();

            public override string ToString() => $":{FieldTypeSignature}";
        }
        [IDescriptor.RegexStruct(IDescriptor.PROP)]
        public sealed class SuperclassSignature : IDescriptor<SuperclassSignature>, IDescriptor
        {
            public ClassTypeSignature ClassTypeSignature { get; set; } = new ClassTypeSignature();

            public override string ToString() => $"{ClassTypeSignature}";
        }
        [IDescriptor.RegexStruct(IDescriptor.PROP)]
        public sealed class SuperinterfaceSignature : IDescriptor<SuperinterfaceSignature>, IDescriptor
        {
            public ClassTypeSignature ClassTypeSignature { get; set; } = new ClassTypeSignature();

            public override string ToString() => $"{ClassTypeSignature}";
        }
        [IDescriptor.RegexStruct("L", IDescriptor.PROP, IDescriptor.PROP, IDescriptor.ZERO_ARRAY_PROP, ";")]
        public sealed class ClassTypeSignature : IDescriptor<ClassTypeSignature>, IFieldTypeSignature
        {
            public PackageSpecifier? PackageSpecifier { get; set; } = null;
            public SimpleClassTypeSignature SimpleClassTypeSignature { get; set; } = new SimpleClassTypeSignature();
            public ClassTypeSignatureSuffix[] ClassTypeSignatureSuffix { get; set; } = new ClassTypeSignatureSuffix[0];

            public override string ToString() => $"L{PackageSpecifier?.ToString() ?? ""}{SimpleClassTypeSignature}{string.Join("", ClassTypeSignatureSuffix.Select(v => v.ToString()))};";
        }
        [IDescriptor.RegexStruct(IDescriptor.PROP, @"\/", IDescriptor.PROP)]
        public sealed class PackageSpecifier : IDescriptor<PackageSpecifier>, IDescriptor
        {
            public Identifier Identifier { get; set; } = new Identifier();
            public PackageSpecifier[] PackageSpecifiers { get; set; } = new PackageSpecifier[0];

            public override string ToString() => $"{Identifier}/{string.Join("", PackageSpecifiers.Select(v => v.ToString()))}";
        }
        [IDescriptor.RegexStruct(IDescriptor.PROP, IDescriptor.PROP)]
        public sealed class SimpleClassTypeSignature : IDescriptor<SimpleClassTypeSignature>, IDescriptor
        {
            public Identifier Identifier { get; set; } = new Identifier();
            public TypeArguments? TypeArguments { get; set; } = null;

            public override string ToString() => $"{Identifier}{TypeArguments?.ToString() ?? ""}";
        }
        [IDescriptor.RegexStruct(@"(\.)", IDescriptor.PROP)]
        public sealed class ClassTypeSignatureSuffix : IDescriptor<ClassTypeSignatureSuffix>, IDescriptor
        {
            public SimpleClassTypeSignature SimpleClassTypeSignature { get; set; } = new SimpleClassTypeSignature();

            public override string ToString() => $".{SimpleClassTypeSignature}";
        }
        [IDescriptor.RegexStruct("T", IDescriptor.PROP, ";")]
        public sealed class TypeVariableSignature : IDescriptor<TypeVariableSignature>, IFieldTypeSignature
        {
            public Identifier Identifier { get; set; } = new Identifier();

            public override string ToString() => $"T{Identifier};";
        }
        [IDescriptor.RegexStruct("<", IDescriptor.ONE_ARRAY_PROP, ">")]
        public sealed class TypeArguments : IDescriptor<TypeArguments>, IDescriptor
        {
            public ITypeArgument[] TypeArgument { get; set; } = new ITypeArgument[1] { new AnyTypeArgument() };

            public override string ToString() => $"<{TypeArgument.Select(v => v.ToString())}>";
        }
        public interface ITypeArgument : IDescriptor { }
        [IDescriptor.RegexStruct(IDescriptor.PROP, IDescriptor.PROP)]
        public sealed class TypeArgument : IDescriptor<TypeArgument>, ITypeArgument
        {
            public WildcardIndicator? WildcardIndicator { get; set; } = null;
            public FieldTypeSignature FieldTypeSignature { get; set; } = new FieldTypeSignature();

            public override string ToString() => $"{WildcardIndicator?.ToString() ?? ""}{FieldTypeSignature}";
        }
        [IDescriptor.RegexStruct(@"(\*)")]
        public sealed class AnyTypeArgument : IDescriptor<AnyTypeArgument>, ITypeArgument
        {
            public static AnyTypeArgument Instance { get; } = new AnyTypeArgument();

            public override string ToString() => $"*";
        }
        [IDescriptor.RegexStruct(@"(?=(\+|\-))", IDescriptor.PROP)]
        public sealed class WildcardIndicator : IDescriptor<WildcardIndicator>, IDescriptor
        {
            public char Indicator { get; set; } = '+';

            public override string ToString() => $"{Indicator}";
        }
        [IDescriptor.RegexStruct("\\[", IDescriptor.PROP)]
        public sealed class ArrayTypeSignature : IDescriptor<ArrayTypeSignature>, IFieldTypeSignature
        {
            public ITypeSignature TypeSignature { get; set; } = BaseType.Byte;

            public override string ToString() => $"[{TypeSignature}";
        }
        [IDescriptor.RegexStruct(IDescriptor.PROP, @"\(", IDescriptor.ZERO_ARRAY_PROP, @"\)", IDescriptor.PROP, IDescriptor.ZERO_ARRAY_PROP)]
        public sealed class MethodTypeSignature : IDescriptor<MethodTypeSignature>, IDescriptor
        {
            public FormalTypeParameters? FormalTypeParameters { get; set; } = null;
            public ITypeSignature[] TypeSignature { get; set; } = new ITypeSignature[0];
            public IReturnType ReturnType { get; set; } = BaseType.Byte;
            public IThrowsSignature[] ThrowsSignature { get; set; } = new IThrowsSignature[0];

            public override string ToString() => $"{FormalTypeParameters?.ToString() ?? ""}({TypeSignature.Select(v => v.ToString())}){ReturnType}{ThrowsSignature.Select(v => v.ToString())}";
        }
        public interface IThrowsSignature : IDescriptor { }
        [IDescriptor.RegexStruct(@"\^", IDescriptor.PROP)]
        public sealed class ClassThrowsSignature : IDescriptor<ClassThrowsSignature>, IThrowsSignature
        {
            public ClassTypeSignature ClassTypeSignature { get; set; } = new ClassTypeSignature();

            public override string ToString() => $"^{ClassTypeSignature}";
        }
        [IDescriptor.RegexStruct(@"\^", IDescriptor.PROP)]
        public sealed class VariableThrowsSignature : IDescriptor<VariableThrowsSignature>, IThrowsSignature
        {
            public TypeVariableSignature TypeVariableSignature { get; set; } = new TypeVariableSignature();

            public override string ToString() => $"^{TypeVariableSignature}";
        }
    }
}
