#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Java.Net.Data.Descriptor;

public class ClassObject
{
    internal const string PROP = "\u0000#VALUE";
    internal const string ZERO_ARRAY_PROP = "\u0000#ARRAY#0";
    internal const string ONE_ARRAY_PROP = "\u0000#ARRAY#1";

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
        bool DEBUG = false;//text == "()V";
        result = default;
        string part_text = text;
        int field_index = 0;
        List<object?> values = new List<object?>();
        string[] formats = Attribute.Format;
        int length = formats.Length;

        NullabilityInfoContext _nullabilityContext = new NullabilityInfoContext();

        if (DEBUG) Console.WriteLine(Prefix + "Create: " + Type.FullName);
        prefix_index++;
        for (int i = 0; i < length; i++)
        {
            string format = formats[i];

            bool TryReadField(out object? value, bool step_field) => TryReadFieldType(out value, out _, step_field);
            bool TryReadFieldType(out object? value, out Type? type, bool step_field)
            {
                value = default;
                PropertyInfo property = Properties[field_index];
                if (DEBUG) Console.WriteLine(Prefix + "Field: " + property.Name);
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
                    isNullable = _nullabilityContext.Create(property).WriteState is NullabilityState.Nullable;
                }
                else
                {
                    isNullable = true;
                    type = underlyingType;
                }
                foreach (ClassObject cobject in IDescriptor.TryGetAll(type))
                    if (cobject.TryRegexCreator(ref part_text, out value))
                        return true;
                if (!isNullable) return false;
                value = null;
                return true;
            }

            if (DEBUG) Console.WriteLine(Prefix + "Format: " + format);
            switch (format)
            {
                case PROP:
                    {
                        if (!TryReadField(out object? value, true))
                        {
                            prefix_index--;
                            if (DEBUG) Console.WriteLine(Prefix + "IGNORED#F: " + Type.FullName);
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
                            if (DEBUG) Console.WriteLine(Prefix + "IGNORED#A: " + Type.FullName);
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
                        if (DEBUG) Console.WriteLine(Prefix + "Check regex: " + regex);
                        if (DEBUG) Console.WriteLine(Prefix + "Text: " + part_text);
                        MatchCollection collection = regex.Matches(part_text);
                        if (collection.Count == 0)
                        {
                            prefix_index--;
                            if (DEBUG) Console.WriteLine(Prefix + "IGNORED: Regex match empty");
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
                        if (DEBUG) Console.WriteLine(Prefix + "Out: " + part_text);
                        break;
                    }
            }
        }

        length = values.Count;
        if (length != Properties.Count)
        {
            prefix_index--;
            if (DEBUG) Console.WriteLine(Prefix + "IGNORED#L: " + Type.FullName);
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
        if (DEBUG) Console.WriteLine(Prefix + "CREATED: " + Type.FullName);
        return true;
    }
}
