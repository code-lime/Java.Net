using System.Collections.Generic;
using System.IO;

namespace Java.Net;

internal static class Utils
{
    internal static byte[] ReadAllBytes(this Stream instream)
    {
        if (instream is MemoryStream memory) return memory.ToArray();
        using var memoryStream = new MemoryStream();
        instream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
    internal static void WriteAllBytes(this Stream outstream, byte[] bytes) => outstream.Write(bytes);
    public static IEnumerable<Mono.Cecil.TypeDefinition> GetAllTypes(this Mono.Cecil.ModuleDefinition module)
    {
        foreach (var type in module.Types)
        {
            yield return type;
            foreach (var ret in type.GetAllTypes())
                yield return ret;
        }
    }
    public static IEnumerable<Mono.Cecil.TypeDefinition> GetAllTypes(this Mono.Cecil.TypeDefinition type)
    {
        foreach (var nested in type.NestedTypes)
        {
            yield return nested;
            foreach (var ret in nested.GetAllTypes())
                yield return ret;
        }
    }
}
