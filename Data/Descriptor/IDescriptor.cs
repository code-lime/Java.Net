#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Java.Net.Data.Descriptor;

public partial interface IDescriptor
{
    [IgnoreProperty] string DescriptorFormat { get; }

    [GeneratedRegex("{(\\d+)}", RegexOptions.Compiled)]
    private static partial Regex RegexArgs();

    private static string ReplaceArgs(string text, List<string> args)
        => ReplaceArgs(text, v => args[v]);
    private static string ReplaceArgs(string text, Func<int, string> func)
        => RegexArgs().Replace(text, match => func.Invoke(int.Parse(match.Groups[1].Value)));

    private static readonly IReadOnlyDictionary<Type, ClassObject> classes = GetAllAttributes<RegexStructAttribute>().ToDictionary(kv => kv.type, kv => new ClassObject(kv.type, kv.attribute));
    static IEnumerable<(Type type, T attribute)> GetAllAttributes<T>() where T : System.Attribute
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var type in assembly.GetTypes())
                foreach (var attribute in type.GetCustomAttributes<T>(true))
                    yield return (type, attribute);
    }
    public static bool TryGet(Type type, [NotNullWhen(true)] out ClassObject? dat)
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
    public static bool TryGetNative(Type type, [NotNullWhen(true)] out ClassObject? dat)
        => classes.TryGetValue(type, out dat);
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

}

public abstract class IDescriptor<T> : IDescriptor where T : IDescriptor<T>
{
    [IgnoreProperty] public string DescriptorFormat => ToString() ?? "";
    [IgnoreProperty] public string Display => this.ToDisplay();

    public override bool Equals(object? obj) => obj is IDescriptor other && other.IsEquals(this);
    public override int GetHashCode() => DescriptorFormat.GetHashCode();
}
